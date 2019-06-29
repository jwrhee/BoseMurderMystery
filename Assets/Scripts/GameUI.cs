using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameUI : MonoBehaviour
{
    public static GameUI instance; 

    public delegate void VoidAction();
    public delegate void BoolAction(bool value); 
    public delegate void StringAction(string value);

    public static event VoidAction OnNextEvent;
    public static event BoolAction OnYesNoEvent;
    public static event StringAction OnCharacterSelectEvent;

    public Text text;

    public GameObject nextMenu;

    public GameObject yesNoMenu;

    public GameObject characterSelectMenu; 

    void Awake() 
    {
        instance = this; 
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

    public void SetNextState() 
    {
        nextMenu.SetActive(true);
        yesNoMenu.SetActive(false);
        characterSelectMenu.SetActive(false);
    }

    public void SetYesNoState() 
    {
        nextMenu.SetActive(false);
        yesNoMenu.SetActive(true);
        characterSelectMenu.SetActive(false);
    }

    public void SetCharacterSelectState(CharacterSelectEvent chrSelectEvent) 
    {
        nextMenu.SetActive(false);
        yesNoMenu.SetActive(false);
        characterSelectMenu.SetActive(true);

        // Disable all character buttons 
        foreach(var button in chrButtons) 
        {
            button.SetActive(false); 
        }

        // Enable buttons that still have unplayed events 
        var chrEvents = chrSelectEvent.chrEvents; 
        foreach(var go in chrEvents) 
        {
            var chrEvent = go.GetComponent<BaseEvent>(); 
            var chrButton = GetCharacterButton(chrEvent.chrID); 
            if (chrButton != null) 
            {
                chrButton.SetActive(true); 
            }
        }
    }

    private GameObject GetCharacterButton(string chrID) 
    {
        foreach(var button in chrButtons) 
        {
            if (button.name == chrID) 
            {
                return button; 
            }
        }
        return null; 
    }

    public List<GameObject> chrButtons; 
}
