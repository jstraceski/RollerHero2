using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour {
    public TriggerScript triggerScript;

    private Light light;
    // Start is called before the first frame update
    void Start() {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update() {
        light.enabled = triggerScript.flag;
    }
}
