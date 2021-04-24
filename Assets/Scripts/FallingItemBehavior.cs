using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingItemBehavior : MonoBehaviour
{
    Rigidbody rb;
    Vector3 maxVelocity;
    GameObject griffen;
    public static Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        maxVelocity = rb.velocity * 16;
        rb.velocity = rb.velocity * 4;
        rb.maxAngularVelocity = 30;
        griffen = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 100)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            griffen.transform.position = originalPosition;
            // FindObjectOfType<ObjectiveSliderBehavior>().ResetKeys();
           // FindObjectOfType<FallingFurnitureManager>().RespawnKeys();
            Destroy(transform.parent.gameObject);

        }
    }

}