using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GestureControl : MonoBehaviour
{
   
    public static event Action OnDoubleTap;
    public static event Action OnHeadNod;
    public static event Action OnHeadShake;

    void Start()
    {
        
    }

    public void InputDoubleTap()
    {
        Debug.Log("Double Tap");
        if (OnDoubleTap != null)
        {
            OnDoubleTap();
        }
    }

    public void InputHeadNod()
    {
        Debug.Log("Head Nod");
        if (OnHeadNod != null)
        {
            OnHeadNod();
        }
    }

    public void InputHeadShake()
    {
        Debug.Log("Head Shake");
        if (OnHeadShake != null)
        {
            OnHeadShake();
        }
    }
}
