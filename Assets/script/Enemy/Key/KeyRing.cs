using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyRing : MonoBehaviour,IDamagableE
{
    public float Acceleration;
    public Vector2 Direction;
    private Rigidbody2D rb;
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Terrin")||other.CompareTag("Player")){
            DieOut();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        rb.velocity=Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity+=Acceleration*Direction*Time.deltaTime;
        if(!CheckInCamrea()){
            DieOut();
        }
    }
    public void DieOut(){
        Destroy(gameObject);
    }
    bool CheckInCamrea(){
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        return (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0);
    }
}
