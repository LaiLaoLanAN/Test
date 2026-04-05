using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class Orange : MonoBehaviour,IDamagableE
{
    private CinemachineVirtualCamera vcam;
    private MCscript MC;
    private Transform MCtransform;
    private Animator anim;
    private Rigidbody2D rb;
    [Header("z")]
    private Vector3 velocity;
    private bool Ischasing=false;
    private bool IsFalling=false;
    private float Xdistance;
    private Vector2 target;
    [SerializeField]private float smoothTime;
    [SerializeField]private float FloatingHeight;
    [Header("Fall")]
    public LayerMask groundLayer;  
    private Coroutine FallCoroutine;
    [SerializeField]private float FallCheckDistance;
    [SerializeField]private float FallCheckTime;
    [SerializeField]private float FallTime;
    public AnimationCurve FallCurve;
    [SerializeField]private float ResetTime;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        MC=FindObjectOfType<MCscript>();
        MCtransform=MC.transform;
        velocity=Vector3.zero;
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Ischasing){
            if(!IsFalling){
                target=MCtransform.position+new Vector3(0,FloatingHeight);
                transform.position = Vector3.SmoothDamp(transform.position,target,ref velocity,smoothTime);
                Xdistance=Math.Abs(MCtransform.position.x-transform.position.x);
                if(Xdistance<FallCheckDistance&&MCtransform.position.y<transform.position.y){
                    if(FallCoroutine==null){
                        FallCoroutine=StartCoroutine(CheckFall());
                    }
                }
            }
        }
        else if(CheckInCamera()){
            Ischasing=true;
        }
    }
    public void DieOut(){
        MC.AddScore(1.2f);
        Destroy(gameObject);
    }
    IEnumerator CheckFall(){
        float AddTime=0;
        while(Xdistance<FallCheckDistance&&AddTime<FallCheckTime){
            yield return null;
            AddTime+=Time.deltaTime;
        }
        if(AddTime>=FallCheckTime){
            IsFalling=true;
            StartCoroutine(FallGround());
        }
        FallCoroutine=null;
    }
    IEnumerator FallGround(){
        float OriginY=transform.position.y;
        float OriginX=transform.position.x;
        float FallDistance;
        RaycastHit2D hit;
        hit=Physics2D.Raycast(transform.position,Vector2.down,200f,groundLayer);
        if(hit.collider!=null){
            FallDistance=hit.distance+0.3f;
        }
        else{
            FallDistance=200f;
        }
        anim.SetTrigger("Down");
        float AddTime=0;
        while(AddTime<FallTime){
            AddTime+=Time.deltaTime;
            transform.position=new Vector2(OriginX,OriginY-FallDistance*FallCurve.Evaluate(AddTime/FallTime));
            yield return null;
        }
        anim.SetTrigger("Crash");
        yield return new WaitForSeconds(ResetTime);
        IsFalling=false;
    }
    bool CheckInCamera(){
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        return (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0);
    }
}
