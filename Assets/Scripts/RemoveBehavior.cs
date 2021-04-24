using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float removeTime = 1;
    void Start()
    {
        Invoke(nameof(Remove), removeTime);
    }

    public void Remove() {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
