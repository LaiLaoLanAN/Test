using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HVMore : MonoBehaviour
{
    [Header("出来")]
    public float OutDistance;
    public AnimationCurve OutCurve;
    public float OutTime;
    [Header("进心")]
    public AnimationCurve InHeartCurve;
    public Vector2 HeartPosition;
    public RebirthHeart RH;
    [Header("其他")]
    private RectTransform RT;
    private float AddTime;
    public Vector2 AdjustV;
    void Start()
    {
        RT=GetComponent<RectTransform>();
        RT.anchoredPosition+=AdjustV;
        StartCoroutine(MoveToHeart());
    }
    IEnumerator MoveToHeart(){
        Vector2 OutPosition=(Vector2)RT.anchoredPosition+Random.insideUnitCircle.normalized*OutDistance;
        Vector2 OriginPosition=(Vector2)RT.anchoredPosition;
        AddTime=0;
        while(AddTime<OutTime){
            AddTime+=Time.deltaTime;
            RT.anchoredPosition=OriginPosition+(OutPosition-OriginPosition)*OutCurve.Evaluate(AddTime/OutTime);
            yield return null;
        }
        RT.anchoredPosition=OutPosition;
        yield return new WaitForSeconds(1f);
        AddTime=0;
        float InHeartTime=0.3f+0.7f*(OutPosition.x-HeartPosition.x)/2000;              //0.3到1
        while(AddTime<InHeartTime){
            AddTime+=Time.deltaTime;
            RT.anchoredPosition=OutPosition+(HeartPosition-OutPosition)*InHeartCurve.Evaluate(AddTime/InHeartTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
