using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPurpleScipt : MonoBehaviour
{
    public GameObject randomPurple;
    [SerializeField]private float MaxShootSpeed;
    public Sprite[] SP;
    public void Shoot(){
        for(int i=0;i<200;i++){
            Vector3 RPposition=transform.position+new Vector3(Random.Range(-2,2),Random.Range(1,6),0);
            GameObject RandomPurplePre=Instantiate(randomPurple,RPposition,transform.rotation);
            float randomangle = Random.Range(0, 2 * Mathf.PI);
            Vector2 randomV=new Vector2(Mathf.Cos(randomangle), Mathf.Sin(randomangle));
            float ShootSpeed = Random.Range(0.4f*MaxShootSpeed,MaxShootSpeed);
            int RSP=Random.Range(0,4);
            RandomPurplePre.GetComponent<Rigidbody2D>().velocity=ShootSpeed*randomV;
            RandomPurplePre.GetComponent<SpriteRenderer>().sprite=SP[RSP];
        }
    }
    void Start(){
    }
}
