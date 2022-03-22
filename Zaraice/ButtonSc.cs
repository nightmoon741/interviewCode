using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSc
{
    public bool IsPressing = false;
    public bool OnPressed =false;
    public bool OnReleased =false;
    public bool IsExtending = false;
    public bool isDelaying = false;

    public float extendingDuration = 0.15f;
    public float delayingDuration = 0.2f;

    private bool curState = false;
    private bool lastState = false;

    private TimerSc extTimer = new TimerSc();
    private TimerSc delayTimer = new TimerSc();

    public void Tick(bool input)
    {
        extTimer.Tick();
        delayTimer.Tick();

        curState = input;
        IsPressing = curState;

        OnPressed = false;
        OnReleased = false;
        IsExtending = false;
        isDelaying = false;

        if (curState != lastState)
        {
            if(curState == true)
            {
                OnPressed = true;
                startTime(delayTimer, delayingDuration);
            }
            else
            {
                OnReleased = true;
                startTime(extTimer, extendingDuration);
            }
        }

        lastState = curState;
        IsExtending = extTimer.state == TimerSc.State.run;
        isDelaying = delayTimer.state == TimerSc.State.run;
    }
    private void startTime(TimerSc timer, float duration)
    {
        timer.duration = duration;
        timer.Go();
    }

}

