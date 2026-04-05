using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MCColliderEnterRoom : MonoBehaviour
{
    public int Broadnum;
    public SpriteRenderer SR;
    public MCEnterRoom MC;
    public Dialogue2 D2;
    public UnityEvent InDepth;
    void Start(){
        SR.color = new Color(1,1,1,0);
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Broad")){
            StartCoroutine(Appear());
            Broadnum=int.Parse(other.gameObject.name.Substring(5));
        }
        if(other.CompareTag("Respawn")){
            InDepth.Invoke();
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Broad")){
            StartCoroutine(Disappear());
            Broadnum=0;
        }
    }

    void Update(){
        if(Broadnum!=0&&Input.GetKeyDown(KeyCode.F)&&!MC.IsReading){
            StartCoroutine(Disappear());
            Dialogue();
        }
    }


    void Dialogue(){
        MC.IsReading=true;
        MC.rb.velocity=new Vector2(0,MC.rb.velocity.y);
        MC.anim.SetInteger("State",0);
        MC.transform.localScale=new Vector2(10*MC.LocalScaleLock,10);
        D2.SetDialogueID(Broadnum);
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
