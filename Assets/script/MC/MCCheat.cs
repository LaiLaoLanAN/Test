using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCCheat : MonoBehaviour
{
    private MCscript MC;
    // Start is called before the first frame update
    void Start()
    {
        MC=GetComponent<MCscript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            MC.eatscore+=20;
            Debug.Log("C");
        }
        // if(Input.GetKeyDown(KeyCode.P)){
            
        //     PlayerPrefs.SetInt("DeathNum",0);
        //     PlayerPrefs.SetInt("NeededPoem",0);
        //     PlayerPrefs.SetInt("StageComplete",0);
        //     PlayerPrefs.SetInt("BossMet",0);
        //     PlayerPrefs.Save();
        //     Debug.Log("P");
        // }
        if(Input.GetKeyDown(KeyCode.K)){
            MC.HaveKey=true;
            Debug.Log("K");
        }
    }
}
