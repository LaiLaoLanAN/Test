using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootKey : MonoBehaviour
{
    public MCscript MC;
    public float MCAddSpeedAmount;
    private SpriteRenderer SR;
    private bool IsKeyDashing=false;
    private float LockAngle;
    private Collider2D itcollider;
    private bool IsAir;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other){
        IDamagableE iDamagableE=other.gameObject.GetComponent<IDamagableE>();
        if(iDamagableE!=null&&!other.CompareTag("Bubble")&&!other.CompareTag("Key")){
            iDamagableE.DieOut();
        }
        Lock keylock=other.gameObject.GetComponent<Lock>();
        if(keylock!=null){
            keylock.UnLock();
        }
        StopKeyDash();
    }
    void Start()
    {
        itcollider=GetComponent<Collider2D>();
        SR=GetComponent<SpriteRenderer>();
        SR.enabled=false;
        itcollider.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1)&&MC.HaveKey&&!Input.GetMouseButton(0)){
            LockAngle=-MC.MouseAngle;
            MC.AddSpeedDirected(MCAddSpeedAmount);
            MC.HaveKey=false;
            SR.enabled=true;
            IsKeyDashing=true;
            IsAir=false;
            itcollider.enabled=true;
            Invoke("Air",0.1f);
        }
    }
    void Air(){
        IsAir=true;
    }
    void LateUpdate(){
        if(IsKeyDashing){
            transform.eulerAngles=new Vector3(0,0,LockAngle);
            if(IsAir&&(MC.IsGround||Input.GetKeyDown(KeyCode.LeftShift)||Input.GetMouseButton(0))){
                StopKeyDash();
            }
        }
    }
    void StopKeyDash(){
        SR.enabled=false;
        IsKeyDashing=false;
        itcollider.enabled=false;
    }
}
