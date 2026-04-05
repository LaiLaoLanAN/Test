using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Data/Save Data")]
public class SaveData : ScriptableObject
{
    public int DeathNum;
    public int CompleteNum;
}
