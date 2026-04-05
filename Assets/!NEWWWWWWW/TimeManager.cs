using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool IsQPressed = false;
    public bool IsEPressed = false;
    public float CurrentTime = 0f;
    public float TimeRate = 1;

    public static TimeManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        IsQPressed=Input.GetKey(KeyCode.Q);
        IsEPressed=Input.GetKey(KeyCode.E);
        if (IsQPressed && !IsEPressed)
        {
            if (CurrentTime > 0)
            {
                TimeRate = -4;
            }
            else
            {
                TimeRate = 0;
            }
        }
        else if(!IsQPressed && IsEPressed)
        {
            TimeRate = 4;
        }
        else if (!IsQPressed && !IsEPressed)
        {
            TimeRate = 1;
        }
        else
        {
            TimeRate = 0;
        }
        CurrentTime += Time.deltaTime * TimeRate;
    }
}
