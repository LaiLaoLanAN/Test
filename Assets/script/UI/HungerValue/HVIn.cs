using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HVIn : MonoBehaviour
{
    public RebirthHeart RH;
    public GameObject HVMoreBall;
    public bool IsLeft;
    private int BallNum=0;
    public void HVMore(float _score){
        BallNum+=(int)(IsLeft?Math.Ceiling(_score/6):Math.Floor(_score/6));
        Invoke("BallShoot",UnityEngine.Random.Range(0.1f,0.3f));
    }
    void BallShoot(){
        if(BallNum==0){
            return;
        }
        BallNum-=1;
        GameObject HVmore=Instantiate(HVMoreBall,transform);
        HVmore.transform.SetParent(transform.parent.parent,true);
        HVmore.GetComponent<HVMore>().RH=RH;
        Invoke("BallShoot",UnityEngine.Random.Range(0.1f,0.3f));
    }
}
