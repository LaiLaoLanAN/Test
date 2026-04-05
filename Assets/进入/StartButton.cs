using UnityEngine;
using UnityEngine.Events;

public class StartButton : MonoBehaviour
{
    [Header("事件设置")]
    public UnityEvent onClick; // 点击事件
    
    [Header("视觉效果")]
    public Color normalColor;
    public Color hoverColor;
    public Color pressedColor;
    public float colorChangeSpeed = 5f;
    public PolygonCollider2D PC2D1;
    public PolygonCollider2D PC2D2;
    public PolygonCollider2D PC2D3;

    private SpriteRenderer spriteRenderer;
    private bool isHovered;
    private bool isPressed;
    private Camera mainCamera;

    public void Collider2DOff(){
        PC2D1.enabled=false;
        PC2D2.enabled=false;
        PC2D3.enabled=false;
    }
    public void Collider2DOn(){
        PC2D1.enabled=true;
        PC2D2.enabled=true;
        PC2D3.enabled=true;
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        spriteRenderer.color = normalColor;
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
        
        // 检测鼠标位置是否在碰撞器内
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);
        
        if (hitCollider != null && hitCollider.gameObject == gameObject)
        {
            isHovered = true;
            
            // 检测鼠标点击
            if (Input.GetMouseButtonDown(0))
            {
                isPressed = true;
                onClick.Invoke(); // 触发点击事件
            }
        }
        
        // 检测鼠标释放
        if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }
    }

    void UpdateVisuals()
    {
        Color targetColor = normalColor;
        
        if (isPressed)
        {
            targetColor = pressedColor;
        }
        else if (isHovered)
        {
            targetColor = hoverColor;
        }
        
        // 平滑过渡颜色
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * colorChangeSpeed);
    }
}