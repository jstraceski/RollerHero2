using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeart : MonoBehaviour {
    public BossBehavior behavior;
    public float pullTime = 1f;
    public float pullReset = 0.1f;
    public float pullTimer = 0f;

    public float endTime = 3;

    public bool end = true;
    public bool pulling = false;
    public bool killed = false;
    public GameObject endItem;
    private GrappleBehavior grappleBehavior;
    public BossLib.BossPiece piece;

    public void Start() {
        piece = new BossLib.BossPiece(gameObject);
    }

    private void Update() {
        if (pulling && pullTimer <= pullTime - pullReset) {
            if (grappleBehavior.grappleState != GrappleBehavior.GrappleState.Tethered) {
                pulling = false;
            }
        }
        
        if (pulling && pullTimer > 0) {
            pullTimer -= Time.deltaTime;
        }

        if (pulling && pullTimer <= 0) {
            behavior.Kill();
            killed = true;
            pullTimer = endTime;
            pulling = false;
        }

        if (killed) {
            pullTimer -= Time.deltaTime;
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            
            if (rb != null) {
                foreach (var joint in gameObject.GetComponents<FixedJoint>()) {
                    Destroy(joint);
                }
                Destroy(rb);
            }

            var tf = piece.o.transform;
            tf.localPosition = Vector3.Lerp(tf.localPosition, piece.startingPos, Time.deltaTime);
            tf.localRotation = Quaternion.Lerp(tf.localRotation, piece.startingRot, Time.deltaTime);
            
            if (pullTimer <= 0) {
                End();
            }
        }
    }

    void End() {
        if (end) {
            end = false;
            gameObject.SetActive(false);
            endItem.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Grapple")) {
            grappleBehavior = other.gameObject.GetComponent<HookBehavior>().grappleBehavior;
            pulling = true;
            pullTimer = pullTime;
        }
    }
}
