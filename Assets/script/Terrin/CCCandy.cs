using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCCandy : MonoBehaviour
{
    public Sprite spritecandy;
    public Sprite spriteempty;
    public SpriteRenderer SR;
    private MCscript MC;
    [SerializeField]private float AddScoreAmount;
    void Start(){
        MC=FindObjectOfType<MCscript>();
        SR.sprite=spritecandy;
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            MC.AddScore(AddScoreAmount/MC.AddScoreAmount);
            SR.sprite=spriteempty;
            Destroy(gameObject);
        }
    }
}
