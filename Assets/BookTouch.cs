using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTouch : MonoBehaviour
{
    public bool touchedBook;

    // Start is called before the first frame update
    void Start()
    {
        touchedBook = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if(other.CompareTag("Player"))
        {
            touchedBook = true;
        }
    }
}