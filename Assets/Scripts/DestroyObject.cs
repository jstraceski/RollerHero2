using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DestroyObject : MonoBehaviour
{
    public Text text;
    public Transform itemPosition;
    public bool active = false;
    public float scaleSpeed = 1.0f;
    public float finalScale = 5.0f;
    public float moveSpeed = 1.0f;
    
    private void Update() {
        if (active) {
            Transform tf = gameObject.transform;
            gameObject.transform.localPosition = Vector3.Lerp(tf.localPosition, Vector3.zero, moveSpeed * Time.deltaTime);
            gameObject.transform.localScale = Vector3.Lerp(tf.localScale, Vector3.one * finalScale, scaleSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
            gameObject.transform.parent = GameObject.FindGameObjectWithTag("Item_Holder").transform;
            GetComponent<Collider>().enabled = false;
            active = true;
        }
    }
}
