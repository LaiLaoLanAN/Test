using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BroadData broadData;
    public SpriteTable spriteTable;
    public GameObject prefabToSpawn;
    [SerializeField]public float spacing = 1.0f;
    private int currentSpawnCount = 1;
    public int maxCount;
    public void ExecuteSpawn()
    {
        currentSpawnCount++;
        Vector3 spawnPosition = transform.position + Vector3.right * ((currentSpawnCount-1) * spacing);
        int SPid = broadData.SpritePairs[currentSpawnCount-1];
        if(SPid==13){

        }
        else{
            var pair = spriteTable.GetPairById(SPid);
            GameObject newPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            newPrefab.transform.parent=transform.parent;
            newPrefab.transform.name="Block"+currentSpawnCount.ToString();
            Transform child = newPrefab.transform.GetChild(0);
            child.name = "Broad"+currentSpawnCount.ToString();
            newPrefab.transform.GetComponent<SpriteRenderer>().sprite = pair.sprite1;
            child.GetComponent<SpriteRenderer>().sprite = pair.sprite2;
        }
    }
    void Start(){
        int i=PlayerPrefs.GetInt("DeathNum",0);
        if(i>maxCount-1){
            i=maxCount-1;
        }
        while(currentSpawnCount<=i){
            ExecuteSpawn();
        }
    }
    void Update(){
        // if(Input.GetKeyDown(KeyCode.K)){
        //     if(currentSpawnCount<=maxCount){
        //         ExecuteSpawn();
        //     }
        // }
    }

}
