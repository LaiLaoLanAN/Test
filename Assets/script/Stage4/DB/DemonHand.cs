using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHand : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer SR;
    private Rigidbody2D rb;
    public DemonBroad DB;
    public enum Anim{wait,grab,put,teleport,shoot,crash,fuck};
                   //0000,1111,222,33333333,44444,55555,6666
    public Anim state;
    public bool IsLeft;
    public int FacingDirection;//然而localscale的朝向与其相反
    public Transform Player;
    public MCscript MC;
    public DemonHand AnotherHand;
    [Header("传送")]
    public GameObject ProtalPre;
    public AnimationCurve TeleportCurve;
    public Coroutine TeleportCoroutine;
    [Header("等待")]
    public Vector3 WaitPosition;
    [Header("抓")]
    public HandCollider grabcollider;
    public AnimationCurve GrabMoveCurve;
    public float GrabSpeed;
    public float GrabAccelerationTime;
    public float GrabReboundDistance;
    public float GrabReboundTime;
    public AnimationCurve GrabReboundCurve;
    [Header("放怪")]
    public GameObject[] PutEnemy;
    [Header("射线")]
    public ShootPoint shootpoint;
    [Header("砸")]
    public float CrashHeight;
    public HandCollider crashcollider;
    public AnimationCurve CrashMoveCurve;
    public float CrashSpeed;
    public float CrashAccelerationTime;
    public float CrashReboundDistance;
    public float CrashReboundTime;
    public AnimationCurve CrashReboundCurve;
    [Header("鄙")]
    public AnimationCurve FuckCurve;
    public float FuckTime;
    public float FuckDistance;
    [Header("砸地")]
    public BrokenTerrin[] brokenterrin;
    // Start is called before the first frame update
    void Start()
    {
        FacingDirection=IsLeft?-1:1;
        WaitPosition=transform.position-transform.parent.position;
        anim=GetComponent<Animator>();
        SR=GetComponent<SpriteRenderer>();
        SR.enabled=false;
        rb=GetComponent<Rigidbody2D>();
        TeleportCoroutine=StartCoroutine(Teleport(WaitPosition,true,false));
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("WaitNum",DB.WaitAnimNum);
    }
    public void Attact(AttactPatten attactpatten,float DelayTime,Vector2 LocalPosition){
        switch(attactpatten){
            case AttactPatten.wait:
                Ready();
                break;
            case AttactPatten.grab:
                StartCoroutine(Grab(DelayTime,LocalPosition));
                break;
            case AttactPatten.put:
                StartCoroutine(Put(DelayTime,LocalPosition));
                break;
            case AttactPatten.shoot:
                StartCoroutine(Shoot(DelayTime,LocalPosition));
                break;
            case AttactPatten.crash:
                StartCoroutine(Crash(DelayTime,LocalPosition));
                break;
        }
    }
    IEnumerator Grab(float DelayTime,Vector2 LocalPosition){
        yield return new WaitForSeconds(DelayTime);
        anim.SetInteger("state",1);
        yield return TeleportCoroutine=StartCoroutine(Teleport(LocalPosition,false,true));
        grabcollider.ColliderOn();
        
        float AddTime=0;
        while(AddTime<GrabAccelerationTime&&!grabcollider.IsCollidered){
            rb.velocity=new Vector2(FacingDirection*GrabSpeed*GrabMoveCurve.Evaluate(AddTime/GrabAccelerationTime),0);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(()=> grabcollider.IsCollidered);
        rb.velocity=Vector2.zero;
        Vector3 NowPosition=transform.position;
        Vector3 TargetPosition=NowPosition+new Vector3(-FacingDirection*GrabReboundDistance,0,0);
        AddTime=0;
        while(AddTime<GrabReboundTime){
            transform.position=NowPosition+GrabReboundCurve.Evaluate(AddTime/GrabReboundTime)*(TargetPosition-NowPosition);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        while(AddTime>0){
            transform.position=NowPosition+GrabReboundCurve.Evaluate(AddTime/GrabReboundTime)*(TargetPosition-NowPosition);
            AddTime-=Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);

        anim.SetInteger("state",0);
        TeleportCoroutine=StartCoroutine(Teleport(WaitPosition,false,false));
        grabcollider.ColliderOff();
    }
    IEnumerator Put(float DelayTime,Vector2 LocalPosition){
        yield return new WaitForSeconds(DelayTime);
        anim.SetInteger("state",2);
        yield return TeleportCoroutine=StartCoroutine(Teleport(LocalPosition,false,true));
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("put");
        GameObject PutThing=Instantiate(PutEnemy[Random.Range(0, PutEnemy.Length)],transform.position+new Vector3(0,-10,0),transform.rotation);
        PutThing.transform.parent=brokenterrin[DB.Floor].transform;
        yield return new WaitForSeconds(0.5f);
        anim.SetInteger("state",0);
        TeleportCoroutine=StartCoroutine(Teleport(WaitPosition,false,false));
        
    }
    IEnumerator Shoot(float DelayTime,Vector2 LocalPosition){
        yield return new WaitForSeconds(DelayTime);
        anim.SetInteger("state",4);
        yield return TeleportCoroutine=StartCoroutine(Teleport(LocalPosition,false,true));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(shootpoint.Shoot());
        anim.SetInteger("state",0);
        TeleportCoroutine=StartCoroutine(Teleport(WaitPosition,false,false));
    }
    IEnumerator Crash(float DelayTime,Vector2 LocalPosition){
        yield return new WaitForSeconds(DelayTime);
        anim.SetInteger("state",5);                    //LocalPosition的y设为-100则开启追踪
        if(LocalPosition.y==-100){
            LocalPosition=(Vector2)Player.position-(Vector2)transform.parent.position+new Vector2(0,CrashHeight);
        }
        yield return TeleportCoroutine=StartCoroutine(Teleport(LocalPosition,false,true));
        crashcollider.ColliderOn();
        
        float AddTime=0;
        while(AddTime<CrashAccelerationTime&&!crashcollider.IsCollidered){
            rb.velocity=new Vector2(0,-CrashSpeed*CrashMoveCurve.Evaluate(AddTime/CrashAccelerationTime));
            AddTime+=Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(()=> crashcollider.IsCollidered);
        rb.velocity=Vector2.zero;
        Vector3 NowPosition=transform.position;
        Vector3 TargetPosition=NowPosition+new Vector3(0,CrashReboundDistance,0);
        AddTime=0;
        while(AddTime<CrashReboundTime){
            transform.position=NowPosition+CrashReboundCurve.Evaluate(AddTime/CrashReboundTime)*(TargetPosition-NowPosition);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        while(AddTime>0){
            transform.position=NowPosition+CrashReboundCurve.Evaluate(AddTime/CrashReboundTime)*(TargetPosition-NowPosition);
            AddTime-=Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);

        anim.SetInteger("state",0);
        TeleportCoroutine=StartCoroutine(Teleport(WaitPosition,false,false));
        crashcollider.ColliderOff();
    }
    public IEnumerator Teleport(Vector2 TargetLocalPosition,bool IsStart=false,bool IsToPoint=false){
        anim.ResetTrigger("TeleportEnd");
        Vector3 NowPosition;
        Vector3 TargetPosition;
        float TeleportTime=1f;
        float AddTime=0;
        if(!IsStart){
            NowPosition=transform.position;
            TargetPosition=NowPosition+new Vector3(15*FacingDirection,0,0);
            GameObject Protal=Instantiate(ProtalPre,NowPosition+new Vector3(12*FacingDirection,-15,0),transform.rotation);
            Protal.transform.localScale=new(-35*FacingDirection,35,1);
            while(AddTime<TeleportTime){
                transform.position=NowPosition+TeleportCurve.Evaluate(AddTime/TeleportTime)*(TargetPosition-NowPosition);
                AddTime+=Time.deltaTime;
                yield return null;
            }
            SR.enabled=false;
            if(!IsToPoint){
                yield return new WaitForSeconds(1f);
            }
            FacingDirection=-FacingDirection;
            transform.localScale=new Vector2(-FacingDirection*0.1f,0.1f);
        }
        transform.position=transform.parent.position+(Vector3)TargetLocalPosition+new Vector3(-10*FacingDirection,0,0);
        NowPosition=transform.position;
        TargetPosition=transform.parent.position+(Vector3)TargetLocalPosition;
        GameObject Protal2=Instantiate(ProtalPre,NowPosition+new Vector3(-3.5f*FacingDirection,-15,0),transform.rotation);
        Protal2.transform.localScale=new(-35*FacingDirection,35,1);
        yield return new WaitForSeconds(0.5f);
        SR.enabled=true;
        AddTime=0;
        while(AddTime<TeleportTime){
            transform.position=NowPosition+TeleportCurve.Evaluate(AddTime/TeleportTime)*(TargetPosition-NowPosition);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        anim.SetTrigger("TeleportEnd");
        if(!IsToPoint){
            Ready();
        }
    }
    void Ready(){
        if(IsLeft){
            DB.LeftReady=true;
        }
        else{
            DB.RightReady=true;
        }
    }
    public void Fuckf(){
        StopAllCoroutines();
        rb.velocity=Vector2.zero;
        shootpoint.Fuck();
        StartCoroutine(Fuck());
    }
    IEnumerator Fuck(){
        anim.SetTrigger("Fuck");
        anim.SetInteger("state",0);
        float AddTime=0;
        Vector2 OriginPosition=transform.position;
        Vector2 TargetMove=MC.MouseDirection*FuckDistance;
        while(AddTime<FuckTime){
            transform.position=OriginPosition+TargetMove*FuckCurve.Evaluate(AddTime/FuckTime);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        transform.position=OriginPosition+TargetMove;
        yield return new WaitForSeconds(0.75f);
        anim.SetTrigger("FuckEnd");
        TeleportCoroutine=StartCoroutine(Teleport(WaitPosition,false,false));
    }
    public IEnumerator BreakGround(){
        StopAllCoroutines();
        SR.enabled=true;
        rb.velocity=Vector2.zero;
        shootpoint.Fuck();
        grabcollider.ColliderOff();
        crashcollider.ColliderOff();
        anim.SetTrigger("Fuck");
        anim.SetInteger("state",5);
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("FuckEnd");
        Vector3 NowPosition=transform.position;
        Vector3 TargetPosition=new Vector3(NowPosition.x,transform.parent.position.y+WaitPosition.y+2f*CrashReboundDistance,0);
        float AddTime=0;
        float _CrashReboundTime=1.7f*CrashReboundTime;
        while(AddTime<_CrashReboundTime){
            transform.position=NowPosition+CrashReboundCurve.Evaluate(AddTime/_CrashReboundTime)*(TargetPosition-NowPosition);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.15f);
        crashcollider.ColliderOn();
        AddTime=0;
        while(AddTime<CrashAccelerationTime&&!crashcollider.IsCollidered&&!AnotherHand.crashcollider.IsCollidered){
            rb.velocity=new Vector2(0,-CrashSpeed*CrashMoveCurve.Evaluate(AddTime/CrashAccelerationTime));
            AddTime+=Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(()=> crashcollider.IsCollidered||AnotherHand.crashcollider.IsCollidered);
        Invoke("BrokenTerrin",0.1f);
        rb.velocity=Vector2.zero;
        NowPosition=transform.position;
        TargetPosition=NowPosition+new Vector3(0,CrashReboundDistance,0);
        AddTime=0;
        while(AddTime<CrashReboundTime){
            transform.position=NowPosition+CrashReboundCurve.Evaluate(AddTime/CrashReboundTime)*(TargetPosition-NowPosition);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        while(AddTime>0){
            transform.position=NowPosition+CrashReboundCurve.Evaluate(AddTime/CrashReboundTime)*(TargetPosition-NowPosition);
            AddTime-=Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        crashcollider.ColliderOff();
        anim.SetInteger("state",0);
        transform.position=new Vector2(0,1000);
        SR.enabled=false;
    }
    void BrokenTerrin(){
        if(brokenterrin[DB.Floor-1]!=null){
            brokenterrin[DB.Floor-1].BreakGround();
        }
    }
    public void NextFloor(){
        StopAllCoroutines();
        FacingDirection=IsLeft?-1:1;
        transform.localScale=new Vector2(-FacingDirection*0.1f,0.1f);
        StartCoroutine(Teleport(WaitPosition,true,false));
    }
    public IEnumerator TrueDeath(){
        SR.enabled=true;
        float AddTime=0;
        while(AddTime<1f){
            SR.color=new Color(1,1,1,1-AddTime);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}