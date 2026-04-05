using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour
{
    [Header("裂隙波设置")]
    public Material crackWaveMaterial;
    public Vector2 waveDirection = Vector2.right;
    public float waveDuration = 2.0f;
    public float waveWidth = 1.0f;
    public float waveIntensity = 0.8f;
    public float edgeSoftness = 0.02f;
    
    [Header("网格设置")]
    public float gridSize = 0.05f;
    public float crackWidth = 0.005f;
    public Color crackColor = Color.black;
    
    [Header("调试控制")]
    public KeyCode triggerKey = KeyCode.Space;
    
    private Coroutine currentWaveCoroutine;
    
    void Start()
    {
        if (crackWaveMaterial == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null) 
                crackWaveMaterial = renderer.material;
        }
        
        InitializeShaderParameters();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            TriggerCrackWave();
        }
    }
    
    void InitializeShaderParameters()
    {
        if (crackWaveMaterial == null) return;
        
        crackWaveMaterial.SetFloat("_WaveProgress", -1f);
        UpdateShaderParameters();
    }
    
    void UpdateShaderParameters()
    {
        if (crackWaveMaterial == null) return;
        
        // 裂隙波参数
        crackWaveMaterial.SetVector("_WaveDirection", new Vector4(waveDirection.x, waveDirection.y, 0, 0));
        crackWaveMaterial.SetFloat("_WaveWidth", waveWidth);
        crackWaveMaterial.SetFloat("_WaveIntensity", waveIntensity);
        crackWaveMaterial.SetFloat("_EdgeSoftness", edgeSoftness);
        
        // 网格参数
        crackWaveMaterial.SetFloat("_GridSize", gridSize);
        crackWaveMaterial.SetFloat("_CrackWidth", crackWidth);
        crackWaveMaterial.SetColor("_CrackColor", crackColor);
    }
    
    public void TriggerCrackWave()
    {
        if (currentWaveCoroutine != null)
            StopCoroutine(currentWaveCoroutine);
        
        currentWaveCoroutine = StartCoroutine(CrackWaveRoutine());
    }
    
    private IEnumerator CrackWaveRoutine()
    {
        if (crackWaveMaterial == null) yield break;
        
        UpdateShaderParameters();
        
        float elapsedTime = 0f;
        
        while (elapsedTime < waveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / waveDuration;
            
            crackWaveMaterial.SetFloat("_WaveProgress", progress);
            yield return null;
        }
        
        // 裂隙波结束
        crackWaveMaterial.SetFloat("_WaveProgress", -1f);
        currentWaveCoroutine = null;
    }
    
    // 预设方向方法
    public void TriggerWaveFromLeft() => TriggerWaveWithDirection(Vector2.right);
    public void TriggerWaveFromRight() => TriggerWaveWithDirection(Vector2.left);
    public void TriggerWaveFromTop() => TriggerWaveWithDirection(Vector2.down);
    public void TriggerWaveFromBottom() => TriggerWaveWithDirection(Vector2.up);
    
    public void TriggerWaveWithDirection(Vector2 direction)
    {
        waveDirection = direction.normalized;
        TriggerCrackWave();
    }
    
    // 设置效果强度
    public void SetStrongEffect()
    {
        waveWidth = 1.5f;
        waveIntensity = 1.0f;
        waveDuration = 3.0f;
        edgeSoftness = 0.01f;
    }
    
    public void SetSubtleEffect()
    {
        waveWidth = 0.5f;
        waveIntensity = 0.6f;
        waveDuration = 1.5f;
        edgeSoftness = 0.05f;
    }
    
    [ContextMenu("测试强裂隙波")]
    public void TestStrongWave()
    {
        SetStrongEffect();
        TriggerCrackWave();
    }
    
    [ContextMenu("测试弱裂隙波")]
    public void TestSubtleWave()
    {
        SetSubtleEffect();
        TriggerCrackWave();
    }
}