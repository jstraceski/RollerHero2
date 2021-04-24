using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollect : MonoBehaviour {
    public bool notifyFallingFurniture = false;
    public DoorBehavior doorBehavior;
    public TriggerScript triggerScript;

    // Start is called before the first frame update
    void Start() {
    }

    private bool objectiveTriggered = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !objectiveTriggered) {
            if (doorBehavior) {
                doorBehavior.open = true;
            }
            if (triggerScript) {
                triggerScript.flag = true;
            }
            if (notifyFallingFurniture) {
                GameObject fallingManager = GameObject.FindGameObjectWithTag("FallingManager");
                fallingManager.GetComponent<FallingFurnitureManager>().KeyCollected();
            } else {
                objectiveTriggered = true;
                FindObjectOfType<ObjectiveSliderBehavior>().isKeyCollected = true;
            }
        }
    }
}