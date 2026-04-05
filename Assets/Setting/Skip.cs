using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour
{
    public AnimationCurve InCurve;
    public float InTime;
    public Vector2 OutPosition;
    public Vector2 InPosition;
    private RectTransform RT;
    // Start is called before the first frame update
    void Start()
    {
        RT=GetComponent<RectTransform>();
        RT.anchoredPosition=OutPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Appearf(){
        StartCoroutine(Appear());
    }
    private IEnumerator Appear(){
        float AddTime=0;
        while(AddTime<InTime){
            RT.anchoredPosition=OutPosition+InCurve.Evaluate(AddTime/InTime)*(InPosition-OutPosition);
            AddTime+=Time.deltaTime;
            yield return null;
        }
    }
}
