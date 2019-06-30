using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public BaseEvent firstEvent; 

    public void Start() 
    {
        firstEvent.Play(); 
    }
}



