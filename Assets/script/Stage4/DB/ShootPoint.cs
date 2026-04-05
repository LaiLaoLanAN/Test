using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPoint : MonoBehaviour
{
    private LineRenderer LR;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groudLayer;
    [SerializeField] private LayerMask playerLayer;
    private RaycastHit2D hitGround;
    private RaycastHit2D hitPlayer;
    public Transform Player;
    public MCCollider MCcollider;
    private bool IsDeadShoot=false;
    private Vector2 Direction;
    // Start is called before the first frame update
    void Start()
    {
        LR=GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsDeadShoot){
            hitPlayer=Physics2D.Raycast(origin:transform.position,direction:Direction,distance:rayDistance,layerMask:playerLayer);
            if(hitPlayer.collider!=null&&(hitGround.point-(Vector2)transform.position).sqrMagnitude>=(hitPlayer.point-(Vector2)transform.position).sqrMagnitude){
                IsDeadShoot=false;
            }
        }
    }
    public IEnumerator Shoot(){
        LR.enabled=true;
        LR.startColor=new Color(0.84f,0.40f,0.91f,1f);
        LR.endColor=new Color(0.84f,0.40f,0.91f,1f);
        LR.startWidth=1f;
        LR.endWidth=1f;
        Vector2 realplayerposition=(Vector2)Player.position+new Vector2(0,3f);
        Direction=(realplayerposition-(Vector2)transform.position).normalized;
        hitGround = Physics2D.Raycast(origin:transform.position,direction:Direction,distance:rayDistance,layerMask:groudLayer);
        LR.SetPosition(0,transform.position);
        if(hitGround.collider!=null){
            LR.SetPosition(1,hitGround.point);
        }
        else{
            LR.SetPosition(1,(Vector2)transform.position+Direction*rayDistance);
        }
        yield return new WaitForSeconds(0.9f);
        LR.enabled=false;
        yield return new WaitForSeconds(0.3f);
        IsDeadShoot=true;
        LR.enabled=true;
        LR.startColor=new Color(0.69f,0.06f,0.93f,1f);
        LR.endColor=new Color(0.69f,0.06f,0.93f,1f);
        LR.startWidth=3f;
        LR.endWidth=3f;
        yield return new WaitForSeconds(0.5f);
        LR.enabled=false;
        IsDeadShoot=false;
    }
    public void Fuck(){
        StopAllCoroutines();
        LR.enabled=false;
        IsDeadShoot=false;
    }

}
