using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        if (FindObjectOfType<ObjectiveSliderBehavior>().isKeyCollected && other.CompareTag("Player")) {
            FindObjectOfType<ObjectiveSliderBehavior>().CompleteObjective(-1);
        }
    }
}
