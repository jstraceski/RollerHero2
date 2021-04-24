using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour {
    public Transform respawnLocation;
    public float minHeight;
    public bool hasHeight = true;
    // Start is called before the first frame update
    void Start()
    {
    }


    //failsafe
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.transform.position.y < minHeight)
        {
            if (hasHeight) {
                player.GetComponent<ForceController>().Respawn();
            } else if (respawnLocation) {
                player.transform.position = respawnLocation.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (hasHeight) {
                other.GetComponent<ForceController>().Respawn();
            } else if (respawnLocation) {
                other.transform.position = respawnLocation.position;
            }
        }
    }
}
