using UnityEngine;
using System.Collections;

public class CameraDie : MonoBehaviour
{
    [Header("效果参数")]
    [SerializeField] private float grayTransitionTime = 1.5f;
    [SerializeField] private float contrastTransitionTime = 1.0f;
    [SerializeField] private float targetGrayIntensity = 0.8f;
    [SerializeField] private float targetContrast = 1.8f; // 推荐1.5-2.0

    public Material effectMaterialPre;
    private Material effectMaterial;
    private Coroutine currentTransition;

    void Start()
    {
        // effectMaterial = new Material(Shader.Find("Custom/ColorEffect"));
        effectMaterial=new Material(effectMaterialPre);
        effectMaterial.SetFloat("_GrayIntensity", 0);
        effectMaterial.SetFloat("_Contrast", 1);
    }

    // 触发高对比灰度效果
    public void ActivateHighContrastGray()
    {
        if (currentTransition != null) StopCoroutine(currentTransition);
        currentTransition = StartCoroutine(TransitionEffect(
            targetGrayIntensity, 
            targetContrast
        ));
    }

    // 恢复正常效果
    public void ResetEffect()
    {
        if (currentTransition != null) StopCoroutine(currentTransition);
        currentTransition = StartCoroutine(TransitionEffect(0, 1));
    }

    IEnumerator TransitionEffect(float targetGray, float targetContrast)
    {
        float startGray = effectMaterial.GetFloat("_GrayIntensity");
        float startContrast = effectMaterial.GetFloat("_Contrast");
        float elapsed = 0;

        while (elapsed < Mathf.Max(grayTransitionTime, contrastTransitionTime))
        {
            elapsed += Time.deltaTime;
            
            // 灰度过渡
            float grayProgress = Mathf.Clamp01(elapsed / grayTransitionTime);
            float currentGray = Mathf.Lerp(startGray, targetGray, grayProgress);
            effectMaterial.SetFloat("_GrayIntensity", currentGray);
            
            // 对比度过渡
            float contrastProgress = Mathf.Clamp01(elapsed / contrastTransitionTime);
            float currentContrast = Mathf.Lerp(startContrast, targetContrast, contrastProgress);
            effectMaterial.SetFloat("_Contrast", currentContrast);
            
            yield return null;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, effectMaterial);
    }

    void OnDestroy()
    {
        if (effectMaterial != null)
            Destroy(effectMaterial);
    }

    // 编辑器调试
    #if UNITY_EDITOR
    [Range(0, 1)] public float debugGray;
    [Range(0, 2)] public float debugContrast;

    void OnValidate()
    {
        if (effectMaterial != null && !Application.isPlaying)
        {
            effectMaterial.SetFloat("_GrayIntensity", debugGray);
            effectMaterial.SetFloat("_Contrast", debugContrast);
        }
    }
    #endif
}