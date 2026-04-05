using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllReset : MonoBehaviour
{
    public DialogueData dialoguedata;
    public SaveData saveData;
    void UpdateDialogueData(){
        int lastend=0;
        for(int i=0;i<dialoguedata.dialoguePairs.Count;i++){
            var Mem=dialoguedata.GetPairById(i);
            Mem.startsub=lastend;
            Mem.endsub=lastend+Mem.num;
            lastend=Mem.endsub;
        }
    }
    void ResetSaveData(){
        saveData.DeathNum=0;
        saveData.CompleteNum=0;
        PlayerPrefs.SetInt("DeathNum",0);
        PlayerPrefs.Save();
    }
    void Start(){
        UpdateDialogueData();
        ResetSaveData();
    }
}
