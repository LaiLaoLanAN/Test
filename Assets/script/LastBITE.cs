using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LastBITE : MonoBehaviour
{
    public MCscript MC;
    public Animator anim;
    public Stage4Manager manager;
    private bool B1;
    private bool B2;
    private PolygonCollider2D PC2D;
    public bool IsLastBite=false;
    public Dictionary<GameObject, int> collidedObjects = new Dictionary<GameObject, int>();
    void OnTriggerEnter2D(Collider2D other){
        GameObject collidedObj=other.gameObject;
        if (collidedObjects.ContainsKey(collidedObj)){
            collidedObjects[collidedObj]++;
        }
        else{
            collidedObjects.Add(collidedObj,1);
        }
    }
    void OnTriggerExit2D(Collider2D other){
        GameObject OutObj=other.gameObject;
        if(collidedObjects.ContainsKey(OutObj)){
            collidedObjects[OutObj]--;
            if(collidedObjects[OutObj]==0){
                collidedObjects.Remove(OutObj);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        PC2D=GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector2(MC.transform.position.x,MC.transform.position.y+2f);
        if(IsLastBite){
            if(Input.GetMouseButtonDown(0)){
                anim.SetTrigger("LastBiteGo2");
            }
        }
        else{
            transform.localEulerAngles=new Vector3(0,0,-MC.MouseAngle);
        }

        if(MC.Closemouth){
            anim.SetTrigger("BITE");
        }
        anim.SetBool("Openmouth",MC.Openmouth||MC.Closemouth);
        MC.Closemouth=false;
        MC.Openmouth=false;
        anim.SetBool("BiteGo2",!collidedObjects.Keys.Any(key => key.tag.Contains("LastDemonBroad")));
    }
    void Bite(){
        var objProcess=new Dictionary<GameObject,int>(collidedObjects);
        bool biteGround=false;
        float rate=1;
        foreach(var obj in objProcess.Keys){
            if(obj!=null){
                IDamagableE iDamagableE=obj.GetComponent<IDamagableE>();
                if(iDamagableE!=null){
                    iDamagableE.DieOut();
                }
                if(obj.tag=="Terrin"&&!biteGround){
                    biteGround=true;
                    rate=1;
                }
            }
        }
        if(biteGround&&MC.eatthing==0){
            MC.BiteGround(rate);
        }
    }
    void LastBite(){
        manager.MCStop();
    }
    void MCAnim(){
        MC.anim.speed=1f;
    }
    void MCAnimSlow(){
        MC.anim.speed=0.4f;
    }
    void LastBiteEnd(){
        IsLastBite=false;
        manager.LastBiteEnd();
    }
}
