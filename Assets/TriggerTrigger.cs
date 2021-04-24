using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrigger : MonoBehaviour
{
    public TriggerScript triggerScript;
    public bool stay = false;
    public bool autoReset = false;
    public float autoResetTime = 0.1f;
    private bool initState;
    private float autoResetTimer = 0;

    private void Start() {
        initState = triggerScript.flag;
    }

    private void Update() {
        if (autoReset) {
            if (autoResetTimer <= 0) {
                triggerScript.flag = initState;
            } else if (autoResetTimer > 0) {
                autoResetTimer -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!stay) {
            if (other.CompareTag("Player") || other.CompareTag("PlayerColliders")) {
                triggerScript.flag = true;
            }
        }
    }
    
    private void OnTriggerStay(Collider other) {
        if (stay) {
            if (other.CompareTag("Player") || other.CompareTag("PlayerColliders")) {
                triggerScript.flag = true;
                if (autoReset) {
                    autoResetTimer = autoResetTime;
                }
            }
        }
    }
}
