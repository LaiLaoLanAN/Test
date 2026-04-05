using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnterRoomCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private Vector3 EnterPosition;
    
    private Vector3 currentVelocity;
    private bool isTransitioning = true;
    [SerializeField] private CinemachineVirtualCamera vcam;
    private CinemachineFramingTransposer framingTransposer;
    void Start()
    {
        transform.position = EnterPosition;
        framingTransposer=vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        framingTransposer.m_LookaheadTime=0f;
    }

    void LateUpdate()
    {
        if (isTransitioning)
        {
            // 初始过渡阶段使用更平滑的移动
            transform.position = Vector3.SmoothDamp(transform.position,target.position,ref currentVelocity,smoothTime * 2,maxSpeed);
            
            // 当接近目标时切换为标准跟随
            if (Vector3.Distance(transform.position, target.position) < 0.4f)
            {
                isTransitioning = false;
                transform.position=target.position;
                transform.parent=target;
                framingTransposer.m_LookaheadTime = 0.59f;
            }
        }
    }
    public void Depth(){
        transform.parent=null;
    }
    public void MCGPUPf(){
        transform.parent=null;
        Vector3 target2=transform.position+new Vector3(0,15,0);
        StartCoroutine(MCGPUP(target2));
    }
    IEnumerator MCGPUP(Vector3 target2){
        framingTransposer.m_LookaheadTime = 0f;
        while(!(Vector3.Distance(transform.position, target2) < 0.4f)){
            transform.position = Vector3.SmoothDamp(transform.position,target2,ref currentVelocity,smoothTime*0.2f,maxSpeed*0.07f);
            yield return null; 
        }
    }
}