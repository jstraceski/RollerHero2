using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehavior : MonoBehaviour
{
    bool leverVariable;
    public TriggerScript triggerScript;
    void Start()
    {
        leverVariable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if(other.CompareTag("Grapple") && !leverVariable)
        {
            leverVariable = true;
            if (triggerScript) {
                triggerScript.flag = true;
            }

            FlipLever();
        }
    }

    void FlipLever()
    {
        transform.Rotate(180, 0 , 0);
        FindObjectOfType<CageBehavior>().cagemove = true;
    }
}
