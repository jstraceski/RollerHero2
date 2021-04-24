using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTouch : MonoBehaviour
{
    public bool touchedChest;

    // Start is called before the first frame update
    void Start()
    {
        touchedChest = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            touchedChest = true;
        }
    }
}
