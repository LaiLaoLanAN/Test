using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWaterMelonPre : MonoBehaviour
{
    public bool IsFacingRight;
    public GameObject ShootWaterMelonPP;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField]private float RestartSpeed;
    [SerializeField]private float OriginMaxSpeed;
    [SerializeField]private float OriginAcceleration;
    [SerializeField]private float ChangingRate;
    private float MaxSpeed;
    private float Acceleration;
    [SerializeField]private float SplitZoomTime;
    [SerializeField]private float OriginSize;
    [SerializeField]private float SplitZoomSize;
    private float NowSpeed;
    private bool CanSplit=true;
    private bool IsBomb=false;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        rb.velocity=new Vector2((IsFacingRight?1:-1)*RestartSpeed,0);
        transform.localScale=new Vector2(OriginSize,OriginSize);
        NowSpeed=RestartSpeed;
        MaxSpeed=OriginMaxSpeed;
        Acceleration=OriginAcceleration;
    }
    void OnTriggerEnter2D(Collider2D other){
        var obj=other.gameObject;
        IsBomb=true;
        anim.SetTrigger("Bomb");
        rb.velocity=Vector2.zero;
        IDamagableE iDamagableE=other.GetComponent<IDamagableE>();
        if(iDamagableE!=null){
            iDamagableE.DieOut();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsBomb){
            if(NowSpeed<MaxSpeed){
                NowSpeed+=Time.deltaTime*Acceleration;
            }
            if(Input.GetMouseButtonDown(1)&&CanSplit)
            {
                CanSplit=false;
                MaxSpeed*=ChangingRate;
                Acceleration*=ChangingRate;
                StartCoroutine(Split());
            }
            rb.velocity=new Vector2((IsFacingRight?1:-1)*NowSpeed,0);
        }
    }
    IEnumerator Split(){
        float TimeBig=SplitZoomTime/3;
        float TimeSmall=TimeBig*2;
        float PassedTime=0;
        float NowSize=OriginSize;
        while(PassedTime<TimeBig){
            if(IsBomb){
                yield break;
            }
            NowSize=OriginSize+(PassedTime/TimeBig)*(SplitZoomSize-OriginSize);
            PassedTime+=Time.deltaTime;
            transform.localScale=new Vector2(NowSize,NowSize);
            yield return null;
        }
        GameObject ShootingWaterMelonPPUp=Instantiate(ShootWaterMelonPP,transform.position+new Vector3((IsFacingRight?1:-1)*7,-1.6f,0),transform.rotation);
        ShootingWaterMelonPPUp.GetComponent<ShootWMPP>().IsUp=true;
        GameObject ShootingWaterMelonPPDown=Instantiate(ShootWaterMelonPP,transform.position+new Vector3((IsFacingRight?1:-1)*7,1.6f,0),transform.rotation);
        ShootingWaterMelonPPDown.GetComponent<ShootWMPP>().IsUp=false;
        NowSize=SplitZoomSize;
        PassedTime=0;
        while(PassedTime<TimeSmall){
            if(IsBomb){
                yield break;
            }
            NowSize=SplitZoomSize-(PassedTime/TimeSmall)*(SplitZoomSize-OriginSize);
            PassedTime+=Time.deltaTime;
            transform.localScale=new Vector2(NowSize,NowSize);
            yield return null;
        }
        transform.localScale=new Vector2(OriginSize,OriginSize);
        CanSplit=true;
    }
    void Disappear(){
        Destroy(gameObject);
    }
}
