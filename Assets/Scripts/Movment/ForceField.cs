using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour {
    public float force = 9.8f * 2.0f;

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("PlayerColliders")) {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb) {
                rb.AddForce((rb.position - transform.position).normalized * force);
            }
        }
    }
}
