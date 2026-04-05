using UnityEngine;
using Cinemachine;
using System.Collections;

public class CinemachineZoom2D : MonoBehaviour
{
    [Header("更改zoomDuration时同时需要改RebirthHeart中协程启动描述")]
    [Tooltip("更改时同时需要改RebirthHeart中协程启动描述")]
    public float zoomDuration;          // 过渡时间 更改时同时需要改RebirthHeart中协程启动描述
    public float targetOrthoSize;       // 目标大小
    public AnimationCurve zoomCurve;          // 动画曲线
    private CinemachineVirtualCamera vcam;
    private float originalOrthoSize;
    private Coroutine zoomRoutine;
    [Header("复活")]
    public float BackDuration;
    private float startSize;
    public AnimationCurve BackCurve;
    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        originalOrthoSize = vcam.m_Lens.OrthographicSize;
    }

    public void StartZoom(){
        StartCoroutine(ZoomRoutine());
    }
    public void BackZoom(){
        StartCoroutine(ZoomBack());
    }
    IEnumerator ZoomRoutine(){
        startSize = vcam.m_Lens.OrthographicSize;
        float elapsed = 0f;
        while (elapsed < zoomDuration){
            elapsed += Time.deltaTime;
            float t =zoomCurve.Evaluate(elapsed/zoomDuration);
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(startSize,targetOrthoSize,t);
            yield return null;
        }
    }
    IEnumerator ZoomBack(){
        float elapsed = 0f;
        while (elapsed<BackDuration){
            elapsed += Time.deltaTime;
            float t =BackCurve.Evaluate(elapsed/BackDuration);
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(targetOrthoSize,startSize,t);
            yield return null;
        }
    }
}