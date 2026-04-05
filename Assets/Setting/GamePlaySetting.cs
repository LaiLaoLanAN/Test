using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySetting : MonoBehaviour
{
    public float IODuration;
    public CanvasGroup canvasGroup;
    private bool IsSetting=false;
    public Exit exit;
    public AllScreenButton allscreenButton;
    [Header("教程")]
    private bool IsGuide=false;
    public CanvasGroup GuidecanvasGroup;
    public CanvasGroup MaincanvasGroup;
    void Start(){
        canvasGroup.alpha=0f;
        canvasGroup.interactable=false;
        exit.enabled=false;
        allscreenButton.enabled=false;
        GuidecanvasGroup.interactable=false;
        GuidecanvasGroup.blocksRaycasts=false;
        GuidecanvasGroup.alpha=0f;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            menu();
        }
    }
    public void menu(){
        IsSetting=!IsSetting;
        if(IsSetting){
            IsGuide=false;
            GuidecanvasGroup.interactable=false;
            GuidecanvasGroup.blocksRaycasts=false;
            MaincanvasGroup.interactable=true;
            GuidecanvasGroup.alpha=0f;
            exit.enabled=true;
            allscreenButton.enabled=true;
            StartCoroutine(Appear());
        }
        else{
            exit.enabled=false;
            allscreenButton.enabled=false;
            StartCoroutine(Disappear());
        }
    }
    public void guide(){
        IsGuide=!IsGuide;
        if(IsGuide){
            GuidecanvasGroup.interactable=true;
            GuidecanvasGroup.blocksRaycasts=true;
            MaincanvasGroup.interactable=false;
            GuidecanvasGroup.alpha=1f;
            exit.enabled=false;
            allscreenButton.enabled=false;
        }
        else{
            GuidecanvasGroup.interactable=false;
            GuidecanvasGroup.blocksRaycasts=false;
            MaincanvasGroup.interactable=true;
            GuidecanvasGroup.alpha=0f;
            exit.enabled=true;
            allscreenButton.enabled=true;
        }
    }
    IEnumerator Appear(){
        float currentAlpha=0f;
        while(currentAlpha<1f){
            currentAlpha+=Time.deltaTime/IODuration;
            canvasGroup.alpha=currentAlpha;
            yield return null;
        }
        canvasGroup.interactable=true;
        Time.timeScale=0f;
    }
    IEnumerator Disappear(){
        Time.timeScale=1f;
        float currentAlpha=1f;
        while(currentAlpha>0){
            currentAlpha-=Time.deltaTime/IODuration;
            canvasGroup.alpha=currentAlpha;
            yield return null;
        }
        canvasGroup.interactable=false;
    }
}
