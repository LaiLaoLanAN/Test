using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPoint : MonoBehaviour
{
    private MCscript MC;
    public Transform Follower;
    public bool IsTesting;
    // Start is called before the first frame update
    void Start(){
        GetComponent<SpriteRenderer>().enabled=false;
        Invoke("LStart",0.02f);
    }
    void LStart()
    {
        MC=FindObjectOfType<MCscript>();
        if(IsTesting){
            MC.transform.position=transform.position;
            Follower.position=transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
