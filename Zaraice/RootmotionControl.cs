﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootmotionControl : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();  
    }

    void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRM", (object)anim.deltaPosition);
    }
}