using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("===== key settings ======")]
    public string keyUp;
    public string keyDown;
    public string keyLeft;
    public string keyRight;

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;
    public string keyE;
    public string keyF;
    public string keyG;

    public ButtonSc buttonA = new ButtonSc();
    public ButtonSc buttonB = new ButtonSc();
    public ButtonSc buttonC = new ButtonSc();
    public ButtonSc buttonD = new ButtonSc();
    public ButtonSc buttonE = new ButtonSc();
    public ButtonSc buttonF = new ButtonSc();
    public ButtonSc buttonG = new ButtonSc();

    [Header("===== output signals ======")]
    public float Signalup;
    public float Signalright;
    public float dmag;
    public Vector3 dvec;

    public bool run,step;
    public bool jump;   
    public bool attack;  
    public bool lockon;
    public bool skill1, skill2, skill3;

    [Header("===== others ======")]

    public bool inputenabled = true;

    private float targetSignalup;
    private float targetSignalright;
    private float velocitySignalup;
    private float velocitySignalright;
    

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

        buttonA.Tick(Input.GetKey(keyA));
        buttonB.Tick(Input.GetKey(keyB));
        buttonC.Tick(Input.GetKey(keyC));
       //(buttonA.IsExtending && buttonA.OnPressed); 判斷是否在時間內連續輸入
        buttonD.Tick(Input.GetKey(keyD));
        buttonE.Tick(Input.GetKey(keyE));
        buttonF.Tick(Input.GetKey(keyF));
        buttonG.Tick(Input.GetKey(keyG));

        #region MoveInput
        targetSignalup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetSignalright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if (inputenabled == false)
        {
            targetSignalup = 0;
            targetSignalright = 0;
        }

        Signalup = Mathf.SmoothDamp(Signalup, targetSignalup, ref velocitySignalup, 0.1f);
        Signalright = Mathf.SmoothDamp(Signalright, targetSignalright, ref velocitySignalright, 0.1f);

        Vector2 tempDAxis = squareToCircle(new Vector2(Signalright, Signalup));
        float Dright = tempDAxis.x;
        float Dup = tempDAxis.y;

        dmag = Mathf.Sqrt((Dup * Dup) + (Dright * Dright));//更改anim裡面forward的浮點值
        dvec = Dright * transform.right + Dup * transform.forward;

        #endregion

        //run = Input.GetKey(keyA);
        step = (buttonA.IsExtending && buttonA.OnPressed);
        run = (buttonA.IsPressing && !buttonA.isDelaying || buttonA.IsExtending);
        jump = buttonB.OnPressed;
        attack = buttonC.OnPressed;
        lockon = buttonD.OnPressed;
        skill1 = buttonE.OnPressed;
        skill2 = buttonF.OnPressed;
        skill3 = buttonG.OnPressed;
        //print(lockon);



        //    bool newjump = Input.GetKey(keyB);
        //     if (newjump != lastJump && newjump == true)
        //     {
        //         jump = true;
        //     }
        //    else
        //    {
        //        jump = false;
        //    }
        //    lastJump = newjump;

        //--------------------------------------------
        //攻擊狀態
        //  bool newattack = Input.GetKey(keyC);
        //  if (newattack != lastattack && newattack == true)
        //  {
        //      attack = true;
        //  }
        //  else
        //  {
        //      attack = false;
        //  }
        //  lastattack = newattack;


    }
    private Vector2 squareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }

}