
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Num : MonoBehaviour
{
    public float FadeDuration;
    public AnimationCurve FadeCurve;
    protected Coroutine FadeCoroutine;
    private SpriteRenderer SR;
    [HideInInspector] public Vector2 OriginPos;
    public Sprite[] sprites;

    [SerializeField] private AnimationCurve ShakeCurve;
    [HideInInspector]public float shakeAmount;
    [HideInInspector] public float shakeTime;
    private Coroutine ShakeCorotine;

    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update  
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize(float OriginX,float _shakeAmount,float _shakeTime)
    {
        OriginPos = new Vector2(OriginX, 0);
        shakeAmount = _shakeAmount;
        shakeTime = _shakeTime;
    }
    public void ChangeNum(int _num)
    {
        if (ShakeCorotine != null)
        {
            StopCoroutine(ShakeCorotine);
        }
        ShakeCorotine = StartCoroutine(Shake(_num));
    }
    IEnumerator Shake(int _num)
    {
        SR.sprite = sprites[_num];
        float timer = 0;
        Vector2 TargetPos= new Vector3(OriginPos.x + Random.Range(-shakeAmount, shakeAmount), OriginPos.y + Random.Range(-shakeAmount, shakeAmount));
        while (timer < shakeTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPos, ShakeCurve.Evaluate(timer / shakeTime));
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = TargetPos;
    }
    //IEnumerator Appear()
    //{
    //    float timer = 0;
    //    float r = SR.color.r;
    //    float g = SR.color.g;
    //    float b = SR.color.b;
    //    while (timer < FadeDuration)
    //    {
    //        SR.color = new Color(r, g, b, FadeCurve.Evaluate(timer / FadeDuration));
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //    SR.color = new Color(r, g, b, 1);
    //}
    //IEnumerator Disappear()
    //{
    //    float timer = 0;
    //    float r = SR.color.r;
    //    float g = SR.color.g;
    //    float b = SR.color.b;
    //    while (timer < FadeDuration)
    //    {
    //        SR.color = new Color(r, g, b, 1 - FadeCurve.Evaluate(timer / FadeDuration));
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }
    //    SR.color = new Color(r, g, b, 0);
    //}
}
