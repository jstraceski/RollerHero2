using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBehavior : MonoBehaviour {
    public int priority = 0;
    public float pullTime = 0.5f;
    public float pullReset = 0.1f;
    public float vaporizeTime = 1f;

    public float pullTimer = 0;
    public bool pulling = false;
    public bool assembled = false;
    private RemoveShell removeShell;
    private GameObject grapple;
    private HookBehavior hookBehavior;
    private GrappleBehavior grappleBehavior;
    private BossLib.BossPiece restoreState;
    
    
    public static void setAssembled(GameObject gameObject, bool assembledState) {
        setAssembled(gameObject.transform, assembledState);
    }
    
    public static void setAssembled(Transform tf, bool assembledState) {
        ShellBehavior shellBehavior = tf.gameObject.GetComponent<ShellBehavior>();
        if (shellBehavior != null) {
            shellBehavior.assembled = assembledState;
        }
        foreach (Transform childTf in tf) {
            shellBehavior = childTf.gameObject.GetComponent<ShellBehavior>();
            if (shellBehavior != null) {
                shellBehavior.assembled = assembledState;
            }

            ShellBehavior.setAssembled(childTf, assembledState);
        }

    }
    

    // Start is called before the first frame update
    void Start() {
        removeShell = GetComponentInParent<RemoveShell>();
        restoreState = new BossLib.BossPiece(gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (pulling && pullTimer > 0) {
            pullTimer -= Time.deltaTime;
        }

        if (pulling && pullTimer <= pullTime - pullReset) {
            if (grappleBehavior.grappleState != GrappleBehavior.GrappleState.Tethered) {
                pulling = false;
            }
        }

        if (pulling && pullTimer <= 0) {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 0.2f;
            hookBehavior.AddRigidBody(gameObject, rb);
            gameObject.GetComponent<Collider>().enabled = false;

            pulling = false;
            grappleBehavior.autoPull = true;
            
            Fade.setStates(gameObject, Fade.FadeState.Fading);

            Invoke(nameof(Vaporize), vaporizeTime);
        }
    }

    void Vaporize() {
        // gameObject.SetActive(false);
        removeShell.QueueRemoved(restoreState);
        foreach (var joint in gameObject.GetComponents<FixedJoint>()) {
            Destroy(joint);
        }
        Destroy(gameObject.GetComponent<Rigidbody>());

       
        if (grappleBehavior.grappleState == GrappleBehavior.GrappleState.Tethered) {
            grappleBehavior.Retract();
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (assembled && !pulling && other.gameObject.CompareTag("Grapple")) {
            grapple = other.gameObject;
            grapple.GetComponent<Collider>().enabled = false;

            hookBehavior = grapple.GetComponent<HookBehavior>();
            grappleBehavior = hookBehavior.grappleBehavior;

            pullTimer = pullTime;
            pulling = true;
        }
    }
}