using UnityEngine;
using UnityEngine.UI; // 添加UI命名空间

public class Exit : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color hoverColor = Color.gray;
    public Color pressedColor = Color.black;
    public float colorChangeSpeed = 5f;

    public LayerMask targetLayers; // 默认检测所有层

    private Image image; // 替换SpriteRenderer为Image
    private bool isHovered;
    private bool isPressed;
    private Camera mainCamera;

    void Start()
    {
        // 获取Image组件
        image = GetComponent<Image>();
        image.color = normalColor;
        mainCamera = Camera.main;
    }

    void Update()
    {
        UpdateButtonState();
        UpdateVisuals();
    }

    void UpdateButtonState()
    {
        // 重置状态
        isHovered = false;
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos, targetLayers);

        if (hitCollider != null && hitCollider.gameObject == gameObject)
        {
            isHovered = true;

            // 鼠标点击检测
            if (Input.GetMouseButtonDown(0))
            {
                isPressed = true;
                ExecuteExitAction();
            }
        }

        // 鼠标释放检测
        if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }
    }

    void UpdateVisuals()
    {
        // 颜色过渡逻辑
        Color targetColor = isPressed ? pressedColor : 
                          isHovered ? hoverColor : 
                          normalColor;

        image.color = Color.Lerp(
            image.color, 
            targetColor, 
            Time.unscaledDeltaTime * colorChangeSpeed
        );
    }

    void ExecuteExitAction()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}