using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4Broad : MonoBehaviour,IDamagableE
{
    public Stage4Manager manager;
    public MCscript MC;
    public float FadeTime;
    public AnimationCurve FadeCurve;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DieOut(){
        StartCoroutine(Disappear());
    }
    IEnumerator Disappear(){
        manager.IsChangingDemon=true;
        SpriteRenderer SR=GetComponent<SpriteRenderer>();
        GetComponent<Collider2D>().enabled=false;
        float AddTime=0;
        bool IsGiven=false;
        while(AddTime<FadeTime){
            SR.color=new Color(1,1,1,1-FadeCurve.Evaluate(AddTime/FadeTime));
            AddTime+=Time.deltaTime;
            if(!IsGiven&&AddTime/FadeTime>0.75){
                MC.eatthing=5;
                IsGiven=true;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
