using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBanana : MonoBehaviour
{
    public MCscript MC;
    public BITE bite;
    private Rigidbody2D rb;
    private BoxCollider2D BC2D;
    private Renderer RE;
    private bool IsRotating=false;
    private int count;
    private bool SBready=true;


    void OnTriggerEnter2D(Collider2D other){
        IDamagableE iDamagableE=other.GetComponent<IDamagableE>();
        if(iDamagableE!=null){
            iDamagableE.DieOut();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        BC2D=GetComponent<BoxCollider2D>();
        RE=GetComponent<Renderer>();
        BC2D.enabled=false;
        RE.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector2(MC.transform.position.x,MC.transform.position.y+2f);
        if(!IsRotating){
            transform.localEulerAngles=new Vector3(0,0,-MC.MouseAngle);
        } 
    }
    public void Shoot(){
        if(!SBready){
            return;
        }
        SBready=false;
        IsRotating=true;
        RE.enabled=true;
        BC2D.enabled=true;
        count=0;
        Invoke("Rotate1",0.15f);
    }
    void Rotate1(){
        count++;
        rb.angularVelocity=150*count;
        if(count<10){
            Invoke("Rotate1",0.1f);
        }
        else{
            Invoke("Rotate2",0.1f);
        }
    }
    void Rotate2(){
        count--;
        rb.angularVelocity=100*count;
        if(count>0){
            Invoke("Rotate2",0.1f);
        }
        else{
            Invoke("End",0.1f);
        }
    }
    void End(){
        RE.enabled=false;
        BC2D.enabled=false;
        MC.eatthing=0;
        IsRotating=false;
        bite.BananaEnd();
        SBready=true;
    }
}
