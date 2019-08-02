using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GestureControl : MonoBehaviour
{
   
    public static event Action OnDoubleTap;
    public static event Action OnHeadNod;
    public static event Action OnHeadShake;

    public float DelayAfterGesture = 2.0f;
    private float GestureTimer;
    private bool GestureInputted;

    void Start()
    {
        GestureTimer = DelayAfterGesture;
    }

    private void FixedUpdate()
    {
        if (GestureInputted && GestureTimer > 0.0f)
        {
            GestureTimer -= Time.fixedDeltaTime;

        } else
        {
            GestureTimer = DelayAfterGesture;
            GestureInputted = false;
        }
    }

    public void InputDoubleTap()
    {
        //Debug.Log("Double Tap");
        if (OnDoubleTap != null && !GestureInputted)
        {
            OnDoubleTap();
            GestureInputted = true;
        }
    }

    public void InputHeadNod()
    {
       // Debug.Log("Head Nod");
        if (OnHeadNod != null && !GestureInputted)
        {
            OnHeadNod();
            GestureInputted = true;
        }
    }

    public void InputHeadShake()
    {
        //Debug.Log("Head Shake");
        if (OnHeadShake != null && !GestureInputted)
        {
            OnHeadShake();
            GestureInputted = true;
        }
    }
}
