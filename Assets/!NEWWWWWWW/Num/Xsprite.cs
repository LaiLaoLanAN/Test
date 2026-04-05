using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xsprite : MonoBehaviour
{

    private SpriteRenderer SR;
    private Vector2 OriginPos;
    public AnimationCurve ShakeCurve;
    private float shakeAmount;
    private float shakeTime;
    private Coroutine ShakeCorotine;

    // Start is called before the first frame update
    void Start()
    {
        SR=GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize(float OriginX, float _shakeAmount, float _shakeTime)
    {
        OriginPos = new Vector2(OriginX, 0);
        shakeAmount = _shakeAmount;
        shakeTime = _shakeTime;
    }
    public void Shakef()
    {
        if (ShakeCorotine != null)
        {
            StopCoroutine(ShakeCorotine);
        }
        ShakeCorotine = StartCoroutine(Shake());
    }
    IEnumerator Shake()
    {
        float timer = 0;
        Vector2 TargetPos = new Vector3(OriginPos.x + Random.Range(-shakeAmount, shakeAmount), OriginPos.y + Random.Range(-shakeAmount, shakeAmount));
        while (timer < shakeTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPos, ShakeCurve.Evaluate(timer / shakeTime));
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = TargetPos;
    }
    public void Disappear()
    {
        SR.enabled = false;
    }
    public void Appear()
    {
        SR.enabled = true;
    }
}
