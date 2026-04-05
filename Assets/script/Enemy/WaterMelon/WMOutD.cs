using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMOutD : MonoBehaviour
{
    private Collider2D colliderWM;
    private WMOut wmout;
    private SpriteRenderer SR;
    // Start is called before the first frame update
    void Start()
    {
        wmout=GetComponent<WMOut>();
        SR=GetComponent<SpriteRenderer>();
        colliderWM=GetComponent<Collider2D>();
    }
    public void DieOut(){
        colliderWM.enabled=false;
        wmout.enabled=false;
        StartCoroutine(Disappear());
    }
    IEnumerator Disappear(){
        yield return new WaitForSeconds(0.5f);
        float currentAlpha=1f;
        while(currentAlpha>0){
            currentAlpha-=Time.deltaTime/0.6f;
            SR.color=new Color(1,1,1,currentAlpha);
            yield return null;
        }
        Destroy(gameObject);
    }
}
