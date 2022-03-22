using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerControl : MonoBehaviour
{
    public bool firemod = false;
    public int rndnum;
    public GameObject swordlight;
    public GameObject model;
    public GameObject Muzzle,Arrow;



    public effectinfo[] skill1eff;
    public effectinfo[] airskill1eff;
    public effectinfo[] skill2eff;
    public effectinfo[] skill3eff;
    public effectinfo[] effect;
    [System.Serializable]
    public class effectinfo
    {        
        public GameObject eff;
        public Transform efftransform;
    }

    private Animator anim;
    public stateManager sm;
    
   
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        swordlight.SetActive(false);
    }
    #region FireSword
    public void ResetTrigger(string triggerName)
    {
        anim.ResetTrigger(triggerName);
    }
    public void Efftrigger(int triggeratknum)
    {     
        if(firemod == true)
        {
        var isw  = Instantiate(effect[triggeratknum].eff, effect[triggeratknum].efftransform.position, effect[triggeratknum].efftransform.rotation);
        SoundManager.instance.fireattacks();
        Destroy(isw,2.0f);
        }
        else
        {
            swordlight.SetActive(true);
            rndnum = Random.Range(0, 3);
            SoundManager.instance.atks(rndnum);
        }
       
    }

    public void Skilleff(int skillnum)
    {
        var isw = Instantiate(skill2eff[skillnum].eff, skill2eff[skillnum].efftransform.position, skill2eff[skillnum].efftransform.rotation);
        
        if(sm.FireEnergy< sm.FireEnergyMax) sm.FireEnergy += 0.1f;
        sm.FireEnergybar.fillAmount = sm.FireEnergy;
        if (sm.FireEnergy >= 1f)
        {
            sm.FireIcon.SetActive(true);
            firemod = true;
        }

        if (skillnum == 0)
        {
        SoundManager.instance.skill2p(1);
        }    
        Destroy(isw, 3.0f);
    }
    public void skill2_2st()
    {
        if (sm.FireEnergy < sm.FireEnergyMax) sm.FireEnergy += 0.1f;
        sm.FireEnergybar.fillAmount = sm.FireEnergy;
        if (sm.FireEnergy >= 1f)
        {
            sm.FireIcon.SetActive(true);
            firemod = true;
        }


        SoundManager.instance.fireattacks();
    }

    public void skill1st(int skillnum)
    {
        if (sm.FireEnergy < sm.FireEnergyMax) sm.FireEnergy += 0.1f;
        sm.FireEnergybar.fillAmount = sm.FireEnergy;
        if (sm.FireEnergy >= 1f)
        {
            sm.FireIcon.SetActive(true);
            firemod = true;
        }

        var isw = Instantiate(skill1eff[skillnum].eff, skill1eff[skillnum].efftransform.position, skill1eff[skillnum].efftransform.rotation);
        Destroy(isw, 3.0f);
        SoundManager.instance.skill1p(skillnum);
    }

    public void airskill1st(int skillnum)
    {
        if (sm.FireEnergy < sm.FireEnergyMax) sm.FireEnergy += 0.1f;
        sm.FireEnergybar.fillAmount = sm.FireEnergy;
        if (sm.FireEnergy >= 1f)
        {
            sm.FireIcon.SetActive(true);
            firemod = true;
        }

        var isw = Instantiate(airskill1eff[skillnum].eff, airskill1eff[skillnum].efftransform.position, airskill1eff[skillnum].efftransform.rotation);
        Destroy(isw, 3.0f);
    }

    public void skill3st(int skillnum)
    {
        if (sm.FireEnergy < sm.FireEnergyMax) sm.FireEnergy += 0.1f;
        sm.FireEnergybar.fillAmount = sm.FireEnergy;
        if (sm.FireEnergy >= 1f)
        {
            sm.FireIcon.SetActive(true);
            firemod = true;
        }

        var isw = Instantiate(skill3eff[skillnum].eff, skill3eff[skillnum].efftransform.position, skill3eff[skillnum].efftransform.rotation);
        Destroy(isw, 3.0f);
    }

    public void Reseteff()
    {
        swordlight.SetActive(false);

    }

    public void skill1trigger()
    {
        if (sm.FireEnergy < sm.FireEnergyMax) sm.FireEnergy += 0.1f;
        sm.FireEnergybar.fillAmount = sm.FireEnergy;
        if (sm.FireEnergy >= 1f)
        {
            sm.FireIcon.SetActive(true);
            firemod = true;
        }
        Vector3 modelorigin = model.transform.position;
        Vector3 boxCenter = modelorigin + model.transform.forward * 2.5f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(1f, 2.5f, 2.5f), model.transform.rotation, LayerMask.GetMask("Enemy"));
        foreach (var col in cols)
        {
         print(col.name);
            col.gameObject.GetComponent<Rigidbody>().AddForce(0, 500, 0);
        }
    }
    #endregion

    #region Bow
    public void BowShoot()
    {
        Muzzle.gameObject.transform.rotation = model.gameObject.transform.rotation;
        Instantiate(Arrow.gameObject,Muzzle.gameObject.transform.position,Muzzle.transform.rotation);
        SoundManager.instance.BowShoots();
        print("ok");
    }
    #endregion
}
