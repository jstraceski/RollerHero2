using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBehavior : MonoBehaviour {
    public bool hooked = false;
    public GrappleBehavior grappleBehavior;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private String[] nonTetherTags = {"PlayerColliders", "Player", "NoTether"};

        // Start is called before the first frame update
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _collider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool CompareTags(GameObject o, String[] tags) {
        foreach (var tag in tags) {
            if (o.CompareTag(tag)) {
                return true;
            }
        }

        return false;
    }

    public void AddRigidBody(GameObject other, Rigidbody rb = null) {
        if (rb == null) {
            rb = other.GetComponent<Rigidbody>();
        }
        
        if (rb != null) {
            FixedJoint fj = gameObject.AddComponent<FixedJoint>();
            fj.connectedBody = rb;
            _rigidbody.isKinematic = false;
        } else {
            _rigidbody.isKinematic = true;
        }

        gameObject.transform.SetParent(other.transform, true);
    }
    
    private void OnCollisionEnter(Collision other) {
        if (!hooked && !CompareTags(other.gameObject, nonTetherTags)) {
            hooked = true;
            AddRigidBody(other.gameObject);
        }
    }
}
