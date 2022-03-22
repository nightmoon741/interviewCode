using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private AIFSM manager;
    private Parameter parameter;
     
    public IdleState(AIFSM manager)
    {
        this.manager = manager; //獲取到狀態機對象
        this.parameter = manager.parameter; //從狀態機對象獲取屬性
    }
     
    public void OnEnter()
    {
        parameter.am.SetTrigger("Idle");
    }

    public void OnUpdata()
    {
        if (parameter.Hp <= 25 && parameter.EndSkillCD == false)
        {
            manager.Invoke("EndSkillCD", 200);
            parameter.am.SetTrigger("EndSkill");
            parameter.IsEndSkill = true;
            parameter.EndSkillCD = true;
        }
        else if (parameter.IsEndSkill == false)
        {

            float offset = Vector3.Distance(manager.transform.position, parameter.Target.transform.position);

            if (offset <= 10)
            {
                manager.SwitchState(StateType.Walk);
            }
        }
    }
    public void OnExit()
    {
        parameter.IsDown = false;
    }

}
//------------------------------------------------------------------------------------------------------------------------


public class WalkState : IState
{
    private AIFSM manager;
    private Parameter parameter;

    public WalkState(AIFSM manager)
    {
        this.manager = manager; //獲取到狀態機對象
        this.parameter = manager.parameter; //從狀態機對象獲取屬性
    }

    public void OnEnter()
    {
        parameter.am.SetTrigger("Walk");
        parameter.IsAtk = false;
    }

    public void OnUpdata()
    {
        manager.BossRot();
        float offset = Vector3.Distance(manager.transform.position, parameter.Target.transform.position); ;//取得AI與角色的距離
        manager.transform.Translate(Vector3.forward *3F* Time.deltaTime);
        if (offset <= 5)
        {
            manager.SwitchState(StateType.Attack);
        }
    }
    public void OnExit()
    {

    }

}
//------------------------------------------------------------------------------------------------------------------------
public class AttackState : IState
{
    private AIFSM manager;
    private Parameter parameter;
    private float timer;
    public AttackState(AIFSM manager)
    {
        this.manager = manager; //獲取到狀態機對象
        this.parameter = manager.parameter; //從狀態機對象獲取屬性
    }

    public void OnEnter()
    {
        if (parameter.AtkCount == 0) parameter.am.SetTrigger("Atk");
        if (parameter.AtkCount == 1) parameter.am.SetTrigger("Atk2");
        parameter.IsAtk = true;
    }

    public void OnUpdata()
    {
        manager.BossRot();
        float offset = Vector3.Distance(manager.transform.position, parameter.Target.transform.position); ;//取得AI與角色的距離
        timer += Time.deltaTime;
        if (timer >= 2.5f)
        {
            if (offset > 5 && parameter.IsAtk == false)
            {
                manager.SwitchState(StateType.Walk);
            }else if (parameter.IsAtk == false && parameter.IsSkill == false)
            {
                if (parameter.AtkCount == 0) parameter.am.SetTrigger("Atk");
                if (parameter.AtkCount == 1) parameter.am.SetTrigger("Atk2");
                parameter.IsAtk = true;       
            }         
            timer = 0f;
        }
        if (parameter.IsAtk == false)
        {
            if (parameter.Skill1CD == false || parameter.Skill2CD == false) manager.SwitchState(StateType.Skill);
        }

        Debug.Log(parameter.IsAtk);
        //manager.transform.position += parameter.am.deltaPosition;
    }
    public void OnExit()
    {
        timer = 0f;
        parameter.IsAtk = false;
    }

}
//------------------------------------------------------------------------------------------------------------------------

public class DownState : IState
{
    private AIFSM manager;
    private Parameter parameter;
    private float timer;

    public DownState(AIFSM manager)
    {
        this.manager = manager; //獲取到狀態機對象
        this.parameter = manager.parameter; //從狀態機對象獲取屬性
    }

    public void OnEnter()
    {
        parameter.am.SetTrigger("Down");
    }

    public void OnUpdata()
    {
        timer += Time.deltaTime;
        if(timer >= 10f)
        {
            parameter.am.SetTrigger("Idle");
            manager.SwitchState(StateType.Idle);
        }
    }
    public void OnExit()
    {
        timer = 0;
    }

}


public class SkillState : IState
{
    private AIFSM manager;
    private Parameter parameter;
    private float timer;

    public SkillState(AIFSM manager)
    {
        this.manager = manager; //獲取到狀態機對象
        this.parameter = manager.parameter; //從狀態機對象獲取屬性
    }

    public void OnEnter()
    {
        parameter.am.SetTrigger("Skill");
        manager.SkillEnter();
    }

    public void OnUpdata()
    {
        if (parameter.IsSkill == false)
        {
         if (parameter.Skill1CD == false || parameter.Skill2CD == false)
        {
            manager.SkillEnter();
        }
         float offset = Vector3.Distance(manager.transform.position, parameter.Target.transform.position); ;//取得AI與角色的距離
            if (offset > 5) manager.SwitchState(StateType.Walk);
            if(offset < 5) manager.SwitchState(StateType.Attack);
        }
     
    }
    public void OnExit()
    {
        parameter.IsSkill = false;
    }
}


