using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    public GameObject dementorDie;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            destroyD();
        }
    }
    private void destroyD()
    {
        Instantiate(dementorDie, transform.position, transform.rotation);
        gameObject.SetActive(false);
        Destroy(gameObject, 1);
    }

}
