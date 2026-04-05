using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBroad : MonoBehaviour
{
    public SpriteRenderer SR;
    public MCscript MC;
    public Transform MCtransform;
    public Transform DemonBroadtransform;
    public Dialogue4 D4;
    private bool InBroad=false;
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            StartCoroutine(Appear());
            InBroad=true;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            if (!gameObject.activeInHierarchy) return;
            StartCoroutine(Disappear());
            InBroad=false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InBroad&&Input.GetKeyDown(KeyCode.F)&&!MC.IsReading){
            StartCoroutine(Disappear());
            StartCoroutine(Dialogue());
        }
    }
    IEnumerator Dialogue(){
        MC.IsReading=true;
        MC.rb.velocity=new Vector2(0,MC.rb.velocity.y);
        MC.anim.SetInteger("State",0);
        MC.transform.localScale=new Vector2(10*MC.LocalScaleLock,10);
        D4.SetDialogue(27,32);
        yield return new WaitUntil(()=>!MC.IsReading);
        MCtransform.position=DemonBroadtransform.position;
    }
    IEnumerator Appear(){
        if(SR.color.a!=1f){
            float currentAlpha=0f;
            while(currentAlpha<1f){
                currentAlpha+=Time.deltaTime/0.2f;
                SR.color = new Color(1,1,1,currentAlpha);
                yield return null;
            }
        }
    }
    IEnumerator Disappear(){
        if(SR!=null&&SR.color.a!=0f){
            float currentAlpha=1f;
            while(currentAlpha>0){
                currentAlpha-=Time.deltaTime/0.2f;
                SR.color = new Color(1,1,1,currentAlpha);
                yield return null;
            }
        }
    }
}
