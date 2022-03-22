using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource effaudioSource,BgmSource,BossSource;
    public AudioClip fireattack,walk,EnterBtn,GameStart,BtnSound,EnterMenu,bgm,BowShoot,ArrowPassBy,bgm2;
    public AudioClip[] atk; 
    public AudioClip[] skill1;
    public AudioClip[] skill2;

    public AudioClip[] BossSound;
 

    private void Awake()
    {
        instance = this;
        BossSource = GameObject.Find("EN").GetComponent<AudioSource>();
        DontDestroyOnLoad(instance);
    }
    #region ui---------------------------------------------------------------------
   public void bgms()
    {
        BgmSource.clip = bgm;
        BgmSource.Play();
    }
    public void bgm2s()
    {
        BgmSource.clip = bgm2;
        BgmSource.Play();
    }

    public void EnterBtns()
    {
        effaudioSource.clip = EnterBtn;
        effaudioSource.Play();
    }
    public void EnterMenus()
    {
        effaudioSource.clip = EnterMenu;
        effaudioSource.Play();
    }

    public void GameStarts()
    {
        effaudioSource.clip = GameStart;
        effaudioSource.Play();
    }
    public void BtnSounds()
    {
        effaudioSource.clip = BtnSound;
        effaudioSource.Play();
    }
    #endregion

    #region FireSword
    public void fireattacks()
    {
        effaudioSource.clip = fireattack;
        effaudioSource.Play();
    }

    public void walks()
    {
        effaudioSource.clip = walk;
        effaudioSource.Play();
    }

    public void atks(int rndnum)
    {
        effaudioSource.clip = atk[rndnum];
        effaudioSource.Play();
    }

    public void skill2p(int s)
    {
        effaudioSource.clip = skill2[s];
        effaudioSource.Play();
    }

    public void skill1p(int s)
    {
        effaudioSource.clip = skill1[s];
        effaudioSource.Play();
    }
    #endregion

    #region Bow
    public void BowShoots()
    {
        effaudioSource.clip = BowShoot;
        effaudioSource.PlayOneShot(BowShoot);
    }

    public void ArrowPassBys()
    {
        effaudioSource.clip = ArrowPassBy;
        effaudioSource.PlayOneShot(ArrowPassBy);
    }
    #endregion

    #region Boss

    public void BossATK1_1()
    {
        BossSource.clip = BossSound[0];
        BossSource.Play();
    }
    public void BossATK1_3()
    {
        BossSource.clip = BossSound[1];
        BossSource.Play();
    }
    public void BossATK2_1()
    {
        BossSource.clip = BossSound[2];
        BossSource.Play();
    }
    public void BossATK2_2()
    {
        BossSource.clip = BossSound[3];
        BossSource.Play();
    }
    public void BossATK2_3()
    {
        BossSource.clip = BossSound[4];
        BossSource.Play();
    }

    #endregion


}
