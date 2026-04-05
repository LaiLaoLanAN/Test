using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttactPattenData", menuName = "Data/AttactPatten Data")]
public class AttactPattenData : ScriptableObject
{
    // public enum AttactPatten{wait,grab,put,shoot,crash};   //在DemonBroad中被定义
    [System.Serializable]
    public class AttactPattenPair
    {
        public int Floor;
        public AttactPatten LeftHandPatten;
        public float LeftDelayTime;
        public Vector2 LeftLocalPosition;
        public AttactPatten RightHandPatten;
        public float RightDelayTime;
        public Vector2 RightLocalPosition;
    }

    public List<AttactPattenPair> AttactPattenPairs = new List<AttactPattenPair>();

    public AttactPattenPair GetRandomPairByFloor(int floor)
    {
        List<AttactPattenPair> attactpattenpairs = AttactPattenPairs.FindAll(pair => pair.Floor == floor);
        if(attactpattenpairs.Count == 0){
            return null;
        }
        int randomIndex = Random.Range(0,attactpattenpairs.Count);
        return attactpattenpairs[randomIndex];

    }
}