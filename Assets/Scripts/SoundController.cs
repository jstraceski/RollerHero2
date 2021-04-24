using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundController : MonoBehaviour
{
    public static int volume = 20;
    [SerializeField]
    Slider slider;
    // Update is called once per frame
    private void Start()
    {
        changeVolume();
    }
    public void changeVolume()
    {
        AudioListener.volume = volume / 100f;
    }

    public void adjustVolumeValue(int val)
    {
        volume = (int)(slider.value);
        Debug.Log(volume);
    }
}
