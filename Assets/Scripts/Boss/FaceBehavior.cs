using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBehavior : MonoBehaviour {
    public enum MouthState {
        Breathe, Idle, Static
    }
    
    public enum EyeState {
        Stare, Idle, Static
    }
    
    public GameObject[] eyes;
    public GameObject mouthTop;
    public GameObject mouthBottom;

    
    public float openDistance = 0;
    public GameObject lookTarget;
    public float lookAngle = 45f;
    public float breatheSpeed = 1;
    public float breatheAmp = 2f;
    public float snapSpeed = 3f;
    
    private Vector3 _topStart;
    private Vector3 _bottomStart;

    public MouthState mouthState = MouthState.Breathe;
    public EyeState eyeState = EyeState.Stare;
    public Transform faceTf;
    
    // Start is called before the first frame update
    void Start() {
        _topStart = mouthTop.transform.localPosition;
        _bottomStart = mouthBottom.transform.localPosition;
        faceTf = transform;

        if (!lookTarget) {
            lookTarget = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (eyeState == EyeState.Stare) {
            if (Vector3.Angle(transform.forward, lookTarget.transform.position - transform.position) < lookAngle) {
                foreach (var eye in eyes) {
                   SlerpLib.LookAtSlerp(eye.transform, lookTarget.transform.position, Time.deltaTime * snapSpeed);
                }
            } else {
                foreach (var eye in eyes) {
                    SlerpLib.LookAtSlerp(eye.transform, eye.transform.position + transform.forward, Time.deltaTime * snapSpeed);
                }
            }
        }

        if (mouthState == MouthState.Breathe) {
            openDistance = Mathf.Sin(Time.time * 360 * breatheSpeed) * breatheAmp * 0.5f + breatheAmp * 0.5f;
        }

        if (mouthState == MouthState.Static) {
            return;
        }

        Vector3 topPos = mouthTop.transform.localPosition;
        Vector3 topPosTarget = _topStart;
        topPosTarget.y += openDistance * 0.5f;
        mouthTop.transform.localPosition = Vector3.Lerp(topPos, topPosTarget, Time.deltaTime * snapSpeed);
        
        Vector3 bottomPos = mouthBottom.transform.localPosition;
        Vector3 bottomPosTarget = _bottomStart;
        bottomPosTarget.y -= openDistance * 0.5f;
        mouthBottom.transform.localPosition = Vector3.Lerp(bottomPos, bottomPosTarget, Time.deltaTime * snapSpeed);
    }

    private void OnDrawGizmos() {
        if (lookTarget) {
            Debug.DrawLine(transform.position, lookTarget.transform.position, Color.cyan);
        }
    }
}
