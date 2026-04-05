using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouthImage : MonoBehaviour
{
    
    public Sprite[] SPs;
    public Image itImage;
    public Image keyImage;
    public MCscript MC;
    // Start is called before the first frame update
    void Start()
    {
        itImage.color=new Color(1f,1f,1f,0f);
        StartCoroutine(Appear());
    }
    IEnumerator Appear(){
        yield return new WaitForSeconds(2f);
        float currentAlpha=0f;
        while(currentAlpha<1f){
            currentAlpha+=Time.deltaTime/0.4f;
            itImage.color=new Color(1f,1f,1f,currentAlpha);
            yield return null;
        }
    }
    public void Disappearf(){
        StartCoroutine(Disappear());
    }
    IEnumerator Disappear(){
        float currentAlpha=1f;
        while(currentAlpha>0f){
            currentAlpha-=Time.deltaTime/0.4f;
            itImage.color=new Color(1f,1f,1f,currentAlpha);
            keyImage.color=new Color(1f,1f,1f,currentAlpha);
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        itImage.sprite=SPs[MC.eatthing];
        if(keyImage.enabled!=MC.HaveKey){
            keyImage.enabled=MC.HaveKey;
        }
    }
}
