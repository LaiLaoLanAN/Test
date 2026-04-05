using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour,IDamagableE
{
    private float LastShootTime;
    private float LastInCameraTime;
    [SerializeField]private float ShootCD;
    public GameObject KeyRing;
    [SerializeField]private float KeyRingAcceleration;
    private MCscript MC;
    private Transform MCtransform;
    // Start is called before the first frame update
    void Start()
    {
        LastShootTime=Time.time;
        MC=FindObjectOfType<MCscript>();
        MCtransform=MC.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckInCamera()&&Time.time-LastShootTime>ShootCD){
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
        MC.AddScore(1.3f);
        Destroy(gameObject);
    }
}
