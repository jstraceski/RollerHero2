using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    public int start = 100;
    public int current;
    public AudioClip dead;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        current = start;
        slider.value = current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amt)
    {
        current -= amt;
        if (current < 0)
        {
            current = 0;
            playerDies();
        }
        slider.value = current;
    }

    private void playerDies()
    {
        gameObject.transform.Rotate(-90, 0, 0, Space.Self);
        AudioSource.PlayClipAtPoint(dead, transform.position);


    }
}
