using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{
    //功能 : 平面範圍偵測 概念導向
    public GameObject Aimodel;

    public GameObject Player;
    public Transform px;
    public float rotspeed = 1.0f;
    public CapsuleCollider col;
    private Animator AIanim;
    private Rigidbody Airigid;
    private Vector3 thrustVec;

    #region Serialize Field
    [SerializeField]
    private int Angle = 5;
    [SerializeField]
    private float Range = 5;
    [SerializeField]
    private int StartAngle = -50;
    #endregion

    public float hpMax = 15.0f;
    public float hp = 15.0f;

    #region Readonly
    private readonly int Count = 20;
    #endregion

    #region Awake
    void Awake()
    {
        col = GetComponent<CapsuleCollider>();
        AIanim =Aimodel.GetComponent<Animator>();
        Airigid = GetComponent<Rigidbody>();
    }
    #endregion



    #region Unity Method
    void Update()
    {
        getRotation();
    }
    #endregion

    #region Function
 
    private void getRotation()
    {
        if (hp > 0)
        {
            Quaternion rot = transform.rotation;

            for (int i = 0; i < Count + 1; i++)
            {
                Quaternion q = Quaternion.Euler(rot.x, rot.y + StartAngle + (Angle * i), rot.z);
                Vector3 newVec = q * transform.forward * Range;
                Debug.DrawRay(transform.position + new Vector3(0, col.bounds.extents.y, 0), newVec, Color.red); //畫出射線        

                RaycastHit hit;//儲存射線照到的物件

                if (Physics.Raycast(transform.position + new Vector3(0, col.bounds.extents.y, 0), newVec, out hit, Range))//發射射線
                {
                    if (hit.collider.tag == "Player")//偵測射線是否打到玩家
                    {
                        Debug.Log(hit);
                        Player = GameObject.FindGameObjectWithTag("Player");//如果射線照到Tag為Player 獲取玩家組件
                        px = Player.transform;
                        //  transform.LookAt(Player.transform);
                        Quaternion targetRot = Quaternion.LookRotation(px.position - transform.position);//取得玩家與怪物的位置
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotspeed * Time.deltaTime);//怪物轉向玩家
                       // transform.rotation = Quaternion.Euler(0f,transform.rotation.y, transform.rotation.z);//將怪物X轉向清空 避免怪物往上轉
                        float offset = Vector3.Distance(px.position, transform.position);//取得AI與角色的距離

                        if (offset > 5.0f)
                        {
                            AIanim.SetTrigger("move");
                            transform.Translate(Vector3.forward * Time.deltaTime);
                        }
                        if (offset < 5.0f)
                        {
                            AIanim.SetTrigger("attack");
                        }

                    }
                }
            }
        }
    }
    #endregion
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "weapon")
        {
         AIanim.SetTrigger("hit");
         hp -= 5;
         hp = Mathf.Clamp(hp, 0, hpMax);
         float x = col.GetComponent<WeaponAttributes>().upforce;
         Airigid.velocity = new Vector3(transform.position.x, x, transform.position.z);         
            if (hp <= 0)
            {
                AIanim.SetTrigger("die");
            }
        }
    }
}
