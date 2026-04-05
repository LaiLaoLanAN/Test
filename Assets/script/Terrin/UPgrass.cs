using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPgrass : MonoBehaviour
{
    [SerializeField]private int direction;
    [SerializeField]private int UpSpeed;
    private Animator anim;
    void OnTriggerEnter2D(Collider2D other){
        IUpable iUpable=other.GetComponent<IUpable>();
        if(iUpable!=null){
            iUpable.LetUp(direction,UpSpeed);
            anim.SetTrigger("UP");
        }
    }
    void Start(){
        anim=GetComponent<Animator>();
    }
}
