using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveShell : MonoBehaviour
{
    private List<BossLib.BossPiece> removedPieces = new List<BossLib.BossPiece>();
    private List<BossLib.BossPiece> restoringPieces = new List<BossLib.BossPiece>();
    
    
    
    // Start is called before the first frame update
    void Start() {
    }

    public void QueueRemoved(BossLib.BossPiece piece) {
        removedPieces.Add(piece);
    }

    public void RestoreRemoved(Vector3 location) {
        int priority = -1;
        BossLib.BossPiece restore = null;
        Debug.Log(removedPieces.Count + " | " + location);
        foreach (var piece in removedPieces) {
            if (piece.priority > priority) {
                restore = piece;
                priority = piece.priority;
            }
        }

        if (priority != -1 && restore != null) {
            restore.o.transform.position = location;
            restore.o.SetActive(true);
            
            Fade.setStates(restore.o, Fade.FadeState.UnFading);
            
            restoringPieces.Add(restore);
            removedPieces.Remove(restore);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < restoringPieces.Count; i++) {
            BossLib.BossPiece piece = restoringPieces[i];
            if (piece.hasRb) {
                foreach (var joint in gameObject.GetComponents<FixedJoint>()) {
                    Destroy(joint);
                }
                Destroy(gameObject.GetComponent<Rigidbody>());
                piece.hasRb = false;
            }

            var tf = piece.o.transform;

            tf.localPosition = Vector3.Lerp(tf.localPosition, piece.startingPos, Time.deltaTime);
            tf.localRotation = Quaternion.Lerp(tf.localRotation, piece.startingRot, Time.deltaTime);
            
            if ((tf.localPosition - piece.startingPos).sqrMagnitude < 0.01) {
                restoringPieces.Remove(piece);
                piece.o.GetComponent<Collider>().enabled = true;
                i--;
            }
        }
    }
}
