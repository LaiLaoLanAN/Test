using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldNumber : MonoBehaviour
{
    public List<GameObject> Numbers=new List<GameObject>();
    public float NumberSize;
    public GameObject NumPre;
    public bool AllowZero;

    public float shakeAmount;
    public float shakeTime;

    public Xsprite x;
    [SerializeField]private int LastNumDigits;
    private void Start()
    {
        foreach(GameObject Num in Numbers)
        {
            Num num=Num.GetComponent<Num>();
            num.Initialize(0, shakeAmount, shakeTime);
        }
        x.Initialize(-NumberSize, shakeAmount, shakeTime);
        ChangeWorldNumber(0);
    }
    public void ChangeWorldNumber(int Num)
    {
        if (Num <= 0)
        {
            Num = 0;
        }
        int _Num = Num;
        List<int> NumberList = new List<int>();
        if (AllowZero && _Num == 0)
        {
            NumberList.Add(0);
        }
        while (_Num > 0)
        {
            NumberList.Add(_Num%10);
            _Num =(int)_Num/10;
        }
        int digits=NumberList.Count;
        if (digits != LastNumDigits)
        {
            while (Numbers.Count > 0)
            {
                Destroy(Numbers[0]);
                Numbers.RemoveAt(0);
            }

            //float RightPos = ((digits - 1)/2)*NumberSize;
            float RightPos = (digits - 1) * NumberSize;
            if (digits > 0)
            {
                x.Appear();
            }
            else
            {
                x.Disappear();
            }
            x.Shakef();
            for (int i = 0; i < digits; i++)
            {
                GameObject NewNum = Instantiate(NumPre, transform);
                Num num = NewNum.GetComponent<Num>();
                num.Initialize(RightPos,shakeAmount,shakeTime);
                num.ChangeNum(NumberList[i]);
                Numbers.Add(NewNum);
                RightPos -= NumberSize;
            }
        }
        else
        {
            for (int i = 0; i < digits; i++)
            {
                Numbers[i].GetComponent<Num>().ChangeNum(NumberList[i]);
            }
            x.Shakef();
        }
        LastNumDigits=digits;
    }
}
