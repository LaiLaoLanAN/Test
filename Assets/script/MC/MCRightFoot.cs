using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MCRightFoot : MonoBehaviour
{
    public bool RIsGround;
    public Collider2D RPlatForm;
    [SerializeField]private float isGroundCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RIsGround=Physics2D.Raycast(transform.position,Vector2.down,isGroundCheck,LayerMask.GetMask("Ground"))||Physics2D.Raycast(transform.position,Vector2.down,isGroundCheck,LayerMask.GetMask("EmptyGround")) || Physics2D.Raycast(transform.position, Vector2.down, isGroundCheck, LayerMask.GetMask("TimeInteractable"));
        RPlatForm = Physics2D.Raycast(transform.position, Vector2.down, isGroundCheck, LayerMask.GetMask("TimeInteractable")).collider;
    }
    private void OnDrawGizmos(){
        Gizmos.DrawLine(transform.position,new Vector2(transform.position.x,transform.position.y-isGroundCheck));
    }
}
