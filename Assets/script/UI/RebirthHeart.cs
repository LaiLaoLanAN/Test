using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RebirthHeart : MonoBehaviour
{
    public Image itimage;
    IEnumerator Appear(){
        float currentAlpha=0f;
        while(currentAlpha<1f){
            currentAlpha+=Time.deltaTime/0.5f;
            itimage.color=new Color(1f,1f,1f,currentAlpha);
            yield return null;
        }
    }
    public void Disappearf(){
        if(itimage.color.a==0){
            return;
        }
        StartCoroutine(Disappear());
    }
    IEnumerator Disappear(){
        float currentAlpha=1f;
        while(currentAlpha>0f){
            currentAlpha-=Time.deltaTime/0.5f;
            itimage.color=new Color(1f,1f,1f,currentAlpha);
            yield return null;
        }
    }
}