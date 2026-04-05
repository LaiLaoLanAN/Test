using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOrangePre : MonoBehaviour
{
    private MCscript MC;
    private Animator anim;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other){
        IDamagableE iDamagableE=other.GetComponent<IDamagableE>();
        if(iDamagableE!=null){
            iDamagableE.DieOut();
        }
        if(MC.IsDashing){
            rb.velocity=new Vector2(rb.velocity.x,-rb.velocity.y);
        }
        else{
            anim.SetTrigger("Bomb");
            rb.velocity=Vector2.zero;
            
        }
    }
    void Die(){
        Destroy(gameObject);
    }
    void Start()
    {
        MC=FindObjectOfType<MCscript>();
        anim=GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
