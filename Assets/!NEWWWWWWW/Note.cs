using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [HideInInspector]public float TotalX;
    [HideInInspector]public float TotalWindowTime;
    [HideInInspector] public int CurrentI;
    private Image image;
    public float FadeDuration;
    private BeatManager beatManager;

    private RectTransform RT;
    private bool IsFade=false;
    private void Start()
    {
        RT = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);
        StartCoroutine(Appear());
        beatManager = BeatManager.Instance;
    }
    private void Update()
    {
        RT.localPosition = new Vector3(TotalX*(beatManager.NotePos(CurrentI) / TotalWindowTime),0,0);
        if (beatManager.CurrentBeatNum > CurrentI&&!IsFade)
        {
            IsFade = true;
            StartCoroutine(Disappear(beatManager.BeatCatched[CurrentI]));
        }
    }
    IEnumerator Appear()
    {
        float Timer = 0;
        while (Timer < FadeDuration)
        {
            image.color = new Color(1,1,1,Timer/FadeDuration);
            Timer += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator Disappear(bool IsCatch)
    {
        float Timer = 0;
        if (IsCatch)
        {
            while (Timer < FadeDuration)
            {
                float x = 1 - Timer / FadeDuration;
                image.color = new Color(x, 1, x, x);
                Timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (Timer < 3 * FadeDuration)
            {
                float x = 1 - Timer / (3 * FadeDuration);
                image.color = new Color(1, x, x, x);
                Timer += Time.deltaTime;
                yield return null;
            }
        }
        Destroy(gameObject);
    }
}
