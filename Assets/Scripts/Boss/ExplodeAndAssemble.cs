using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAndAssemble : MonoBehaviour {
    List<BossLib.BossPiece> pieces = new List<BossLib.BossPiece>();

    // Start is called before the first frame update
    void Start() {
        Explode();
    }

    public bool startAssembly = false;
    public bool isAssembled = false;

    // Update is called once per frame
    void Update() {
        if (startAssembly && !isAssembled) {
            float diff = 0;
            foreach (var piece in pieces) {
                if (piece.hasRb) {
                    foreach (var joint in gameObject.GetComponents<FixedJoint>()) {
                        Destroy(joint);
                    }
                    Destroy(piece.rb);
                    piece.hasRb = false;
                }

                var tf = piece.o.transform;

                tf.localPosition = Vector3.Lerp(tf.localPosition, piece.startingPos, Time.deltaTime);
                diff += (tf.localPosition - piece.startingPos).sqrMagnitude;

                tf.localRotation = Quaternion.Lerp(tf.localRotation, piece.startingRot, Time.deltaTime);
            }

            if (diff < 0.1) {
                isAssembled = true;
            }
        }
    }

    public void Explode() {
        List<GameObject> children = new List<GameObject>();
        BossLib.AddDescendantsWithTag(transform, "VisualOnly", children);


        foreach (var child in children) {
            child.AddComponent<Rigidbody>();
            
            var bPiece = new BossLib.BossPiece(child);
            
            bPiece.rb.velocity = (child.transform.position - transform.position).normalized * 20;

            pieces.Add(bPiece);
        }
    }
}