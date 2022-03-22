using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actorcontrol : MonoBehaviour
{
    #region Public
    public GameObject model;
    public PlayerInput pi;
    public CameraControl camcon;
    //public CinemachinCameraControl camcon;

   
    public float movespeed = 2.0f;
    public float runspeed = 2.0f;
    public float jumpVelocity = 4.0f;
    #endregion

    [Space(10)]
    [Header("===== friction setting =====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    #region private
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 movevec;//儲存跟玩家相關移動控制
    private Vector3 thrustVec;
    private Vector3 deltaPos;
    private CapsuleCollider col;
    private bool canAttackair = false;
    private bool skill1reset,skill2reset,skill3reset;
    #endregion

    private bool lockPlanar = false;
    private bool trackDirection = false;

    // Start is called before the first frame update
    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }


    // Update is called once per frame
    void Update()
    {
        if (pi.lockon)
        {
            camcon.LockUnLock();
          
        }

        if(camcon.lockdot.enabled == false)
        {
            anim.SetFloat("forward", pi.dmag * Mathf.Lerp(anim.GetFloat("forward"), ((pi.run) ? 2.0f : 1.0f), 0.5f)); //更改anim裡面forward的浮點值
            anim.SetFloat("right", 0);           
        }
        else
        {
            Vector3 locaDvec = transform.InverseTransformVector(pi.dvec);
            anim.SetFloat("forward", locaDvec.z * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", locaDvec.x * ((pi.run) ? 2.0f : 1.0f));
            
        }
        // print(pi.Signalup);

        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttackair = true;
        }

        if (pi.step)
        {
            anim.SetTrigger("step");
        }

        #region Normal Attack
        if (pi.attack && (CheckState("ground") || CheckStateTag("attack") ) && canAttackair == false)
        {
            anim.SetTrigger("attack1");
           
        }

        if(pi.attack  && (!CheckState("ground")) && !CheckState("fall") && canAttackair == true)
        {
            anim.SetTrigger("airattack");       
        }
        #endregion

        #region skill1
        if (pi.skill1 && canAttackair == false && !CheckState("skill1") )
        {
            anim.SetTrigger("skill1");         
        }

        if (pi.skill1 && canAttackair == true && !CheckState("fall") && skill1reset == false)
        {
            skill1reset = true;
            anim.SetTrigger("airskill1");
        }
        #endregion

        #region skill2
        if (pi.skill2 && CheckState("skill2-1"))
        {
            anim.SetTrigger("skill2-2");           
            skill2reset = true;
            canAttackair = true;
        }
        else if(pi.skill2 && skill2reset == false)
        {
            anim.SetTrigger("skill2-1");
            SoundManager.instance.skill2p(0);
        }
        #endregion

        #region skill3
        if (pi.skill3 && canAttackair == false)
        {
            anim.SetTrigger("skill3");
        }

        if (pi.skill3 && canAttackair == true && !CheckState("fall") && skill3reset == false)
        {
            anim.SetTrigger("airskill3");
            skill3reset = true;
        }
        #endregion

        #region lockon
        if (camcon.lockdot.enabled == false)
        {
            if (pi.dmag > 0.1f)
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.dvec, 0.2f);
            }
            if (lockPlanar == false)
            {
                movevec = pi.dmag * model.transform.forward * movespeed * ((pi.run) ? runspeed : 1.0f);
            }
        }else  {
            if(trackDirection == false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.dvec, 0.2f);             
            } 
            if(lockPlanar == false)
            {
             movevec = pi.dvec * movespeed * ((pi.run) ? runspeed : 1.0f);
            }
            
        }
        #endregion


    }
    void FixedUpdate()
    {
        rigid.position += deltaPos;
        rigid.velocity = new Vector3(movevec.x, rigid.velocity.y, movevec.z) + thrustVec;      
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
  
    }

    #region checkstate
    private bool CheckState(string stateName , string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }
    private bool CheckStateTag(string TagName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsTag(TagName);
    }
    #endregion



    //mesage processing block
    #region Ground
    public void IsGround()
    {
        anim.SetBool("IsGround", true);
        Debug.Log("IsGround");

    }

    public void IsNotGround()
    {
        anim.SetBool("IsGround", false);

    }

    public void OnGroundEnter()
    {
        pi.inputenabled = true;
        lockPlanar = false;
        canAttackair = false;
        col.material = frictionOne;
        trackDirection = false;
        skill1reset = false;
        skill2reset = false;
        skill3reset = false;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        model.SendMessage("weaponDistable");
    }
    public void OnGroundExit()
    {
        col.material = frictionZero;
    }
    #endregion

    public void OnJumpEnter()
    {
        //pi.inputenabled = false;
        //lockPlanar = true;
        trackDirection = true;
        thrustVec = new Vector3(0, jumpVelocity,0);
        
    }


    public void OnStepEnter()
    {
        pi.inputenabled = false;
        //lockPlanar = true;
        //Vector3 stepspeed = 50 * model.transform.forward;
        //thrustVec += stepspeed;
        //thrustVec.y = 0;
        rigid.MovePosition(model.transform.position+ model.transform.forward * 120 * Time.fixedDeltaTime);
    }



    public void Onattack1Enter()
    {
        pi.inputenabled = false;
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), 1.0f);
    }
    public void Onattack1Update()
    {
        //thrustVec = model.transform.forward * anim.GetFloat("attack1Velocity");
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), lerpTarget, 0.03f));
    }
    public void OnAttackExit()
    {
        model.SendMessage("weaponDistable");

    }
    public void OnHitEnter()
    {
     pi.inputenabled = false;
     movevec = Vector3.zero;
    }

    public void OnSkill1Enter()
    {
        pi.inputenabled = false;
        canAttackair = false;
        movevec = Vector3.zero;
    }

    public void OnSkill2Enter()
    {
        pi.inputenabled = false;
    }

    public void OnSkill2UP()
    {
        rigid.useGravity = true;
        thrustVec = new Vector3(0, 6.0f, 0);
    }

    public void Exitairattack()
    {
        canAttackair = false;
        rigid.useGravity = true;     
    }
    public void Onairattack()
    {
        pi.inputenabled = false;
        canAttackair = true;
        rigid.useGravity = false;
        movevec = Vector3.zero;
        rigid.velocity = Vector3.zero; 
        lockPlanar = true;
    }




    public void OnUpdateRM(object _deltaPos)
    {
        //print((Vector3)_deltaPos);
        deltaPos +=(Vector3) _deltaPos;
    }

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }


}