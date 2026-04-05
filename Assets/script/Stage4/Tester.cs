using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public MCscript MC;
    public Transform MCFollower;
    public DemonHand LeftHand;
    public DemonHand RightHand;
    public DemonBroad DB;
    public bool TESTMODE;
    public bool StopAttact;
    public int StartFloor;
    // Start is called before the first frame update
    void Start()
    {
        if(!TESTMODE){
            StopAttact=false;
        }
        else{
            Invoke("StartFloorf",0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)){
            RightHand.Attact(AttactPatten.crash,0.5f,new Vector2(0,0));
            // PlayerPrefs.SetInt("StageComplete",3); 
        }
    }
    void StartFloorf(){
        DB.Floor=StartFloor;
        DB.transform.position=DB.FloorPosition[DB.Floor];
        MC.transform.position=DB.transform.position;
        MCFollower.position=MC.transform.position;
        // LeftHand.TeleportCoroutine=StartCoroutine(LeftHand.Teleport(LeftHand.WaitPosition,true,false));
        // RightHand.TeleportCoroutine=StartCoroutine(RightHand.Teleport(RightHand.WaitPosition,true,false));
    }
}
