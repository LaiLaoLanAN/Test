using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class Banana : MonoBehaviour,IUpable,IDamagableE
{
    private CinemachineVirtualCamera vcam;
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groudLayer;
    [SerializeField] private LayerMask playerLayer;
    private LineRenderer LR;
    private PolygonCollider2D PC2D;
    private CapsuleCollider2D CC2D;

    private bool Ischasing=false;
    private bool Flying=false;
    private bool IsDeadShoot=false;
    private float Xdistance;
    private int XDirection;
    private int MoveDirection=1;
    private MCscript MC;
    private Transform player;
    private MCCollider MCcollider;

    private Vector2 realposition;
    private Vector2 realplayerposition;
    private Vector2 Direction;
    public RaycastHit2D hitGround;
    public RaycastHit2D hitPlayer;


    [SerializeField]private float isGroundCheckbanana;
    [SerializeField]private float chasingspeed;
    [SerializeField]private float FrictionAcceleration;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
        LR=GetComponent<LineRenderer>();
        LR.startWidth=0.3f;
        LR.endWidth=0.3f;
        groudLayer=LayerMask.GetMask("Ground");
        PC2D=GetComponent<PolygonCollider2D>();
        CC2D=GetComponent<CapsuleCollider2D>();
        MC=FindObjectOfType<MCscript>();
        player=MC.transform;
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        MCcollider=FindObjectOfType<MCCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        XDirection=Math.Sign(player.position.x-transform.position.x);
        Xdistance=Math.Abs(player.position.x-transform.position.x);
        if(!Ischasing&&CheckInCamera()){
            Ischasing=true;
        }
        if(Ischasing&&!Flying){
            anim.SetBool("Moving",true);
            if(Xdistance<15f){
                MoveDirection=-XDirection;
            }
            else if(Xdistance<25f){
                Flying=true;
                anim.SetBool("Moving",false);
                anim.SetBool("Flying",true);
                rb.bodyType=RigidbodyType2D.Kinematic;
                rb.velocity=new Vector2(0,10);
                PC2D.enabled=false;
                CC2D.enabled=true;
            }
            else{
                MoveDirection=XDirection;
            }
            transform.localScale=new Vector2(-8*MoveDirection,6);
            if(Math.Abs(rb.velocity.x)>0.3f){
                rb.velocity-=new Vector2(Math.Sign(rb.velocity.x)*FrictionAcceleration*Time.deltaTime,0);
            }
        }
        if(IsDeadShoot){
            hitPlayer=Physics2D.Raycast(origin:realposition,direction:Direction,distance:rayDistance,layerMask:playerLayer);
            if(hitPlayer.collider!=null&&(hitGround.point-realposition).sqrMagnitude>=(hitPlayer.point-realposition).sqrMagnitude){
                IsDeadShoot=false;
            }
        }
    }
    void Shoot1(){
        StartCoroutine(Shoot());
    }
    // void Shoot1(){
    //     LR.enabled=true;
    //     LR.startColor=new Color(1f,1f,0.8f,1f);
    //     LR.endColor=new Color(1f,1f,0.8f,1f);
    //     LR.startWidth=0.3f;
    //     LR.endWidth=0.3f;
    //     rb.velocity=Vector2.zero;
    //     hitGround = Physics2D.Raycast(origin:realposition,direction:Direction,distance:rayDistance,layerMask:groudLayer);
    //     LR.SetPosition(0,realposition);
    //     if(hitGround.collider!=null){
    //         LR.SetPosition(1,hitGround.point);
    //     }
    //     else{
    //         LR.SetPosition(1,realposition+Direction*rayDistance);
    //     }
    //     Invoke("Hide",1.2f);
    //     Invoke("Shoot2",1.5f);

    // }
    // void Shoot2(){
    //     IsDeadShoot=true;
    //     LR.enabled=true;
    //     LR.startColor=new Color(1f,1f,0f,1f);
    //     LR.endColor=new Color(1f,1f,0f,1f);
    //     LR.startWidth=1f;
    //     LR.endWidth=1f;
    //     Invoke("End",0.75f);
    //     PC2D.enabled=true;
    //     CC2D.enabled=false;
    // }
    // void Hide(){
    //     LR.enabled=false;
    // }
    // void End(){
    //     LR.enabled=false;
    //     rb.bodyType=RigidbodyType2D.Dynamic;
    //     anim.SetBool("Flying",false);
    //     IsDeadShoot=false;
    //     Invoke("Reset",3f);
    // }
    // void Reset(){
    //     Flying=false;
    //     anim.SetBool("Moving",true);
    // }
    public void LetUp(int UpDirection,float UpSpeed){
        switch(UpDirection){
            case 1:
                rb.velocity=new Vector2(0,UpSpeed);
                break;
            case 2:
                rb.velocity=new Vector2(0,-UpSpeed);
                break;
            case 3:
                rb.velocity=new Vector2(-UpSpeed,0);
                break;
            case 4:
                rb.velocity=new Vector2(UpSpeed,0);
                break;
        }
    }
    public void DieOut(){
        MC.AddScore(1.2f);
        Destroy(gameObject);
    }
    public void Move(){
        rb.velocity=new Vector2(MoveDirection*chasingspeed,rb.velocity.y);
    }
    bool CheckInCamera(){
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        return (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0);
    }
    public IEnumerator Shoot(){
        realposition=new Vector2(transform.position.x+1f,transform.position.y+1f);
        realplayerposition=new Vector2(player.position.x,player.position.y+3f);
        Direction=(realplayerposition-realposition).normalized;
        LR.enabled=true;
        rb.velocity=Vector2.zero;
        LR.startColor=new Color(1f,1f,0.8f,1f);
        LR.endColor=new Color(1f,1f,0.8f,1f);
        LR.startWidth=0.3f;
        LR.endWidth=0.3f;
        hitGround = Physics2D.Raycast(origin:transform.position,direction:Direction,distance:rayDistance,layerMask:groudLayer);
        LR.SetPosition(0,transform.position);
        if(hitGround.collider!=null){
            LR.SetPosition(1,hitGround.point);
        }
        else{
            LR.SetPosition(1,(Vector2)transform.position+Direction*rayDistance);
        }
        yield return new WaitForSeconds(1.2f);
        LR.enabled=false;
        yield return new WaitForSeconds(0.3f);
        IsDeadShoot=true;
        LR.enabled=true;
        LR.startColor=new Color(1f,1f,0f,1f);
        LR.endColor=new Color(1f,1f,0f,1f);
        LR.startWidth=1f;
        LR.endWidth=1f;
        yield return new WaitForSeconds(0.75f);
        LR.enabled=false;
        IsDeadShoot=false;
        rb.bodyType=RigidbodyType2D.Dynamic;
        anim.SetBool("Flying",false);
        yield return new WaitForSeconds(3f);
        Flying=false;
        anim.SetBool("Moving",true);
    }
}