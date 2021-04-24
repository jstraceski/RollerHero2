using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpLib : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void LookAtSlerp(Transform tf, Vector3 target, float dt, bool yOnly = false) {
        if (yOnly) {
            target.y = tf.position.y;
        }
        tf.rotation = Quaternion.Slerp( tf.rotation, Quaternion.LookRotation( target - tf.position ), dt);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
