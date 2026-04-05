using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Data/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialoguePair
    {
        public int id;
        public int num;
        public int startsub;
        public int endsub;
    }

    public List<DialoguePair> dialoguePairs = new List<DialoguePair>();

    public DialoguePair GetPairById(int id)
    {
        return dialoguePairs.Find(pair => pair.id == id);
    }
}
