using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnTrigger : MonoBehaviour {
    public float delay = 0.5f;
    public TriggerScript triggerScript;

    // Update is called once per frame
    void Update()
    {
        if (triggerScript.flag) {
            Destroy(gameObject, delay);
        }   
    }
}
