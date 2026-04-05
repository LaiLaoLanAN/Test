using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootApplePre : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D CC2D;
    [SerializeField]private float Bombr;
    private bool IsBomb;
    private List<GameObject> collidedObjects =new List<GameObject>();

    void OnTriggerEnter2D(Collider2D other){
        if(!IsBomb&&(other.gameObject.tag!="Player"&&other.gameObject.tag!="Weapon")){
            anim.SetBool("IsBomb",true);
            rb.bodyType=RigidbodyType2D.Kinematic;
            rb.velocity=Vector2.zero;
            IsBomb=true;
        }
        if(!collidedObjects.Contains(other.gameObject)){
            collidedObjects.Add(other.gameObject);
        }

    }
    void OnTriggerExit2D(Collider2D other){
        collidedObjects.Remove(other.gameObject);
        
    }

    void Detect(){
        CC2D.radius=Bombr;
    }

    void Bomb(){
        var objProcess=new List<GameObject>(collidedObjects);
        foreach(var obj in objProcess){
            IDamagableE iDamagableE=obj.GetComponent<IDamagableE>();
            if(iDamagableE!=null){
                iDamagableE.DieOut();
            }
        }
    }

    void End(){
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        anim.SetBool("IsBomb",false);
        rb=GetComponent<Rigidbody2D>();
        CC2D=GetComponent<CircleCollider2D>();
        CC2D.radius=0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
