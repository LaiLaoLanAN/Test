using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMOut : MonoBehaviour
{
    [Header("运动参数")]
    public WMMain wmMain;
    public float targetDistance;  // 目标移动距离
    private float totalTime;      // 总运动时间（秒）
    public Vector2 direction;// 移动方向（需归一化）
    public Vector2 targetdirection;

    [Header("状态")]
    public bool isMoving = false;
    public float elapsedTime = 0f;
    public float currentSpeed = 0f;

    private bool Out_In;
    private float acceleration;
    private Vector2 StartPosion;
    void Start()
    {
        direction = direction.normalized;
        acceleration = (2 * targetDistance) / Mathf.Pow(totalTime, 2);
    }
    void Update()
    {
        if (!isMoving) return;

        elapsedTime += Time.deltaTime;
        
        // 运动阶段
        if (elapsedTime <= totalTime)
        {
            // 计算当前速度 v = a * t
            currentSpeed = acceleration * elapsedTime;
            
            // 计算位移 deltaS = v * deltaTime
            float deltaDistance = currentSpeed * Time.deltaTime;
            transform.Translate((Vector3)targetdirection * deltaDistance);
        }
        // 停止阶段
        else
        {
            Brake();
        }
    }

    // 开始移动
    public void StartMove(bool out_in)
    {  
        Out_In=out_in;
        targetdirection=Out_In?direction:-direction;
        totalTime=Out_In?wmMain.InToOutTime:wmMain.OutToInTime;
        StartPosion=Out_In?Vector2.zero:transform.localPosition;
        elapsedTime = 0f;
        currentSpeed = 0f;
        acceleration = (2 * targetDistance) / Mathf.Pow(totalTime, 2);
        isMoving = true;
    }

    // 瞬间刹车
    public void Brake()
    {
        isMoving = false;
        currentSpeed = 0f;
        if(!Out_In){
            transform.localPosition=Vector2.zero;
        }
    }
    
}