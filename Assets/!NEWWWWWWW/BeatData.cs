using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatData", menuName = "Data/Beat Data")]
public class BeatData : ScriptableObject
{
    public int TotalBeatNum;
    public float BeatWindowTime;
    public List<float> beats;
}
