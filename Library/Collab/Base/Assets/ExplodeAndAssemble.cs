using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAndAssemble : MonoBehaviour
{
    
    private void AddDescendantsWithTag(Transform parent, string tag, List<GameObject> list)
    {
        foreach (Transform child in parent)
        {
            if (!child.gameObject.CompareTag(tag))
            {
                list.Add(child.gameObject);
            }
            AddDescendantsWithTag(child, tag, list);
        }
    }

    class BossPiece {
        public GameObject o;
        public Vector3 startingPos;
        public Quaternion startingRot;
        public bool hasRb = true;
        public Rigidbody rb;

        public BossPiece(GameObject child, Vector3 transformPosition, Quaternion transformRotation, Rigidbody rbIn) {
            o = child;
            startingPos = transformPosition;
            startingRot = transformRotation;
            rb = rbIn;
        }
    }
    
    List<BossPiece> pieces = new List<BossPiece>();
    
    // Start is called before the first frame update
    void Start() {
        List<GameObject> children = new List<GameObject>();
        AddDescendantsWithTag(transform, "VisualOnly", children);

        
        foreach (var child in children) {
            var ctf = child.transform;
            var bPiece = new BossPiece(child, ctf.localPosition, ctf.localRotation, child.AddComponent<Rigidbody>());
            bPiece.rb.velocity = (child.transform.position - transform.position).normalized * 20;
            
            pieces.Add(bPiece);
        }
        
        
    }

    public bool startAssembly = false;
    public bool isAssembled = false;

    // Update is called once per frame
    void Update()
    {
        if (startAssembly && !isAssembled) {
            float diff = 0;
            foreach (var piece in pieces) {
                if (piece.hasRb) {
                    Destroy(piece.rb);
                }

                var tf = piece.o.transform;
                
                tf.localPosition = Vector3.Lerp(tf.localPosition, piece.startingPos, Time.deltaTime);
                diff += (tf.localPosition - piece.startingPos).sqrMagnitude;
                
                tf.localRotation = Quaternion.Lerp(tf.localRotation, piece.startingRot, Time.deltaTime);
            }
            if (diff < 0.01) {
                isAssembled = true;
            }
        }

    }
}
