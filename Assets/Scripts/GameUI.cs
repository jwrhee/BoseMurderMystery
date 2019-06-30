using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public List<GameObject> chrButtons;

    public List<CharacterUI> portraits;

    public Text text;

    public GameObject nextMenu;

    public GameObject yesNoMenu;

    public GameObject characterSelectMenu; 

    void Awake() 
    {
        instance = this; 
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

    public void SetActivePortrait(string chrID) 
    {
        foreach(var portrait in portraits) 
        {
            if (portrait.name == chrID) 
            {
                portrait.gameObject.SetActive(true); 
            }
            else 
            {
                portrait.gameObject.SetActive(false); 
            }
        }
    }
}
