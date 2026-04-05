using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MCEnterRoom : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    private float Movecontroller;
    [SerializeField]private float Movespeed;
    [SerializeField]private float Accelerationspeed;
    [SerializeField]private float Jumpspeed;
    [SerializeField]private float dashspeed;
    [SerializeField]private float DashCD;
    [SerializeField]private float JumpAddTime;
    private float JumpTime;
    private bool IsJumping;
    public MCRightFoot RF;
    public MCLeftFoot LF;
    private bool Candash=true;
    public bool IsGround=true;
    public float LocalScaleLock=1;
    private bool IsDashing=false;

    public bool Openmouth=false;
    public bool Closemouth=false;


    private float dashdirection;
    private float Lastdashtime;
    private float Lastmousetime;
    public int Xdirection;

    public Vector2 MouseDirection;
    public float MouseDistance;
    public float MouseAngle;
    
    public bool IsReading;

    public enum Anim{wait,run,jump,fall,dash,open};
    public Anim state;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        Lastdashtime=Time.time;
        
    }


    // Update is called once per frame
    void Update()
    {
        Movecontroller=Input.GetAxisRaw("Horizontal");
        Vector2 mouse=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 MCpos=new Vector2(transform.position.x,transform.position.y+3f);
        MouseDistance=(mouse-MCpos).magnitude;
        MouseDirection=(mouse-MCpos).normalized;
        MouseAngle=Mathf.Atan2(MouseDirection.x,MouseDirection.y)*Mathf.Rad2Deg;
        float Xspeed=rb.velocity.x; 
        Xdirection=Math.Sign(Xspeed);
        IsGround=RF.RIsGround||LF.LIsGround;


        if(Math.Abs(Xspeed)<=Movespeed+0.3f){
            IsDashing=false;
        }
        if(!IsReading){
            if (IsGround==true){
                if(Movecontroller==0){
                    if(Math.Abs(rb.velocity.x)<0.3f){
                        rb.velocity=new Vector2(0,rb.velocity.y);
                    }
                    else{
                        rb.velocity-=new Vector2(2f*Accelerationspeed*Time.deltaTime*Xdirection,0);
                    }
                }
                if(IsDashing){
                    rb.velocity-=new Vector2(15*Accelerationspeed*Xdirection*Time.deltaTime,0);
                }
                else{
                    if(Movecontroller!=0 && Movecontroller+Xdirection==0 || Math.Abs(Xspeed)<=Movespeed){
                        rb.velocity+=new Vector2(10*Accelerationspeed*Movecontroller*Time.deltaTime,0);
                    }
                
                    if( (Xdirection==1 && Xspeed>Movespeed) || (Xdirection==-1 && Xspeed<-Movespeed) ){
                        rb.velocity = new Vector2(Movecontroller*Movespeed,rb.velocity.y);
                    }
                }
            }
            else {
                if(Movecontroller==0){                                                                        //移动
                    if(Math.Abs(rb.velocity.x)<0.3f){
                        rb.velocity=new Vector2(0,rb.velocity.y);
                    }
                    else{
                        rb.velocity-=new Vector2(0.6f*Accelerationspeed*Time.deltaTime*Xdirection,0);
                    }
                }
                if(IsDashing){
                    rb.velocity-=new Vector2(7*Accelerationspeed*Xdirection*Time.deltaTime,0);
                }
                else{
                    if(Movecontroller!=0 && Movecontroller+Xdirection==0 || Math.Abs(Xspeed)<=Movespeed){
                        rb.velocity+=new Vector2(6*Accelerationspeed*Movecontroller*Time.deltaTime,0);
                    }
                    if( (Xdirection==1 && Xspeed>Movespeed) || (Xdirection==-1 && Xspeed<-Movespeed) ){
                        rb.velocity = new Vector2(Movecontroller*Movespeed,rb.velocity.y);
                    }
                }
                
            }

            Xspeed=rb.velocity.x; 
            Xdirection=Math.Sign(Xspeed);  

            if (Input.GetKeyDown(KeyCode.Space)&& IsGround){
                rb.velocity=new Vector2(Xspeed,Jumpspeed);
                JumpTime=Time.time;
                IsJumping=true;                                                        //跳跃
            }

            if(Input.GetButtonUp("Jump")){
                IsJumping=false;
            }

            if(IsJumping && Time.time-JumpTime<JumpAddTime){
                rb.velocity+=new Vector2(0,20f*Time.deltaTime);
            }
            else{
                rb.velocity-=new Vector2(0,20f*Time.deltaTime);
            }

            if(IsGround){
                Candash=true;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)&& Candash && Time.time-Lastdashtime>DashCD){
                anim.SetInteger("State",4);
                dashdirection = Math.Sign(transform.localScale.x);
                if(Movecontroller!=0){
                    dashdirection=Movecontroller;                                        //冲刺
                    LocalScaleLock=Movecontroller;
                }
                rb.velocity=new Vector2(dashspeed*dashdirection,0);
                IsDashing=true;
                Candash=false;
                Lastdashtime=Time.time;
            }     
            Xspeed=rb.velocity.x; 
            Xdirection=Math.Sign(Xspeed);  

            if(IsDashing){
                float dashcal=(Math.Abs(Xspeed)-Movespeed)/(dashspeed-Movespeed);
                transform.localScale=new Vector2(dashdirection*(10+3*dashcal),10-2*dashcal);
            }
            else{
                if (Math.Abs(Xspeed)>=Movespeed-0.3f){                 //行动时的角色缩放变换
                LocalScaleLock=Xdirection;
                transform.localScale=new Vector2(10*LocalScaleLock,10);
                }
                else if(Math.Abs(Xspeed)<=0.3f){
                    transform.localScale=new Vector2(10*LocalScaleLock,10);
                }
                else if(Xdirection!=0 && LocalScaleLock+Xdirection==0){
                    transform.localScale = new Vector2(-10*Xdirection+20*(Xspeed/Movespeed),10);
                }

            }

            state=Anim.wait;
            if(Movecontroller!=0){
                state=Anim.run;
            }
            if(!IsGround){
                if(rb.velocity.y>0.3f){
                    state=Anim.jump;
                }
                else{                                                               //动画
                    state=Anim.fall;
                }
            }
            if(IsDashing){
                state=Anim.dash;
            }
            if(Openmouth){
                state=Anim.open;
            }
            if(IsReading){
                state=Anim.wait;
            }

            anim.SetInteger("State",(int)state);
        }
    }
}
