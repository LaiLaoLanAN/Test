using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Background : MonoBehaviour
{
    public SpriteRenderer SRE3;
    public SpriteRenderer SRE4;
    public SpriteRenderer SRE5;
    public SpriteRenderer SRE1;
    public SpriteRenderer SRE2;
    private Animator anim;
    public Dialogue1 Dlg;
    public float fadeduration=10f;
    public void StartGame(){
        StartCoroutine(FadeOut());

    }
    IEnumerator FadeOut(){
        float currentAlpha=1f;
        while(currentAlpha>0){
            currentAlpha-=Time.deltaTime/fadeduration;
            SRE3.color=new Color(1f,1f,1f,currentAlpha);
            SRE4.color=new Color(1f,1f,1f,currentAlpha);
            SRE5.color=new Color(1f,1f,1f,currentAlpha);
            SRE1.color=new Color(1f,1f,1f,currentAlpha);
            yield return null;
        }
        SRE3.enabled=false;
        SRE4.enabled=false;
        SRE5.enabled=false;
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("Change");

        
    }
    void Start(){
        anim=GetComponent<Animator>();
    }
    void EnterDialogue(){
        Dlg.SetDialogueID(0);
    }
    public void EnterGame(){
        StartCoroutine(EnterG());
    }
    
    IEnumerator EnterG(){
        float currentAlpha=1f;
        while(currentAlpha>0){
            currentAlpha-=Time.deltaTime/fadeduration;
            SRE2.color=new Color(1f,1f,1f,currentAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
