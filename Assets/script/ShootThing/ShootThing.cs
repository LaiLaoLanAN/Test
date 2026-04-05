using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootThing : MonoBehaviour
{
    public MCscript MC;
    public GameObject Shootapplepre;
    public GameObject Shootwatermelonpre;    
    [SerializeField]private float MaxShootSpeedApple;
    public GameObject ShootOrangePre;
    [SerializeField]private float ShootSpeedOrange;
    public void ShootApple(){
        GameObject ShootingApple=Instantiate(Shootapplepre,transform.position,transform.rotation);
        float ShootSpeed=MC.MouseDistance;
        if(ShootSpeed>MaxShootSpeedApple){
            ShootSpeed=MaxShootSpeedApple;
        }
        ShootingApple.GetComponent<Rigidbody2D>().velocity=MC.MouseDirection*ShootSpeed;
    }
    public void ShootWaterMelon(bool IsFacingRight){
        GameObject ShootingWaterMelon=Instantiate(Shootwatermelonpre,transform.position,transform.rotation);
        ShootingWaterMelon.GetComponent<ShootWaterMelonPre>().IsFacingRight=IsFacingRight;
    }
    public void ShootOrange(){
        GameObject ShootingOrange=Instantiate(ShootOrangePre,transform.position+(Vector3)MC.MouseDirection*6f,transform.rotation);
        ShootingOrange.GetComponent<Rigidbody2D>().velocity=MC.MouseDirection*ShootSpeedOrange;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
