using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour
{
    #region public
    public float Cameraup;
    public float CameraRight;
    public float xspeed = 100.0f;
    public float yspeed = 80.0f;
    public Image lockdot,EnemyHpIcon;


    #endregion

    [Range(1, 10)]
    public float mouseXsp = 1.0f;
    [Range(1, 10)]
    public float mouseYsp = 1.0f;

    #region private
    [SerializeField] private GameObject PlayerHandle;
    [SerializeField] private GameObject CameraHandle;
    private float tempEulerx;
    private GameObject model;
    public GameObject cameras;
    private AIFSM enemystate;
    private float cameraspeed = 0.05f;
   
    [SerializeField]
    private LockTarget lockTarget;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        CameraHandle = transform.parent.gameObject;
        PlayerHandle = CameraHandle.transform.parent.gameObject;
        tempEulerx = 20;
        model = PlayerHandle.GetComponent<actorcontrol>().model;
        //cameras = Camera.main.gameObject;
        lockdot.enabled = false;
        EnemyHpIcon.enabled = false;
        Cursor.lockState = CursorLockMode.Locked; //將滑鼠隱藏鎖定在中央
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (lockTarget == null)
        {
            //取得滑鼠移動的路徑
            Cameraup = Input.GetAxis("Mouse Y") * 3.0f * mouseXsp;
            CameraRight = Input.GetAxis("Mouse X") * 2.5f * mouseYsp;
            //------------------------------------------------------

            Vector3 tempModelEuler = model.transform.eulerAngles;

            PlayerHandle.transform.Rotate(Vector3.up, CameraRight * xspeed * Time.fixedDeltaTime);
            tempEulerx -= Cameraup * yspeed * Time.fixedDeltaTime;
            tempEulerx = Mathf.Clamp(tempEulerx, -40, 30);
            CameraHandle.transform.localEulerAngles = new Vector3(tempEulerx, 0, 0);

            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            PlayerHandle.transform.forward = tempForward;
            CameraHandle.transform.LookAt(lockTarget.obj.transform);
        }

        cameras.transform.position = Vector3.Lerp(cameras.transform.position, transform.position, cameraspeed);
        cameras.transform.eulerAngles = transform.eulerAngles;
    }
    void Update()
    {
        if (lockTarget != null)
        {
            lockdot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
            EnemyHpIcon.fillAmount = enemystate.parameter.Hp / enemystate.parameter.MaxHp;
            if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10.0f)
            {
                lockTarget = null;
                lockdot.enabled = false;
                EnemyHpIcon.enabled = false;
            }
        }

    }
    public void LockUnLock()
    {
        //try to lock
        Vector3 modelorigin = cameras.transform.position;
        Vector3 modelorigin2 = modelorigin + new Vector3(0, 0, 0);
        Vector3 boxCenter = modelorigin2 + cameras.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(1f, 1f, 5f), cameras.transform.rotation, LayerMask.GetMask("Enemy"));

        if (cols.Length == 0)
        {
            lockTarget = null;
            lockdot.enabled = false;
            EnemyHpIcon.enabled = false;
        }
        else
        {
            foreach (var col in cols)
            {
                print(col.name);
                if (lockTarget != null && lockTarget.obj == col.gameObject)
                {
                    lockTarget = null;
                    lockdot.enabled = false;
                    EnemyHpIcon.enabled = false;
                }
                else
                {
                    lockTarget = new LockTarget(col.gameObject, col.bounds.extents.y);
                    enemystate = col.gameObject.GetComponent<AIFSM>();
                    lockdot.enabled = true;
                    EnemyHpIcon.enabled = true;                  
                    break;
                }

            }
        }
    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;
        public LockTarget(GameObject _obj, float _halfheight)
        {
            obj = _obj;
            halfHeight = _halfheight;
        }
    }


  
}
