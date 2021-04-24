using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour {
    public TriggerScript triggerScript;

    public bool multiTrigger = false;
    private bool triggered = false;
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerColliders")) {
            if (multiTrigger || !triggered) {
                triggered = true;
                triggerScript.flag = true;
            }
        }
    }
    
}
