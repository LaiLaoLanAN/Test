using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Dialogue1 : MonoBehaviour
{
    private int startid;
    private int endid;
    private bool Dialoging=false;
    public string filePath ="dialogue";
    public CanvasGroup canvasGroup;
    public Image image;
    public Background BG;
    private string[] allLines;
    public TMP_Text tmptext;
    public DialogueData dialoguedata;
    public void SetDialogueID(int id){
        var mem=dialoguedata.GetPairById(id);
        SetDialogue(mem.startsub,mem.endsub-1);
    }
    private void SetDialogue(int sid,int eid){
        startid=sid;
        endid=eid;
        StartCoroutine(Appear());
    }
    IEnumerator Appear(){
        yield return new WaitForSeconds(1f);
        float currentAlpha=0f;
        while(currentAlpha<1f){
            currentAlpha+=Time.deltaTime/0.75f;
            canvasGroup.alpha=currentAlpha;
            yield return null;
        }
        Dialoging=true;
    }
    IEnumerator Disappear(){
        yield return new WaitForSeconds(1f);
        float currentAlpha=1f;
        while(currentAlpha>0){
            currentAlpha-=Time.deltaTime/1.5f;
            canvasGroup.alpha=currentAlpha;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        BG.EnterGame();
    }
    // Start is called before the first frame update
    void Start()
    {
        allLines=Resources.Load<TextAsset>(filePath).text.Split(new[]{"\r\n","\r","\n"},StringSplitOptions.None);
        canvasGroup.alpha=0f;
        tmptext.text="";
    }

    // Update is called once per frame
    void Update()
    {
        if(Dialoging&&Input.GetMouseButtonDown(0)){
            string targetLine=allLines[startid].Trim();
            string[] parts=targetLine.Split(new[]{' '},2);
            int Imagenum=int.Parse(parts[0]);
            string saying=parts[1];
            string Imagepath="Image/"+Imagenum;
            image.sprite=Resources.Load<Sprite>(Imagepath);
            tmptext.text=saying;

            if(startid==endid){
                Dialoging=false;
                StartCoroutine(Disappear());
            }
            startid++;
        }
    }
}
