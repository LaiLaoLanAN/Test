using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootDemonBroad : MonoBehaviour
{
    private SpriteRenderer SR;
    public Stage4Manager manager;
    public Vector3 DemonPos;
    public Vector3 DemonSize;
    public AnimationCurve Curve;
    public float MoveMiddleTime;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        SR=GetComponent<SpriteRenderer>();
        SR.enabled=false;
        transform.localScale=new Vector2(1,1);
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot(){
        SR.enabled=true;
        transform.parent=null;
        StartCoroutine(MoveMiddle());
    }
    IEnumerator MoveMiddle(){
        Vector3 OriginSize=transform.localScale;
        Vector3 OriginPos=transform.position;
        Vector3 OriginAngles=transform.eulerAngles;
        Vector3 DemonAngles=new Vector3(0,0,-720);
        float AddTime=0;
        float Progress=0;
        while(AddTime<MoveMiddleTime){
            Progress=Curve.Evaluate(AddTime/MoveMiddleTime);
            transform.position=OriginPos+Progress*(DemonPos-OriginPos);
            transform.localScale=OriginSize+Progress*(DemonSize-OriginSize);
            transform.eulerAngles=OriginAngles+Progress*(DemonAngles-OriginAngles);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        Invoke("DemonStart",0.5f);
    }
    void DemonStart(){
        anim.SetTrigger("DemonStart");
    }
    void DemonEnd(){
        manager.DemonDialogue();
    }
}
