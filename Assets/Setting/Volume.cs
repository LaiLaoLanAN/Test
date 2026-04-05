using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value=AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float volume){
        AudioListener.volume=volume;
    }
}
