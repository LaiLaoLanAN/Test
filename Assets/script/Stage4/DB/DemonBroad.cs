using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttactPatten{wait,grab,put,shoot,crash};
                        //000,1111,222,33333,44444
public class DemonBroad : MonoBehaviour
{
    public Stage4Manager manager;
    public Tester tester;
    public DemonHand LeftHand;
    public DemonHand RightHand;
    public MCscript MC;
    [Header("层")]
    public int Floor=0;
    public Vector2[] FloorPosition;
    public float NextFloorMoveTime;
    public AnimationCurve NextFloorMoveCurve;
    [Header("攻击")]
    public AttactPattenData APData;
    public bool LeftReady=false;
    public bool RightReady=false;
    [Header("等待动画")]
    public int WaitAnimNum=1;
    public float WaitAnimCD;
    private float WaitAddTime=0;
    [Header("攻击")]
    public float[] AttactCD;
    private float LastAttactTime=-1;
    [Header("受击")]
    public Collider2D itcollider;
    public Wave wave;
    public Transform Player;
    public int[] NeedHit;
    public int NowHit=0;
    public float[] HitGapTime;
    [Header("砸地")]
    public bool IsBreakingGround;
    public Coroutine BGcoroutine=null;
    [Header("真死")]
    public float shakeDuration;
    public float minFrequency;
    public float maxFrequency;
    public float shakeAmount;
    public AnimationCurve TrueDeathShakeCurve;
    public GameObject DeadDemonBroad;
    // Start is called before the first frame update
    void Start()
    {
        transform.position=FloorPosition[0];
        // IsBreakingGround=true;
        // StartCoroutine(DeathCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(IsBreakingGround){
            return;
        }
        if(LastAttactTime==-1){
            if(Ready()){
                LastAttactTime=Time.time;
            }
        }
        else if(Time.time-LastAttactTime>AttactCD[Floor]&&!tester.StopAttact&&Ready()){
            Attact();
        }

        WaitAddTime+=Time.deltaTime;             //等待的动画切换
        if(WaitAddTime>=WaitAnimCD){
            WaitAnimNum=WaitAnimNum==6?1:WaitAnimNum+1;
            WaitAddTime=0;
        }
    }
    void Attact(){
        LastAttactTime=-1;
        LeftReady=false;
        RightReady=false;
        var APpair=APData.GetRandomPairByFloor(Floor);
        LeftHand.Attact(APpair.LeftHandPatten,APpair.LeftDelayTime,APpair.LeftLocalPosition);
        RightHand.Attact(APpair.RightHandPatten,APpair.RightDelayTime,APpair.RightLocalPosition);
    }
    bool Ready(){
        return LeftReady&&RightReady;
    }
    public void DieOut(){
        if(IsBreakingGround){
            return;
        }
        Vector2 Direction=(Vector2)((transform.position+new Vector3(-0.038f,0.133f,0))-(Player.position+new Vector3(0,3,0))).normalized;
        wave.TriggerWaveWithDirection(Direction);
        NowHit+=1;
        MC.AddScore(1.1f);
        if(NowHit>=NeedHit[Floor]){
            IsBreakingGround=true;
            StartCoroutine(ColliderOffBool());
            if(Floor==6){
                StartCoroutine(DeathCoroutine());
                return;
            }
            Floor+=1;
            NowHit=0;
            StartCoroutine(BreakGround());
        }
        else{
            StartCoroutine(ColliderOffTime(HitGapTime[Floor]));
        }
    }
    IEnumerator ColliderOffTime(float WaitTime){
        itcollider.enabled=false;
        yield return new WaitForSeconds(WaitTime);
        itcollider.enabled=true;
    }
    IEnumerator ColliderOffBool(){
        itcollider.enabled=false;
        yield return new WaitUntil(()=>!IsBreakingGround); 
        itcollider.enabled=true;
    }
    IEnumerator BreakGround(){
        LeftReady=false;
        RightReady=false;
        StartCoroutine(LeftHand.BreakGround());
        yield return StartCoroutine(RightHand.BreakGround());
        // yield return new WaitForSeconds(3f);
        Vector2 OriginPosition=FloorPosition[Floor-1];
        Vector2 TargetPosition=FloorPosition[Floor];
        float AddTime=0f;
        while(AddTime<NextFloorMoveTime){
            transform.position=OriginPosition+NextFloorMoveCurve.Evaluate(AddTime/NextFloorMoveTime)*(TargetPosition-OriginPosition);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        transform.position=TargetPosition;
        LeftHand.NextFloor();
        RightHand.NextFloor();
        yield return new WaitForSeconds(1f);
        IsBreakingGround=false;
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3f);
        LeftHand.StartCoroutine(LeftHand.TrueDeath());
        RightHand.StartCoroutine(RightHand.TrueDeath());
        yield return new WaitForSeconds(1f);
        float AddTime=0;
        Vector3 originalPosition=transform.position;
        while (AddTime<shakeDuration){
            float currentFrequency=minFrequency+(maxFrequency-minFrequency)*TrueDeathShakeCurve.Evaluate(AddTime/shakeDuration);
            float currentAmount=0.5f*shakeAmount+0.5f*shakeAmount*TrueDeathShakeCurve.Evaluate(1-AddTime/shakeDuration);
            float xShake = Mathf.Sin(Time.time * currentFrequency * Mathf.PI) * currentAmount;
            float yShake = Mathf.Cos(Time.time * currentFrequency * Mathf.PI * 0.7f) * currentAmount;
            Vector3 shakeOffset=new Vector3(xShake,yShake,0);
            transform.position=originalPosition+shakeOffset;
            AddTime+=Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
        yield return new WaitForSeconds(1f);
        DeadDemonBroad.SetActive(true);
        DeadDemonBroad.transform.position=transform.position;
        MC.eatthing=0;
        manager.LastBite();
        Destroy(gameObject);
    }
}
