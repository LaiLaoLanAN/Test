using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBroadDeath : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("被射出")]
    public Vector2 ShootSpeed;
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Terrin")){
            rb.velocity=Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        transform.eulerAngles=new Vector3(0,0,-56);
        rb.velocity=ShootSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
