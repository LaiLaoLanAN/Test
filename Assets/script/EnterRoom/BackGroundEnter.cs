using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BackGroundEnter : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public List<Sprite> SP;
    [SerializeField]private string[] SongName;
    void Start(){
        image1.color=new Color(1f,1f,1f,0f);
        image2.color=new Color(1f,1f,1f,0f);
        StartCoroutine(Appear());
    }
    IEnumerator Appear(){
        float currentAlpha=0f;
        while(currentAlpha<1f){
            currentAlpha+=Time.deltaTime/1f;
            image1.color=new Color(1f,1f,1f,currentAlpha);
            yield return null;
        }
    }
    public void saying(){
        StartCoroutine(Saying());
    }
    IEnumerator Saying(){
        yield return new WaitForSeconds(1f);
        int randomIndex = Random.Range(0,SP.Count);
        image2.sprite=SP[randomIndex];
        MusicManager.Instance.PlayTrack(SongName[randomIndex]);
        float currentAlpha=0f;
        while(currentAlpha<1f){
            currentAlpha+=Time.deltaTime/1f;
            image2.color=new Color(1f,1f,1f,currentAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        while(currentAlpha>0f){
            currentAlpha-=Time.deltaTime/1.5f;
            image2.color=new Color(1f,1f,1f,currentAlpha);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Stage"+(PlayerPrefs.GetInt("StageComplete",0)+1));
    }
}
