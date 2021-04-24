using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidOnTrigger : MonoBehaviour {
    public TriggerScript triggerScript;

    private Collider collider;
    // Start is called before the first frame update
    void Start() {
        collider = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerScript.flag) {
            collider.enabled = true;
        }
    }
}
