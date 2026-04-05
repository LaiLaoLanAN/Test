using System.Collections.Generic;
using UnityEngine;

public class CircleGenerator : MonoBehaviour
{
    //[SerializeField] private float startRadius;
    //[SerializeField] private float endRadius;
    //[SerializeField] private Color startColor;
    //[SerializeField] private Color endColor;
    //[SerializeField] private float lineWidth;
    //[SerializeField] private int segments = 50;
    //[SerializeField] private BeatData beatData;
    //private List<RingData> activeRings = new List<RingData>();

    //[SerializeField] private float TotalWindowTime;
    //[SerializeField] private Transform Player;
    //[SerializeField] private Vector3 offset;
    //private int CurrentI=0;

    //[System.Serializable]
    //class RingData
    //{
    //    public LineRenderer lineRenderer;
    //    public int CurrentI;
    //    public float currentRadius;
    //}

    //void Update()
    //{
    //    SpawnRing();
    //    UpdateRings();
    //    transform.position = Player.position + offset;
    //}


    //void SpawnRing()
    //{
    //    if (!BeatManager.Instance.IsPlaying || BeatManager.Instance.IsRecording)
    //    {
    //        return;
    //    }
    //    float NowTime = Time.time - beatData.BeatStartTime;
    //    if (beatData.beats[CurrentI] - NowTime <= TotalWindowTime)
    //    {
    //        //Debug.Log(1);
    //        GameObject ringObj = new GameObject("Ring2D");
    //        ringObj.transform.SetParent(transform);
    //        ringObj.transform.localPosition = Vector3.zero;

    //        // 添加 LineRenderer
    //        LineRenderer lr = ringObj.AddComponent<LineRenderer>();

    //        // 配置 LineRenderer
    //        ConfigureLineRenderer(lr);

    //        // 生成圆环点
    //        GenerateCirclePoints(lr, startRadius);

    //        RingData data = new RingData
    //        {
    //            lineRenderer = lr,
    //            CurrentI = this.CurrentI,
    //            currentRadius= startRadius
    //        };

    //        activeRings.Add(data);
    //        CurrentI++;
    //    }
    //}

    //void ConfigureLineRenderer(LineRenderer lr)
    //{
    //    lr.useWorldSpace = false;
    //    lr.loop = true;

    //    // 使用默认 Sprite 材质
    //    lr.material = new Material(Shader.Find("Sprites/Default"));

    //    lr.startColor = startColor;
    //    lr.endColor = startColor;

    //    lr.startWidth = lineWidth;
    //    lr.endWidth = lineWidth;

    //    lr.alignment = LineAlignment.View;
    //    lr.textureMode = LineTextureMode.Tile;

    //    // 2D 排序
    //    lr.sortingOrder = 1;
    //    lr.sortingLayerName = "Effects";
    //}

    //void GenerateCirclePoints(LineRenderer lr, float radius)
    //{
    //    Vector3[] points = new Vector3[segments + 1];

    //    float angle = 0f;
    //    float angleStep = 360f / segments;

    //    for (int i = 0; i <= segments; i++)
    //    {
    //        float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
    //        float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
    //        points[i] = new Vector3(x, y, 0);
    //        angle += angleStep;
    //    }

    //    lr.positionCount = points.Length;
    //    lr.SetPositions(points);
    //}

    //void UpdateRings()
    //{
    //    for (int i = activeRings.Count - 1; i >= 0; i--)
    //    {
    //        RingData data = activeRings[i];

    //        if (data.lineRenderer == null)
    //        {
    //            activeRings.RemoveAt(i);
    //            continue;
    //        }


    //        if (beatData.CurrentBeatNum > data.CurrentI)
    //        {
    //            Destroy(data.lineRenderer.gameObject);
    //            activeRings.RemoveAt(i);
    //        }

    //        else
    //        {
    //            data.currentRadius = Mathf.Lerp(startRadius, endRadius, 1-(beatData.beats[data.CurrentI] + beatData.BeatStartTime - Time.time) / TotalWindowTime);
    //            GenerateCirclePoints(data.lineRenderer, data.currentRadius);

    //            // 更新颜色
    //            Color currentColor = Color.Lerp(startColor, endColor, 1-(beatData.beats[data.CurrentI] + beatData.BeatStartTime - Time.time) / TotalWindowTime);
    //            data.lineRenderer.startColor = currentColor;
    //            data.lineRenderer.endColor = currentColor;

    //            // 可选：脉动效果
    //            AddPulseEffect(data);
    //        }
    //    }
    //}



    //void AddPulseEffect(RingData data)
    //{
    //    // 轻微脉动
    //    float pulse = Mathf.Sin(Time.time * 10f) * 0.05f;
    //    Color pulseColor = data.lineRenderer.startColor;
    //    pulseColor.a += pulse * 0.1f;
    //    data.lineRenderer.startColor = pulseColor;
    //    data.lineRenderer.endColor = pulseColor;
    //}
}