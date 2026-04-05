using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMIn : MonoBehaviour,IDamagableE
{
    public WMMain wmMain;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    private bool IsStatic=true;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider=GetComponent<CircleCollider2D>();
        rb=GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        // DieOut();
    }

    // Update is called once per frame
    void Update(){
        if(IsStatic){
            transform.position=wmMain.transform.position;
        }
    }
    public void Shoot(bool Out_In){
        if(Out_In){
            circleCollider.enabled=true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            IsStatic=false;
        }
        else{
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity=-transform.localPosition/wmMain.OutToInTime;
            Invoke("UpMiddleFinish",wmMain.OutToInTime);
        }
    }
    void UpMiddleFinish(){
        transform.localPosition=Vector2.zero;
        circleCollider.enabled=false;
        rb.velocity=Vector2.zero;
        IsStatic=true;
    }
    public void DieOut(){
        wmMain.DieOut();
        Destroy(gameObject);
    }                       //为省性能Addscore在wmMain中
}
