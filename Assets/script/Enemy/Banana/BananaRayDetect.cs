using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaRayDetect : MonoBehaviour
{
    private float ShootTime;
    private bool IsShoot=false;
    public Banana Banana;
    private Rigidbody2D rb;
    private MCCollider MCC;
    public float TerrinTime=-1f;
    public float PlayerTime=-1f;
    // Start is called before the first frame update
     void OnTriggerEnter2D(Collider2D other){
        if(IsShoot){
            Debug.Log("1");
            if(other.CompareTag("Terrin")){
                rb.velocity=Vector2.zero;
                IsShoot=false;
                TerrinTime=Time.time;
                Debug.Log("2");
            }
            if(IsShoot&&other.CompareTag("Player")){
                rb.velocity=Vector2.zero;
                IsShoot=false;
                PlayerTime=Time.time;
            }
        }
    }
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        MCC=FindObjectOfType<MCCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsShoot){
            transform.localPosition=new Vector2(0.109f,0.079f);
        }
        if(IsShoot&&Time.time-ShootTime>3f){
            rb.velocity=Vector2.zero;
            transform.localPosition=new Vector2(0.109f,0.079f);
            IsShoot=false;
        }
    }
    void LateUpdate(){
        if(PlayerTime>0&&TerrinTime>PlayerTime){
            Debug.Log("3");
        }
    }
    public void Shoot(Vector2 Direction){
        rb.velocity=Direction*300f;
        ShootTime=Time.time;
        IsShoot=true;
    }
}
