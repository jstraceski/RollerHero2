using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour {
    public RemoveShell removeShell;
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerColliders")) {
            removeShell.RestoreRemoved(gameObject.transform.position);
            Destroy(gameObject);
        }
    }
}
