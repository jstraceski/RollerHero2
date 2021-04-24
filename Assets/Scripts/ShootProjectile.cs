using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProjectile : MonoBehaviour
{
    public float speed = 100f;
    public GameObject prefab;
    AudioClip sound;

    public Image reticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Fire1"))
		{
            GameObject shoot =
                Instantiate(prefab,
                transform.position +transform.forward,
                transform.rotation) as GameObject;

            shoot.GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.VelocityChange);
            shoot.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);
            AudioSource.PlayClipAtPoint(sound, transform.position);
		}
    }

    private void FixedUpdate()
    {
        ReticleEffect();
    }

    void ReticleEffect()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            if (hit.collider.CompareTag("Dementor"))
            {
                reticle.color = Color.red;
                reticle.transform.localScale = Vector3.Lerp(reticle.transform.localScale, new Vector3(.7f, .7f, 1), Time.deltaTime * 2);
            }
            else
            {
                reticle.color = Color.white;
                reticle.transform.localScale = Vector3.Lerp(reticle.transform.localScale, Vector3.one, Time.deltaTime * 2);
            }
        }

    }
}
