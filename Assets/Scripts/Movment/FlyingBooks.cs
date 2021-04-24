using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBooks : MonoBehaviour
{
    [SerializeField]
    float size;
    [SerializeField]
    Vector3 centerOffset;
    [SerializeField]
    Vector3 facingPointOffset;

    [SerializeField]
    float speed;
    Vector3 centerPoint;
    Vector3 facingPoint;


    // Start is called before the first frame update
    void Start()
    {
        centerPoint = transform.position + centerOffset;
        facingPoint = transform.position + facingPointOffset;
        transform.position = new Vector3(centerPoint.x + size, centerPoint.y, centerPoint.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(facingPoint);
        transform.RotateAround(centerPoint, new Vector3(0, 1, 0), speed * Time.deltaTime);
    }
}
