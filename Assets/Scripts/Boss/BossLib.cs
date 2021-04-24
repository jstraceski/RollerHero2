using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLib : MonoBehaviour
{
    public static void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list, bool invert = false) {
        foreach (Transform child in parent) {
            if (!child.gameObject.CompareTag(tag) ^ invert) {
                list.Add(child.gameObject);
            }

            AddDescendantsWithTag(child, tag, list);
        }
    }
    
    
    public class BossPiece {
        public GameObject o;
        public Vector3 startingPos;
        public Quaternion startingRot;
        public bool hasRb = true;
        public Rigidbody rb;
        public int priority;

        public BossPiece(GameObject child, Vector3 transformPosition, Quaternion transformRotation, Rigidbody rbIn = null) {
            o = child;
            startingPos = transformPosition;
            startingRot = transformRotation;
            rb = rbIn;
        }
        public BossPiece(GameObject child) {
            o = child;
            startingPos = child.transform.localPosition;
            startingRot = child.transform.localRotation;
            rb = child.GetComponent<Rigidbody>();

            if (rb != null) {
                hasRb = true;
            }
            
            ShellBehavior shellBehavior = child.GetComponent<ShellBehavior>();
            if (shellBehavior != null) {
                priority = shellBehavior.priority;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
