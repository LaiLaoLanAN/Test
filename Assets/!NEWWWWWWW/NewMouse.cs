using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMouse : MonoBehaviour
{
    private int CurrentI=0;
    public float TotalWindowTime;
    public float TotalX;
    public GameObject NotePrefreb;
    public float rotationSpeed;
    private RectTransform RT;
    private BeatManager beatManager;
    private void Start()
    {
        RT = GetComponent<RectTransform>();
        beatManager=BeatManager.Instance;
    }
    private void Update()
    {
        Vector2 RealPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RT.position = RealPosition;
        Vector2 MiddlePos = Camera.main.transform.position;
        Vector2 direction= MiddlePos - RealPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        RT.rotation=Quaternion.Slerp(RT.rotation,targetRotation,rotationSpeed * Time.deltaTime);
        if (!BeatManager.Instance.IsPlaying || BeatManager.Instance.IsRecording)
        {
            return;
        }
        if (beatManager.BeatAppearTime(CurrentI)<= TotalWindowTime && CurrentI<beatManager.beatData.TotalBeatNum-1)
        {
            GameObject NewNode = Instantiate(NotePrefreb, transform);
            Note note = NewNode.GetComponent<Note>();
            note.TotalWindowTime = this.TotalWindowTime;
            note.TotalX = this.TotalX;
            note.CurrentI = this.CurrentI;
            CurrentI++;
        }
    }
}
