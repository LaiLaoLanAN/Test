using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGenerator : MonoBehaviour,IDamagableE
{
    public bool CanShoot;
    private float LastShootTime;
    private float LastInCameraTime;
    [SerializeField]private float ShootCD;
    public GameObject KeyRing;
    [SerializeField]private float KeyRingAcceleration;
    private MCscript MC;
    private Transform MCtransform;
    [Header("1")]
    private Animator anim;
    private Collider2D itcollider;
    private bool IsReady;
    // Start is called before the first frame update
    void Start()
    {
        LastShootTime=Time.time;
        MC=FindObjectOfType<MCscript>();
        MCtransform=MC.transform;
        anim=GetComponent<Animator>();
        itcollider=GetComponent<Collider2D>();
        itcollider.enabled=false;
        IsReady=false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled=!CanShoot;
    }

    // Update is called once per frame
    void Update()
    {
        if(CanShoot&&IsReady&&CheckInCamera()&&Time.time-LastShootTime>ShootCD){
            GameObject ShootKR=Instantiate(KeyRing,transform.position,transform.rotation);
            KeyRing keyring=ShootKR.GetComponent<KeyRing>();
            keyring.Acceleration=KeyRingAcceleration;
            keyring.Direction=(MCtransform.position-new Vector3(0,3,0)-transform.position).normalized;
            LastShootTime=Time.time;
        }
    }
    bool CheckInCamera(){
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        return (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0);
    }
    public void DieOut(){
        anim.SetTrigger("BITE");
        itcollider.enabled=false;
        IsReady=false;
    }
    void Reset(){
        itcollider.enabled=true;
        IsReady=true;
    }
}
