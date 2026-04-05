using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MOUSE : MonoBehaviour
{
    public MCscript MC;
    private RectTransform RT;
    private Image image;
    public Image imageIn;
    public AnimationCurve Curve;
    // Start is called before the first frame update
    void Start()
    {
        RT=GetComponent<RectTransform>();
        image=GetComponent<Image>();
        image.enabled=false;
        Invoke("Appear",2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 RealPosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RT.position=RealPosition;
        if(MC.Openmouth){
            imageIn.fillAmount=Curve.Evaluate((Time.time-MC.LastClickTime)/MC.NeededClickTime);
        }
        else{
            imageIn.fillAmount=0;
        }
    }
    void Appear(){
        image.enabled=true;
        imageIn.enabled=true;
    }
    public void Disappear(){
        Destroy(gameObject);
    }
}