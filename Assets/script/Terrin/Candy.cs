using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    private MCscript MC;
    [SerializeField]private float AddScoreAmount;
    void Start(){
        MC=FindObjectOfType<MCscript>();
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            MC.AddScore(AddScoreAmount/MC.AddScoreAmount);
            Destroy(gameObject);
        }
    }
}
