using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockingBehavior : MonoBehaviour {
    public float rockSpeed;
    public float rockAngle;
    public float offset = 120;

    private Quaternion startingRot;
    // Start is called before the first frame update
    void Start() {
        startingRot = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = startingRot * Quaternion.Euler(0, 0, Mathf.Sin((Time.time * 360 * rockSpeed) + offset) * rockAngle);
    }
}
