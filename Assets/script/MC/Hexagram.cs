using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagram : MonoBehaviour
{
    public Transform MCtransform;
    private MCscript MC;
    private SpriteRenderer SR;
    public Vector3 AdjustPosition;
    private float currenttime;
    private float ProcessTime;
    void OnTriggerEnter2D(Collider2D obj){
        IDamagableE iDamagableE=obj.GetComponent<IDamagableE>();
        if(iDamagableE!=null){
            iDamagableE.DieOut();
        }
    }
    void Start(){
        SR=GetComponent<SpriteRenderer>();
        MC=MCtransform.GetComponent<MCscript>();
        ProcessTime=MC.RebirthInvincibleTime;
    }
    void OnEnable(){
        currenttime=0;
    }
    void LateUpdate(){
        currenttime+=Time.deltaTime;
        MC.eatscore=0;
        MC.MoreScore=0;
        SR.color=new Color(1,1,1,1-currenttime/ProcessTime);
        transform.position=MCtransform.position+AdjustPosition;
    }
}
