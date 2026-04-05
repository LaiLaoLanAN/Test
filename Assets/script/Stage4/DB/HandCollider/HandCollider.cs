using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour,IDamagableE
{
    private Collider2D itcollider;
    public bool IsCollidered=false;
    public DemonHand demonhand;
    public MCscript MC;
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Terrin")){
            IsCollidered=true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        itcollider=GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ColliderOn(){
        IsCollidered=false;
        itcollider.enabled=true;
    }
    public void ColliderOff(){
        IsCollidered=false;
        itcollider.enabled=false;
    }
    public void DieOut(){
        if(demonhand.DB.IsBreakingGround){
            return;
        }
        ColliderOff();
        demonhand.Fuckf();
        MC.AddScore(2f);
    }
}
