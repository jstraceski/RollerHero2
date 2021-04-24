using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyMove : MonoBehaviour {
    private Vector3 startPos;

    public float spookSpeed;
    public float spookAmplitude;
    public Vector3 spookDirection = Vector3.up;
    // Start is called before the first frame update
    void Start() {
        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        gameObject.transform.position = startPos + spookDirection * spookAmplitude * Mathf.Sin(spookSpeed * Time.time * 2 * Mathf.PI);
    }
}
