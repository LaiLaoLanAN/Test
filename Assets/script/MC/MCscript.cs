using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Events;
//1111

public class MCscript : MonoBehaviour,IUpable
{
    public Rigidbody2D rb;
    public Animator anim;
    private SpriteRenderer spriteRenderer;
    private float Movecontroller;
    public float OringinMoveSpeed;
    public float Movespeed;
    [SerializeField]private float Accelerationspeed;
    [SerializeField]private float Jumpspeed;
    [SerializeField]private float dashspeed;
    [SerializeField]private float DashCD;
    [SerializeField]private float JumpAddTime;
    [SerializeField]private float BiteGroundSpeed;
    private float JumpTime;
    private bool IsJumping;
    public MCRightFoot RF;
    public MCLeftFoot LF;
    private bool Candash=true;
    public bool IsGround=true;
    public float LocalScaleLock=1;
    public bool IsDashing=false;

    private float dashdirection;
    private float Lastdashtime;
    public int Xdirection;
    private float Xspeed;

    public Vector2 MouseDirection;
    public float MouseDistance;
    public float MouseAngle;

    public enum Anim{wait,run,jump,fall,dash,open,openrun};
    public Anim state;

    public bool IsReading=false;
    [Header("咬合")]
    public bool Openmouth=false;
    public bool Closemouth=false;
    public float NeededClickTime;
    public float LastClickTime;
    public float OriginClickTime;
    public bool InMist=false;
    [Header("一些事件")]
    public UnityEvent<float> MoreScoreEvent;
    [Header("吃食的事")]
    public int eatthing=0;     // 0空1苹果2香蕉3西瓜4橘子5牌子
    public bool HaveKey=false;
    public float eatscore=0;
    [SerializeField]private float DecreasingSpeed;
    public float AddScoreAmount;
    private Coroutine decreasecoroutine;
    private bool IsDecreasing=true;
    public float MaxScore;
    public float MoreScore;
    public float RebirthNeededScore;
    
    [Header("活")]
    public bool CanRebirth=false;
    public bool IsDead=false;
    public float RebirthInvincibleTime;
    public GameObject Hexagram;
    [Header("其他玩意的调用")]
    public Vector3 PlatformLastPos;
    public MCCollider MCcollider;
    public GameObject MCshade;
    private InputManager inputManager;

    private int JumpNum = 0;
    public int JumpMaxNum;

    public float ScaleRate=1;
    public bool IsInPlatform=false;
    // Start is called before the first frame update
    void Start()
    {
        Hexagram.SetActive(false);
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        spriteRenderer=GetComponent<SpriteRenderer>();
        Lastdashtime=Time.time;
        LastClickTime=Time.deltaTime;
        inputManager = InputManager.Instance;
        inputManager.GetBeatKeyEvent("Jump").AddListener(Jump);
        inputManager.GetBeatKeyEvent("Dash").AddListener(Dash);
        bool SB = Candash;//解除SB报错
    }

    void OnDestroy()
    {
        inputManager.GetBeatKeyEvent("Jump").RemoveListener(Jump);
        inputManager.GetBeatKeyEvent("Dash").RemoveListener(Dash);
    }
    // Update is called once per frame
    void Update()
    {
        if(IsDead||IsReading){
            return;
        }
        // Movespeed=OringinMoveSpeed*(1+eatscore/MaxScore/2);
        Movespeed =OringinMoveSpeed;
        Movecontroller=Input.GetAxisRaw("Horizontal");
        Vector2 mouse=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 MCpos=new Vector2(transform.position.x,transform.position.y+3f);
        MouseDistance=(mouse-MCpos).magnitude;
        MouseDirection=(mouse-MCpos).normalized;
        MouseAngle=Mathf.Atan2(MouseDirection.x,MouseDirection.y)*Mathf.Rad2Deg;
        Xspeed=rb.velocity.x; 
        Xdirection=Math.Sign(Xspeed);
        IsGround=RF.RIsGround||LF.LIsGround;


        if(Math.Abs(Xspeed)<=Movespeed+0.3f){
            IsDashing=false;
        }


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
            
                if( (Math.Abs(Xspeed)>Movespeed)&&Movecontroller==Xdirection ){
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
                    rb.velocity-=new Vector2(0.4f*Accelerationspeed*Time.deltaTime*Xdirection,0);
                }
            }
            if(IsDashing){
                rb.velocity-=new Vector2(5*Accelerationspeed*Xdirection*Time.deltaTime,0);
            }
            else{
                if(Movecontroller!=0 && Movecontroller+Xdirection==0 || Math.Abs(Xspeed)<=Movespeed){
                    rb.velocity+=new Vector2(5*Accelerationspeed*Movecontroller*Time.deltaTime,0);
                }
                if( (Math.Abs(Xspeed)>Movespeed)&&Movecontroller==Xdirection ){
                    rb.velocity = new Vector2(Movecontroller*Movespeed,rb.velocity.y);
                }
            }
            
        }

        Xspeed=rb.velocity.x; 
        Xdirection=Math.Sign(Xspeed);  


        if(inputManager.GetBeatKeyUp("Jump"))
        {
            IsJumping=false;
        }
        if(IsGround){
            JumpNum = 0;
        }
        else if(IsJumping && Time.time-JumpTime<JumpAddTime){
            rb.velocity+=new Vector2(0,18f*Time.deltaTime);
        }
        else if(!IsGround){
            rb.velocity-=new Vector2(0,33f*Time.deltaTime);
            IsJumping=false;
        }



        if (Input.GetKeyDown(KeyCode.J))
        {
            //KeyCode keyCode = inputManager.LastKeyCode;   //还没实现，以后做冲刺攻击和跳跃攻击用
            GenerateShade();
            Openmouth = true;
            Closemouth = true;
            LastClickTime = Time.time;
        }                                                                                 //咬合

        if (Input.GetKeyUp(KeyCode.J))
        {
            Openmouth = false;
        }

        if(!IsInPlatform&&(LF.LPlatForm!=null || RF.RPlatForm!=null))
        {
            IsInPlatform = true;
            if (LF.LPlatForm != null)
            {
                transform.parent = LF.LPlatForm.transform;
                ScaleRate = 1 / LF.LPlatForm.transform.localScale.x;

            }
            else
            {
                transform.parent =RF.RPlatForm.transform;
                ScaleRate = 1 / RF.RPlatForm.transform.localScale.x;
            }
        }
        else if(IsInPlatform && (LF.LPlatForm == null && RF.RPlatForm == null))
        {
            IsInPlatform=false;
            transform.parent = null;
            ScaleRate = 1;
        }

        if (IsGround){
            Candash=true;
        }
        
        Xspeed=rb.velocity.x; 
        Xdirection=Math.Sign(Xspeed);
        if(IsDashing){
            float dashcal=(Math.Abs(Xspeed)-Movespeed)/(dashspeed-Movespeed);
            if(10-2*dashcal>2f){
            transform.localScale= ScaleRate*new Vector2(dashdirection*(10+3*dashcal),10-2*dashcal);
            }
            else{
                transform.localScale= ScaleRate*new Vector2(dashdirection*(10+3*dashcal),2);
            }
        }
        else {
            if (Math.Abs(Xspeed)>=Movespeed-0.3f){                 //行动时的角色缩放变换,在想改IsDashing时，还要改1/-1的dashdirection
            LocalScaleLock=Xdirection;
            transform.localScale= ScaleRate*new Vector2(10*LocalScaleLock,10);
            }
            else if(Math.Abs(Xspeed)<=0.3f){
                transform.localScale= ScaleRate*new Vector2(10*LocalScaleLock,10);
            }
            else if(Xdirection!=0 && LocalScaleLock+Xdirection==0){
                transform.localScale = ScaleRate*new Vector2(-10*Xdirection+20*(Xspeed/Movespeed),10);
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
            if(IsGround&&Movecontroller!=0){
                state=Anim.openrun;
            }
            else{
                state=Anim.open;
            }
            
        }

        anim.SetInteger("State",(int)state);
        if(IsDecreasing){
            eatscore-=Time.deltaTime*DecreasingSpeed;             //得分衰减
        }
        if(eatscore<0){
            eatscore=0;
        }
        if(eatscore>MaxScore){
            if(MoreScore<RebirthNeededScore){
                float _score=(eatscore-MaxScore)-(eatscore-MaxScore)%3;
                if(MoreScore+_score>=RebirthNeededScore){
                    _score=RebirthNeededScore-MoreScore;
                    CanRebirth=true;
                }
                MoreScore+=_score;
                MoreScoreEvent.Invoke(_score);
            }
            eatscore=MaxScore;
        }
    }
    private void Jump()
    {
        GenerateShade();
        if (JumpNum > JumpMaxNum||!IsGround)
        {
            return;
        }
        //rb.velocity = new Vector2(Xspeed, Jumpspeed * (((JumpMaxNum - JumpNum) * (JumpMaxNum - JumpNum)) / (JumpMaxNum * JumpMaxNum)));
        rb.velocity = new Vector2(Xspeed, Jumpspeed);
        JumpNum++;
        JumpTime = Time.time;
        IsJumping = true;
    }
    private void Dash()
    {
        GenerateShade();
        anim.SetInteger("State", 4);
        dashdirection = Math.Sign(transform.localScale.x);
        if (Movecontroller != 0)
        {
            dashdirection = Movecontroller;
            LocalScaleLock = Movecontroller;                                        //冲刺
        }
        rb.velocity = new Vector2(dashspeed * dashdirection, 0);
        IsDashing = true;
        Lastdashtime = Time.time;
    }
    public void BiteGround(float rate){
        if (!(MouseAngle<-110||MouseAngle>110))
        {
            return;
        }
        rb.velocity=new Vector2(-rate*BiteGroundSpeed*MouseDirection.x+rb.velocity.x,-rate*BiteGroundSpeed*MouseDirection.y);
        if(Math.Abs(rb.velocity.x)>Movespeed+0.3f){
            IsDashing=true;
            dashdirection=Math.Sign(rb.velocity.x);
        }
        Candash=true;
    }
    public void AddSpeedDirected(float AddAmount){
        Vector2 AddSpeed=AddAmount*MouseDirection;
        rb.velocity+=AddSpeed;
        bool XNeed=false;
        bool YNeed=false;
        if(AddSpeed.x>0?rb.velocity.x<AddSpeed.x:rb.velocity.x>AddSpeed.x){
            XNeed=true;
        }
        if(AddSpeed.y>0?rb.velocity.y<AddSpeed.y:rb.velocity.y>AddSpeed.y){
            YNeed=true;
        }
        rb.velocity=new Vector2(XNeed?AddSpeed.x:rb.velocity.x,YNeed?AddSpeed.y:rb.velocity.y);
        if(Math.Abs(rb.velocity.x)>Movespeed+0.3f){
            IsDashing=true;
            dashdirection=Math.Sign(rb.velocity.x);
        }
    }
    public void MCDeath(){
        IsDead=true;
        anim.enabled=false;
        transform.localScale=new Vector2(10*LocalScaleLock,10);
        StartCoroutine(DieBlack());
        rb.bodyType = RigidbodyType2D.Static;
        if(PlayerPrefs.GetInt("DeathNum",0)<20){
            PlayerPrefs.SetInt("DeathNum",PlayerPrefs.GetInt("DeathNum",0)+1); 
        }
        PlayerPrefs.Save();
        this.enabled=false;
    }
    public void MCRebirthPre(){
        eatscore=0;
        MoreScore=0;
        CanRebirth=false;
        IsDead=true;
        anim.enabled=false;
        transform.localScale=new Vector2(10*LocalScaleLock,10);
        rb.bodyType = RigidbodyType2D.Static;
    }
    public void MCRebirth(){
        IsDead=false;
        anim.enabled=true;
        Invoke("ColliderBack",RebirthInvincibleTime);
        Hexagram.SetActive(true);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    void ColliderBack(){
        Hexagram.SetActive(false);
    }
    IEnumerator DieBlack()
    {
        Color originalColor = spriteRenderer.color;
        float elapsedTime = 0f;
        while (elapsedTime < 2.5f){
            float progress=elapsedTime/2.5f;
            spriteRenderer.color = Color.Lerp(originalColor,Color.black,progress);
            elapsedTime+=Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = Color.black;
    }

    public void LetUp(int UpDirection,float UpSpeed){
        Candash=true;
        switch(UpDirection){
            case 1:
                rb.velocity=new Vector2(0,UpSpeed);
                break;
            case 2:
                rb.velocity=new Vector2(0,-UpSpeed);                     //被弹射
                break;
            case 3:
                IsDashing=true;
                dashdirection=-1;
                rb.velocity=new Vector2(-UpSpeed,0);
                break;
            case 4:
                IsDashing=true;
                dashdirection=1;
                rb.velocity=new Vector2(UpSpeed,0);
                break;
        }
    }
    public void AddScore(float addscoreRate){
        eatscore+=addscoreRate*AddScoreAmount;
        decreasecoroutine=StartCoroutine(DecreaseCoroutine());
    }
    public void GenerateShade()
    {
        GameObject shade = Instantiate(MCshade, transform.position, transform.rotation);
        shade.transform.localScale = transform.localScale;
        shade.GetComponent<MCShade>().Shadef(spriteRenderer.sprite);
    }
    IEnumerator DecreaseCoroutine(){
        IsDecreasing=false;
        yield return new WaitForSeconds(2.5f);
        IsDecreasing=true;
    }
}
