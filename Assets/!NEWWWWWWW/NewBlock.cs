using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBlock : MonoBehaviour
{
    [SerializeField] private Vector3 StartPos;
    [SerializeField] private Vector3 EndPos;
    [SerializeField] private float StartTime;
    [SerializeField] private float EndTime;
    private TimeManager timeManager;
    // Start is called before the first frame update
    void Start()
    {
        timeManager = TimeManager.Instance;
        transform.position = StartPos;
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = timeManager.CurrentTime;
        if (currentTime > StartTime && currentTime < EndTime)
        {
            transform.position = Vector3.Lerp(StartPos, EndPos, (currentTime - StartTime) / (EndTime - StartTime));
        }
    }
}
