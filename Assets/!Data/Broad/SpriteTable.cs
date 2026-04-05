using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteTable", menuName = "Data/Sprite Table")]
public class SpriteTable : ScriptableObject
{
    [System.Serializable]
    public class SpriteTables
    {
        public int SPid;
        public Sprite sprite1;
        public Sprite sprite2;
    }

    public List<SpriteTables> spriteTables = new List<SpriteTables>();

    public SpriteTables GetPairById(int SPid)
    {
        return spriteTables.Find(pair => pair.SPid == SPid);
    }
}
