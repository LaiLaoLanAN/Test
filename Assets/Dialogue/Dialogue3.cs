using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Dialogue3 : MonoBehaviour
{
    public BreakGround BG;
    private int startid;
    private int endid;
    private bool Dialoging=false;
    public string filePath;
    public CanvasGroup canvasGroup;
    public Image image;
    private string[] allLines;
    public TMP_Text tmptext;
    public DialogueData dialoguedata;
    public void SetDialogueID(int id){
        var mem=dialoguedata.GetPairById(id);
        SetDialogue(mem.startsub,mem.endsub-1);
    }
    public void SetDialogue(int sid,int eid){
        string targetLine=allLines[sid].Trim();
        string[] parts=targetLine.Split(new[]{' '},2);
        int Imagenum=int.Parse(parts[0]);
        string saying=parts[1];
        string Imagepath="Image/"+Imagenum;
        image.sprite=Resources.Load<Sprite>(Imagepath);
        tmptext.text=saying;
        startid=sid+1;
        endid=eid;
        StartCoroutine(Appear());
    }
    IEnumerator Appear(){
        float currentAlpha=0f;
        while(currentAlpha<1f){
            currentAlpha+=Time.deltaTime/0.75f;
            canvasGroup.alpha=currentAlpha;
            yield return null;
        }
        Dialoging=true;
    }
    IEnumerator Disappear(){
        float currentAlpha=1f;
        while(currentAlpha>0){
            currentAlpha-=Time.deltaTime/0.75f;
            canvasGroup.alpha=currentAlpha;
            yield return null;
            BG.F4Back();
        }

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
            if(startid>endid){
                Dialoging=false;
                StartCoroutine(Disappear());
            }
            else{
                string targetLine=allLines[startid].Trim();
                string[] parts=targetLine.Split(new[]{' '},2);
                int Imagenum=int.Parse(parts[0]);
                string saying=parts[1];
                string Imagepath="Image/"+Imagenum;
                image.sprite=Resources.Load<Sprite>(Imagepath);
                tmptext.text=saying;
            }            

            startid++;
        }
    }
}
