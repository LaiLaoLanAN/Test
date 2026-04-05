using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGuideButton : MonoBehaviour
{
    public Sprite[] sprite;
    private Image image;
    private Button button;
    public int ID;
    public string saying;
    public TMP_Text tmptext;
    public bool IsStart;
    // Start is called before the first frame update
    void Start()
    {
        int stagecomplete=PlayerPrefs.GetInt("StageComplete",0);
        int deathnum=PlayerPrefs.GetInt("DeathNum",0);
        image=GetComponent<Image>();
        button=GetComponent<Button>();
        if((stagecomplete<ID||(IsStart&&deathnum==0))&&deathnum<=20){
            image.sprite=sprite[0];
            button.enabled=false;
        }
        else{
            image.sprite=sprite[1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GuideText(){
        tmptext.text=saying;
    }
}
