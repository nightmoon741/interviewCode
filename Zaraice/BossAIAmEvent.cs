using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIAmEvent : MonoBehaviour
{
    public Collider weaponCol;
    public GameObject weaponEff;
    public Effinfo[] Effect;

    [System.Serializable]
    public class Effinfo
    {
        public GameObject Eff;
        public Transform EffPos;
    }
  public void WeaponOn()
    {
        weaponCol.enabled = true;
        weaponEff.SetActive(true);
    }
  public void WeaponOff()
    {
        weaponCol.enabled = false;
        weaponEff.SetActive(false);
    }

    public void Atk1_1()
    {
        SoundManager.instance.BossATK1_1();
    }
    public void Atk1_3()
    {
        SoundManager.instance.BossATK1_3();
        var isw = Instantiate(Effect[0].Eff, Effect[0].EffPos.position, Effect[0].EffPos.rotation);
        Destroy(isw,0.5f);
    }
    public void Atk2_1()
    {
        SoundManager.instance.BossATK2_1();

    }
    public void Atk2_2()
    {
        SoundManager.instance.BossATK2_2();
        var isw = Instantiate(Effect[1].Eff, Effect[1].EffPos.position, Effect[1].EffPos.rotation);
        Destroy(isw, 1f);
    }
    public void Atk2_3()
    {
        SoundManager.instance.BossATK2_3();
    }

    public void EndSkill1()
    {
        var isw = Instantiate(Effect[2].Eff, Effect[2].EffPos.position, Effect[2].EffPos.rotation);
        Destroy(isw, 3f);
    }
    public void EndSkill1_2()
    {
        var isw = Instantiate(Effect[3].Eff, Effect[3].EffPos.position, Effect[3].EffPos.rotation);
        Destroy(isw, 3f);
    }




}
