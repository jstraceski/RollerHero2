using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinner : MonoBehaviour {
    public float spinSpeed;
    public float sinSpeed;
    public float sineAmplitude;

    public GameObject center;
    public GameObject arm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        center.transform.rotation *= Quaternion.AngleAxis(spinSpeed * Time.deltaTime, center.transform.up);
        Vector3 armPos = arm.transform.localPosition;
        armPos.y = sineAmplitude * Mathf.Sin(sinSpeed * Time.time);
        arm.transform.localPosition = armPos;
    }
}
