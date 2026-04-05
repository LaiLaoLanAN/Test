using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHeart : MonoBehaviour
{
    public Image InsideImage;
    private BeatManager beatManager;
    void Start()
    {
        beatManager = BeatManager.Instance;
    }
    void Update()
    {
        if (beatManager.IsPlaying&&!beatManager.IsRecording)
        {
            InsideImage.fillAmount = 1- beatManager.GetFillAmount();
        }
    }
}
