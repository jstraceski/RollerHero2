using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour {
    public TriggerScript triggerScript;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerScript.flag != audioSource.isPlaying) {
            if (!audioSource.isPlaying) {
                audioSource.Play();
            } else {
                audioSource.Pause();
            }
        }
    }
}
