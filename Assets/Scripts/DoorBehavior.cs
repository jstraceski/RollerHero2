using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour {
    public float dSpeed = 1.0f;
    public bool open = false;
    public Transform fTf;
    public TriggerScript triggerScript;
    
    private Transform sTf;
    private Transform tf;
    private bool openState = false;
    
    
    
    // Start is called before the first frame update
    void Start() {
        sTf = new GameObject().transform;
        sTf.localPosition = transform.localPosition;
        sTf.localRotation = transform.localRotation;
        sTf.localScale = transform.localScale;
        
        tf = transform;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (triggerScript) {
            open = triggerScript.flag;
        }
        
        if (openState != open) {
            if (open) {
                tf.localScale = Vector3.Lerp(tf.localScale, fTf.localScale, Time.deltaTime * dSpeed);
                tf.localPosition = Vector3.Lerp(tf.localPosition, fTf.localPosition, Time.deltaTime * dSpeed);
                tf.localRotation = Quaternion.Lerp(tf.localRotation, fTf.localRotation, Time.deltaTime * dSpeed);

                float diff = (tf.localPosition - fTf.localPosition).sqrMagnitude
                             + (tf.localScale - fTf.localScale).sqrMagnitude
                             + Quaternion.Angle(fTf.localRotation, tf.localRotation);
                if (diff < 0.1f) {
                    openState = open;
                }
            } else {
                tf.localScale = Vector3.Lerp(tf.localScale, sTf.localScale, Time.deltaTime * dSpeed);
                tf.localPosition = Vector3.Lerp(tf.localPosition, sTf.localPosition, Time.deltaTime * dSpeed);
                tf.localRotation = Quaternion.Lerp(tf.localRotation, sTf.localRotation, Time.deltaTime * dSpeed);
                
                float diff = (tf.localPosition - fTf.localPosition).sqrMagnitude
                             + (tf.localScale - fTf.localScale).sqrMagnitude
                             + Quaternion.Angle(fTf.localRotation, tf.localRotation);
                if (diff < 0.1f) {
                    openState = open;
                }
            }
        }
    }
}
