using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MCCollider : MonoBehaviour
{  
    public MCscript MC;
    public Transform MCFollower;
    private bool IsInPlatform=false;
    private void Start()
    {

    }
    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Platform"))
        {
            MC.PlatformTransform = other.transform;
            MC.PlatformLastPos = MC.PlatformTransform.position;
            IsInPlatform=true;
        }
        //if(other.CompareTag("Apple")||(other.CompareTag("Banana"))||(other.CompareTag("DieThing"))||(other.CompareTag("Orange"))||(other.CompareTag("DemonHand"))){
        //    DieF();
        //}
        //if(other.CompareTag("Respawn")){
        //    Respawn();
        //}
        //if(other.CompareTag("Mist")){
        //    MC.InMist=true;
        //}
        //if(other.CompareTag("Broad")){
        //    InBroad=true;
        //    StartCoroutine(Appear());
        //}
    }
    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Platform"))
        {
            MC.PlatformTransform = null;
        }
    }
}
