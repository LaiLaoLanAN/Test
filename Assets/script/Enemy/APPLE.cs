using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class APPLE : MonoBehaviour,IUpable,IDamagableE
{
    private Rigidbody2D rb;
    private Animator anim;
    private MCscript MC;
    private Transform player;
    private int Direction;
    private float Xdistance;
    private float Ydistance;
    private bool waiting=true;
    private bool Ischasing=false;
    private bool Startchasing1=false;
    private bool Startchasing2=false;
    private float Jumptime;
    private float Falltime;
    public bool IsGround;
    [SerializeField]private float isGroundCheckapple;

    private float LastJumpTime;
    [SerializeField]private float chasingdistance;
    [SerializeField]private float chasingspeed;

    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
        MC=FindObjectOfType<MCscript>();
        player=MC.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float timeRate = TimeManager.Instance.TimeRate;
        if (timeRate < 1)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        else if(timeRate>1)
        {
            transform.position += (Vector3)(rb.velocity * (timeRate - 1) * Time.deltaTime);
        }
        IsGround=Physics2D.Raycast(transform.position,Vector2.down,isGroundCheckapple,LayerMask.GetMask("Ground"));

        
        Direction=Math.Sign(player.position.x-transform.position.x);
        Xdistance=Math.Abs(player.position.x-transform.position.x);
        Ydistance=player.position.y-transform.position.y;
        if(waiting&&CheckInCamera()){
            Startchasing1=true;
            anim.SetTrigger("chase");
            Jumptime=Time.time;
            waiting=false;
        }
        if(Startchasing1){
            rb.velocity=new Vector2(0,5);
            if(Time.time-Jumptime>1.2f){
                Startchasing1=false;
                Startchasing2=true;
                Falltime=Time.time;
            }
        }
        if(Startchasing2){
            rb.velocity-=new Vector2(0,25f*Time.deltaTime);
            if(Time.time-Falltime>0.7f){
                Startchasing2=false;
                Ischasing=true;
                anim.SetTrigger("touchground");
            }
        }
        if(Ischasing){
            if(Xdistance>6f&&Xdistance<30f&&Ydistance>7f&&IsGround&&Direction==Math.Sign(rb.velocity.x))
            {
                if(Math.Abs(rb.velocity.x)>0.4f*chasingspeed){
                    rb.velocity-=new Vector2(30f*Direction*Time.deltaTime,0);
                }
            }
            else{
                LastJumpTime=Time.time;

            }
            if(Time.time-LastJumpTime>0.2f){
                rb.velocity=new Vector2(rb.velocity.x,12);
            }
            if(Math.Abs(rb.velocity.x)<chasingspeed||Math.Sign(rb.velocity.x)+Direction==0){
                rb.velocity+=new Vector2(10f*Direction*Time.deltaTime,0);
            }
            if(Direction!=0){
                transform.localScale=new Vector2(10*Direction,10);
            }
        }
    }
    private void OnDrawGizmos(){
        Gizmos.DrawLine(transform.position,new Vector2(transform.position.x,transform.position.y-isGroundCheckapple));
    }



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
        MC.AddScore(1f);
        Destroy(gameObject);
    }
    bool CheckInCamera(){
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        return (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0);
    }

}
