// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class AllScreenButton : MonoBehaviour
// {
//     private bool Check;
//     public Image image;
//     void Start()
//     {
//         Screen.fullScreen=false;
//         Check=Screen.fullScreen;
//         image.enabled=Check;
//     }
    
//     public void Change(){
//         Check=!Check;
//         if(Check){
//             image.enabled=true;
//         }
//         else{
//             image.enabled=false;
//         }
//         Screen.fullScreen=Check;
//     }
// }
using UnityEngine;
using UnityEngine.UI; // 添加UI命名空间

public class AllScreenButton : MonoBehaviour
{
    public Color normalColor=Color.white;
    public Color hoverColor=Color.gray;
    public Color pressedColor=Color.black;
    public float colorChangeSpeed=5f;
    public bool Check;
    public Image checkimage;
    public LayerMask targetLayers;

    private Image image;
    private bool isHovered;
    private bool isPressed;
    private Camera mainCamera;
    public bool IsFirst;
    void Start()
    {
        image=GetComponent<Image>();
        image.color=normalColor;
        mainCamera=Camera.main;
        if(IsFirst){
            Screen.fullScreen=true;
            Check=true;
            checkimage.enabled=Check;
        }
        else{
            Check=Screen.fullScreen;
            checkimage.enabled=Check;
        }
    }

    void Update()
    {
        UpdateButtonState();
        UpdateVisuals();
    }
    void UpdateButtonState()
    {
        isHovered=false;
        Vector2 mouseWorldPos=mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider=Physics2D.OverlapPoint(mouseWorldPos, targetLayers);

        if (hitCollider != null && hitCollider.gameObject == gameObject){
            isHovered=true;
            if (Input.GetMouseButtonDown(0)){
                isPressed=true;
                ExecuteExitAction();
            }
        }
        if (Input.GetMouseButtonUp(0)){
            isPressed=false;
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
        Check=!Check;
        checkimage.enabled=Check;
        Screen.fullScreen=Check;
    }
    public void Change(){
        ExecuteExitAction();
    }
}