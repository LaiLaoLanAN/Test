using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioFitter : MonoBehaviour 
{
    private float targetAspect = 2560/1440; // 你的UI设计比例

    void Start() 
    {
        UpdateViewport();
        // Screen.fullScreenChanged += UpdateViewport;
    }
    void Update(){
        UpdateViewport();
    }
    void UpdateViewport() 
    {
        float currentAspect = (float)Screen.width / Screen.height;
        
        if (currentAspect> targetAspect) 
        {
            // 屏幕过宽时，左右扩展视口并填充黑边
            float widthScale = targetAspect / currentAspect;
            GetComponent<Camera>().rect = new Rect((1 - widthScale) / 2, 0, widthScale, 1);
        }
        else 
        {
            // 屏幕过高时，保持原始比例（Unity默认会在上下填充黑边）
            GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        }
    }
}