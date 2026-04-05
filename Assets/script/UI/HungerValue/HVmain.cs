using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerValue : MonoBehaviour
{
    public MCscript MC;
    private RectTransform RT;
    public AnimationCurve IOcurve;
    public float UpY;
    public float DownY;
    public float DurationTime=0.8f;
    private bool IsIn=false;

    [Header("孩子们的位移")]            //   分数到位置转化关系为1：10
    [SerializeField]private float smoothtime;
    [SerializeField]private float maxspeed;
    private float currentvelocity;
    private float currentX;

    public RectTransform RTLeft;
    public RectTransform RTRight;
    public RectTransform RTMiddle;
    void Start(){
        RT=GetComponent<RectTransform>();
        StartCoroutine(IO(true));
    }

    void Update(){
        if(IsIn){
            currentX = Mathf.SmoothDamp(current: currentX,
                target:1500*(MC.eatscore/MC.MaxScore),
                currentVelocity: ref currentvelocity,
                smoothTime: smoothtime,
                maxSpeed: maxspeed
            );
            RTLeft.anchoredPosition=new Vector2(-25-currentX,0);
            RTRight.anchoredPosition=new Vector2(25+currentX,0);
            RTMiddle.sizeDelta=new Vector2(2*currentX+10,100);
        }
    }
    IEnumerator IO(bool io){
        yield return new WaitForSeconds(io?2f:1f);
        float currentTime=0f;
        while(currentTime<DurationTime){
            currentTime+=Time.deltaTime;
            float Y;
            if(io){
                Y=UpY-(UpY-DownY)*IOcurve.Evaluate(currentTime/DurationTime);
            }
            else{
                Y=DownY+(UpY-DownY)*IOcurve.Evaluate(currentTime/DurationTime);
            }
            RT.localPosition=new Vector3(0,Y,0);
            yield return null;
        }
        if(io){
            IsIn=true;
        }
    }
    public void FadeOut(){
        StartCoroutine(IO(false));
    }
}