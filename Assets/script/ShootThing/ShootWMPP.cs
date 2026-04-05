using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWMPP : MonoBehaviour
{
    public bool IsUp;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField]private float Speed;
    void OnTriggerEnter2D(Collider2D other){
        var obj=other.gameObject;
        anim.SetTrigger("Bomb");
        rb.velocity=Vector2.zero;
        IDamagableE iDamagableE=other.GetComponent<IDamagableE>();
        if(iDamagableE!=null){
            iDamagableE.DieOut();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
        transform.localEulerAngles=new Vector3(0,0,IsUp?0:180);
        rb.velocity=new Vector2(0,(IsUp?1:-1)*Speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Disappear(){
        Destroy(gameObject);
    }
}
