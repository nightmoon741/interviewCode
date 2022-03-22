using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StateType
{
    Idle,Walk,Attack,Down,Skill
}


[Serializable]
public class Parameter
{
    public float Hp = 50;
    public float MaxHp = 50;
    public Animator am;
    public GameObject Target;
    public GameObject modle;
    public GameObject Shadow;
    public GameObject[] ShadowPos;
    public GameObject eff;
    public Transform[] efftransform;
    public Image Hpbar;
    public bool IsAtk = false;
    public bool IsSkill = false;
    public bool IsEndSkill = false;
    public bool IsDown = false;
    public bool Skill1CD, Skill2CD,EndSkillCD;
    public int AtkCount,SkillCount;
    
}



public class AIFSM : MonoBehaviour
{

    // Start is called before the first frame update
    private IState currenState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>(); //建立一個字典，存入狀態，透過前面的key 找後面的value
    public Parameter parameter = new Parameter();
    void Start()
    {
        parameter.am = parameter.modle.GetComponent<Animator>();
        parameter.Target = GameObject.FindGameObjectWithTag("Player");
        states.Add(StateType.Idle, new IdleState(this)); //新贈一個狀態到字典裡
        states.Add(StateType.Walk, new WalkState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Down, new DownState(this));
        states.Add(StateType.Skill, new SkillState(this));
        SwitchState(StateType.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        currenState.OnUpdata();//執行當前狀態的OnUpdata  
    }

    public void SwitchState(StateType type)//切換狀態
    {
     if(currenState != null)
        {
            currenState.OnExit();//如果切換狀態時，還有前一個狀態則執行離開狀態的方法
        }
        currenState = states[type];//更改狀態時使用字典透過enum 找到相應的狀態
        Debug.Log(type);
        currenState.OnEnter();//執行新狀態的OnEnter方法
    }


    public void OnUpdateRM(object _deltaPos)
    {
        transform.position += (Vector3)_deltaPos;
    }

    public void BossRot()
    {
        Vector3 x = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 a = new Vector3(parameter.Target.transform.position.x, 0f, parameter.Target.transform.position.z);
        Quaternion targetRot = Quaternion.LookRotation(a - x);//取得玩家與怪物的位置(上面兩行把Y軸清除避免怪物往上看)
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 10F * Time.deltaTime);//怪物轉向玩家
    }
    public void SkillEnter()
    {
       print("OK");
        if( parameter.Skill1CD == false)
        {
         Instantiate(parameter.Shadow, parameter.ShadowPos[0].transform.position,parameter.Shadow.transform.rotation);
          Instantiate(parameter.Shadow, parameter.ShadowPos[1].transform.position, parameter.Shadow.transform.rotation);
          parameter.Skill1CD = true;
          Invoke("Skill1CD", 40);
        }
        else if( parameter.Skill2CD == false)
        {
        parameter.efftransform[0].LookAt(parameter.Target.transform);
        var x = Instantiate(parameter.eff, parameter.efftransform[0].transform.position, parameter.efftransform[0].transform.rotation);
        Destroy(x, 5);
        parameter.efftransform[1].LookAt(parameter.Target.transform);
        x = Instantiate(parameter.eff, parameter.efftransform[1].transform.position, parameter.efftransform[1].transform.rotation);
        Destroy(x, 5);
        parameter.efftransform[2].LookAt(parameter.Target.transform);
        x = Instantiate(parameter.eff, parameter.efftransform[2].transform.position, parameter.efftransform[2].transform.rotation);
        Destroy(x, 5);
            parameter.Skill2CD = true;
            Invoke("Skill2CD", 10);
        }
        parameter.IsSkill = true;
    }
    public void Skill1CD()
    {
      parameter.Skill1CD = false;
    }
    public void Skill2CD()
    {
        parameter.Skill2CD = false;
    }
    public void EndSkillCD()
    {
        parameter.EndSkillCD = false; 
    }
    public void ExitEND()
    {
        parameter.IsEndSkill = false;
    }

    public void ExitAtk()
    {
        parameter.IsAtk = false;
        Debug.Log(parameter.IsAtk);
        parameter.AtkCount += 1;
        if (parameter.AtkCount > 1) parameter.AtkCount = 0;
    }

    public void ExitSkill()
    {
        parameter.IsSkill = false;
    }

    private void OnTriggerEnter(Collider other)
    {
      if(other.tag == "weapon")
        {
            parameter.Hp -= 1;
            parameter.Hpbar.fillAmount = parameter.Hp / parameter.MaxHp;
            if (parameter.Hp <= 26 && parameter.IsDown == false )
            {
                SwitchState(StateType.Down);
                parameter.IsDown = true;
            }
        } 
    }


}
