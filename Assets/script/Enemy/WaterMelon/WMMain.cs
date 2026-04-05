using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WMMain : MonoBehaviour
{
    public UnityEvent<bool> Shoot;
    public UnityEvent DieOutEvent;
    public float InToOutDistance;
    public float InToOutTime;
    public float OutToInDistance;
    public float OutToInTime;
    private Transform MCtransform;
    private MCscript MC;

    public bool Is_in=true;
    public bool Is_out=false;
    private bool Is_Rotate=false;
    private float RotateSpeed=0;
    [SerializeField]private float RotateAcceleration;
    [SerializeField]private float RotateMaxSpeed;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        MC=FindObjectOfType<MCscript>();
        MCtransform=MC.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float MCDistancesqr=(MCtransform.position-transform.position).sqrMagnitude;
        if(Is_in&&MCDistancesqr<InToOutDistance*InToOutDistance){
            Is_in=false;
            Shoot.Invoke(true);
            Invoke("InToOutFinish",InToOutTime);
        }
        else if(Is_out&&MCDistancesqr<OutToInDistance*OutToInDistance){
            Is_out=false;
            Is_Rotate=false;
            RotateSpeed=0;
            rb.angularVelocity=0;
            // Shoot.Invoke(false);
            Invoke("OutToInFinishPre",0.4f);
        }
        if(Is_Rotate){
            if(RotateSpeed<RotateMaxSpeed){
                RotateSpeed+=RotateAcceleration*Time.deltaTime;
                rb.angularVelocity=RotateSpeed;
            }
        }

    }
    public void InToOutFinish(){
        Invoke("rotate",0.2f);
    }
    void rotate(){
        Is_out=true;
        Is_Rotate=true;
    }
    public void OutToInFinishPre(){
        Shoot.Invoke(false);
        TurnToZero();
        Invoke("OutToInFinish",OutToInTime);
    }
    public void OutToInFinish(){
        Invoke("Stop",1f);
        rb.angularVelocity=0;
    }
    void Stop(){
        Is_in=true;
    }
    void TurnToZero(){
        float an=transform.eulerAngles.z%360;
        float AV=((an-180)<0?-an:360-an)/(OutToInTime);
        rb.angularVelocity = AV;
    }
    public void DieOut(){
        MC.AddScore(1.5f);
        DieOutEvent.Invoke();
        rb.angularVelocity=0;
        Is_Rotate=false;
        Invoke("ParentDie",1.1f);
        this.enabled=false;
    }
    void ParentDie(){
        Destroy(transform.parent.gameObject);
    }
}
