using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Stage4Manager : MonoBehaviour
{
    public MCscript MC;
    public bool IsChangingDemon=false;
    public Dialogue4 D4;
    public UnityEvent StartFight;
    public GameObject broad;
    public GameObject Shootdemonbroad;
    public GameObject demonbroad;
    [Header("泡泡")]
    public GameObject Bubble1;
    public GameObject Bubble2;
    [Header("摄像机")]
    public CinemachineVirtualCamera virtualCamera;
    public float CameraOriginalSize;
    public float CameraBigSize;
    public float CameraBigTime;
    public AnimationCurve CameraBigCurve;
    [Header("最后咬")]
    public GameObject Bite1;
    public GameObject Bite2;
    public LastBITE lastbite;
    public GameObject DemonBroad2;
    public GameObject LastTerrin;
    public MCCollider MCcollider;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("BossMet",0)==1){
            broad.SetActive(false);
            Bubble1.SetActive(true);
            Bubble2.SetActive(true);
            demonbroad.SetActive(true);
            virtualCamera.m_Lens.OrthographicSize=CameraBigSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsChangingDemon){
            MC.rb.velocity=new Vector2(0,MC.rb.velocity.y);
        }
    }
    public void DemonDialogue(){
        MC.IsReading=true;
        MC.anim.SetInteger("State",0);
        MC.transform.localScale=new Vector2(10*MC.LocalScaleLock,10);
        D4.SetDialogue(13,18);
    }
    public void startfight(){
        IsChangingDemon=false;
        demonbroad.SetActive(true);
        Destroy(Shootdemonbroad);
        StartCoroutine(CameraBig());
        Bubble1.SetActive(true);
        Bubble2.SetActive(true);
        PlayerPrefs.SetInt("BossMet",1);
    }
    IEnumerator CameraBig(){
        float AddTime=0;
        while(AddTime<CameraBigTime){
            virtualCamera.m_Lens.OrthographicSize=CameraOriginalSize+CameraBigCurve.Evaluate(AddTime/CameraBigTime)*(CameraBigSize-CameraOriginalSize);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
    public void LastBite(){
        Bite1.SetActive(false);
        Bite2.SetActive(true);
    }
    public void MCStop(){
        MC.anim.speed=0f;            //调回原速在LastBITE脚本中
        MC.rb.bodyType=RigidbodyType2D.Static;
        MC.transform.localScale=new Vector2(10*MC.LocalScaleLock,10);
        MC.IsReading=true;
        D4.SetDialogue(19,20);
        StartCoroutine(Beast());
    }
    IEnumerator Beast(){
        yield return new WaitUntil(()=>!MC.IsReading);
        MC.IsReading=true;
        lastbite.IsLastBite=true;
    }
    public void LastBiteEnd(){
        MC.anim.speed=0f;
        D4.SetDialogue(21,21);
        StartCoroutine(BroadDisappear());
    }
    IEnumerator BroadDisappear(){
        yield return new WaitUntil(()=>!MC.IsReading);
        Destroy(DemonBroad2);
        Destroy(LastTerrin);
        lastbite.anim.SetTrigger("LastBiteGo3");
        MC.rb.bodyType=RigidbodyType2D.Dynamic;
        MC.BiteGround(5);
        yield return new WaitForSeconds(7f);
        MC.IsReading=true;
        D4.SetDialogue(22,26);
        yield return new WaitUntil(()=>!MC.IsReading);
        yield return new WaitForSeconds(3f);
    }
}
