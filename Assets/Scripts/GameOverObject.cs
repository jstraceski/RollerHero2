using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverObject : MonoBehaviour {

    public TriggerScript triggerScript;
    public bool multiTrigger = false;
    private bool triggered = false;
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Grapple") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerColliders")) {
            Debug.Log("GAME OVER");
            if (multiTrigger || !triggered) {
                triggered = true;
                if (triggerScript) {
                    triggerScript.flag = true;
                }
            }
            
        }
    }
}
