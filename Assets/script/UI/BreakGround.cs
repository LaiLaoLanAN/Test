using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakGround : MonoBehaviour
{
    private Animator anim;
    public Dialogue3 dialogue3;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        Invoke("F1",1f);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void F1(){
        anim.SetTrigger("1");
        Invoke("F2",0.2f);
    }
    void F2(){
        anim.SetTrigger("2");
        Invoke("F3",1f);
    }
    void F3(){
        anim.SetTrigger("3");
        Invoke("F4",4f);
    }
    void F4(){
        if(PlayerPrefs.GetInt("NeededPoem", 0)==1){
            PlayerPrefs.SetInt("NeededPoem",0);
            PlayerPrefs.Save();
            int num=PlayerPrefs.GetInt("StageComplete", 0);
            switch(num)
            {
                case 1:
                    dialogue3.SetDialogue(0,11);
                    break;
                case 2:
                    dialogue3.SetDialogue(12,18);
                    break;
                case 3:
                    dialogue3.SetDialogue(19,24);
                    break;
                case 4:
                    dialogue3.SetDialogue(25,36);
                    PlayerPrefs.SetInt("StageComplete",0); 
                    break;
            }
        }
        else{
            anim.SetTrigger("4");
            Invoke("Back",2f);
        }
    }
    public void F4Back(){
        anim.SetTrigger("4");
        Invoke("Back",1f);
    }
    void Back(){
        SceneManager.LoadScene(1);
    }
}
