using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameMenuManager : MonoBehaviour
{
    public bool AdmissionB = false;
    public GameObject Main,GameMenu,Flash,GameSetting;
    public Animator FadeAnimation;
    // Start is called before the first frame update

    public void AdmissionStart()//主畫面點擊
    {
        if(AdmissionB == false)//判斷是否以點擊過主畫面
        {
         SoundManager.instance.EnterMenus();
        Main.SetActive(false);//關閉主畫面
        Flash.SetActive(true);//開啟閃光
        StartCoroutine(DelayFunc(0.4f, "Admission"));//調用協程延遲
        AdmissionB = true;
        }
      
    }
    public void playgame()//開始遊戲
    {
        FadeAnimation.SetBool("FadeIn", true);
        FadeAnimation.SetBool("FadeOut", false);
        SoundManager.instance.GameStarts();
        AdmissionB = false;//開始遊戲將主畫面點擊清除
        StartCoroutine(DelayFunc(1, "GameStart"));
        SoundManager.instance.BgmSource.Stop();
    }

    public void GameSettingEnter()//進入主選單
    {
        GameMenu.SetActive(false);
        GameSetting.SetActive(true);
        SoundManager.instance.BtnSounds();
    }
    public void GameSettingExit()//離開主選單
    {
        GameSetting.SetActive(false);
        GameMenu.SetActive(true);
        SoundManager.instance.BtnSounds();
    }

    public void quitgame()//關閉遊戲
    {
        FadeAnimation.SetBool("FadeIn", true);
        FadeAnimation.SetBool("FadeOut", false);
        GameMenu.SetActive(false);
        SoundManager.instance.BtnSounds();
        SoundManager.instance.BgmSource.Stop();
        StartCoroutine(DelayFunc(1,"Quit"));            
    }

    public void EnterSoundEffect()//選擇音效
    {
        SoundManager.instance.EnterBtns();
 
    }

    IEnumerator DelayFunc(float index,string implement)//延遲協程
    {
        yield return new WaitForSeconds(index);//回傳等待秒數
        if(implement == "Admission")
        {
         GameMenu.SetActive(true);
         Flash.SetActive(false);
        }
        if (implement == "Quit")
        {
            Application.Quit();
        }
        if (implement == "GameStart")
        {
            SceneManager.LoadScene(1);
        }
        yield return null;
    }


    void Start()
    {
        SoundManager.instance.bgms();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
