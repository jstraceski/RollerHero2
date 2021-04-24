using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    Transform playerBody;
    
    public Vector3 shotDir;
    public bool firstPerson = false;
    public GameObject third;
    public GameObject pov;
    
      
    
    private float x, xm, xl;  
    private float y, ym, yl;
     
    public float sensitivity;
    public float returnSpeed;

    public bool looking = false;
    
    void Awake()
    {
        Vector3 euler = transform.localRotation.eulerAngles;
        x = euler.x;
        y = euler.y;
    }
     
    void Update()
    {
        third.SetActive(true);
        const float MIN_X = -120.0f;
        const float MAX_X = 120.0f;
        const float MIN_Y = -90.0f;
        const float MAX_Y = 90.0f;


        if (Input.GetMouseButton(1)) { 
            Cursor.lockState = CursorLockMode.Locked;
            looking = true;

            xm = Input.GetAxis("Mouse X");
            ym = Input.GetAxis("Mouse Y");

            x += xm * (sensitivity * Time.deltaTime);
            y -= ym * (sensitivity * Time.deltaTime);

            if (x < MIN_X) x = MIN_X;
            else if (x > MAX_X) x = MAX_X;
            
            if (y < MIN_Y) y = MIN_Y;
            else if (y > MAX_Y) y = MAX_Y;
            
            transform.localRotation = Quaternion.Euler(y, x, 0.0f);
        } else {
            looking = false;
            Cursor.lockState = CursorLockMode.None;
            
            Vector3 euler = Vector3.Lerp(new Vector3(y, x, 0), Vector3.zero, Time.deltaTime * returnSpeed);

            y = euler.x;
            x = euler.y;

            transform.localRotation = Quaternion.Euler(euler);
        }
    }
}
