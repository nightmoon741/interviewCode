using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class stateManager : IactorManagerIn
{
    public float HpMax = 15.0f;
    public float Hp = 15.0f;
    public float FireEnergyMax = 1;
    public float FireEnergy = 0;
    public Image Hpbar,FireEnergybar;
    public GameObject FireIcon;

    void Start()
    {
        Hp = HpMax;
        SoundManager.instance.bgm2s();
    }
    public void AddHP(float value)
    {
        Hp += value;
        Hp = Mathf.Clamp(Hp, 0, HpMax);
        Hpbar.fillAmount = Hp / HpMax;
        if (Hp > 0)
        {
            am.hit();       
        }
        else
        {
            am.die();
            StartCoroutine(DelayFunc(4f));
        }
    }
    // Start is called before the first frame update
  public void test()
    {

    }
    IEnumerator DelayFunc(float index)//延遲協程
    {
        yield return new WaitForSeconds(index);//回傳等待秒數  
        {
            SceneManager.LoadScene(0);
        }
        yield return null;
    }
}
