using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDemo : MonoBehaviour
{
    public float speed = 0.5f;
    public float strength = 1f;
    public Vector3 axis = Vector3.forward;

    public bool pingPong = false;

    private Vector3 starttf;
    // Start is called before the first frame update
    void Start()
    {
        starttf = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (pingPong)
        {
            transform.position = starttf + axis.normalized * (Mathf.PingPong(Time.time * speed, 2) - 1)  * strength;
        }
        else
        {
            transform.position = starttf + axis.normalized * ((Mathf.Sin(Time.time * speed * 2 * Mathf.PI) * 2) - 1) * strength;
        }
    }
}
