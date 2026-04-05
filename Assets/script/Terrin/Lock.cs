using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    private SpriteRenderer SR;
    public float FadeTime;
    // Start is called before the first frame update
    void Start()
    {
        SR=GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UnLock(){
        GetComponent<Collider2D>().enabled=false;
        StartCoroutine(Disappear());
    }
    IEnumerator Disappear(){
        float r=SR.color.r;
        float g=SR.color.g;
        float b=SR.color.b;
        float AddTime=0;
        while(AddTime<FadeTime){
            SR.color=new Color(r,g,b,1-AddTime/FadeTime);
            AddTime+=Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
