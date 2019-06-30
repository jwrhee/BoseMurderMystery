using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public delegate void VoidAction();
    public delegate void BoolAction(bool value);
    public delegate void StringAction(string value);

    public static event VoidAction OnNextEvent;
    public static event BoolAction OnYesNoEvent;
    public static event StringAction OnCharacterSelectEvent;

    public BaseEvent firstEvent; 

    public void Start() 
    {
        firstEvent.Play(); 
    }

    public void OnYesNo(bool isYes)
    {
        OnYesNoEvent?.Invoke(isYes);
    }

    public void OnNext()
    {
        OnNextEvent?.Invoke();
    }

    public void OnCharacterSelect(string chrID)
    {
        OnCharacterSelectEvent?.Invoke(chrID);
    }

}



