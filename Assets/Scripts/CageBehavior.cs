using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBehavior : MonoBehaviour
{
    public bool cagemove;
    public float moveTime;
    void Start()
    {
        cagemove = false;
        moveTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(cagemove)
        {
            MoveCageUp();
        }
    }

    void MoveCageUp()
    {
        Vector3 pos = transform.position;
        moveTime += Time.deltaTime;
        transform.position = Vector3.Lerp(pos, pos + Vector3.up, Time.deltaTime);
        if(moveTime >= 2)
        {
            cagemove = false;
        }
    }
}
