using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    Vector3 direct;
    [SerializeField]
    float dist;
    Vector3 init;

    [SerializeField]
    float rspeed;
    [SerializeField]
    Vector3 rdirect;
    [SerializeField]
    [Tooltip("Distance to rotate before switching directions, set to 0 to spin continuously")]
    float rdist;
    Quaternion rinit;
    // Start is called before the first frame update
    void Start()
    {
        direct = direct.normalized;
        init = transform.position;
        rdirect = rdirect.normalized;
        rinit = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mov = new Vector3(direct.x * Mathf.Sin(speed * Time.time) + init.x, direct.y * Mathf.Sin(speed * Time.time) * dist + init.y, direct.z * Mathf.Sin(speed * Time.time)  + init.z);
        transform.position = mov;
        Vector3 rotWay;
        if(rdist == 0)
        {
            rotWay = new Vector3(rdirect.x * rspeed * Time.time, rdirect.y * rspeed * Time.time, rdirect.z * rspeed * Time.time);
        }
        else
        {
            rotWay = new Vector3(rdirect.x * Mathf.Sin(rspeed * Time.time) * rdist,
                rdirect.y * Mathf.Sin(rspeed * Time.time) * rdist,
                rdirect.z * Mathf.Sin(rspeed * Time.time) * rdist);
        }
        Quaternion rot = Quaternion.Euler(rotWay) *rinit;
        transform.rotation = rot;
    }
}
