using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour,IDamagableE
{
    private Animator anim;
    public bool CanBite=false;
    [SerializeField]private float AnimPreparedTime;
    void Start()
    {
        anim=GetComponent<Animator>();
        // Invoke("RealPrepare",AnimPreparedTime);
    }
    public void DieOut(){
        anim.SetTrigger("Break");
        CanBite=false;
    }
    void AnimPrepare(){
        Invoke("RealPrepare",AnimPreparedTime);
    }
    void RealPrepare(){
        anim.SetTrigger("Prepare");
    }
    void PrepareEnd(){
        CanBite=true;
    }
}
