using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public List<GameObject> chrButtons;

    public GameObject portraitContainer; 
    public List<CharacterUI> portraits;

    public Text text;

    public GameObject nextMenu;

    public GameObject yesNoMenu;

    public GameObject characterSelectMenu;

    private bool isBoseConnected;

    void Awake() 
    {
        instance = this;
        Bose.Wearable.WearableControl.Instance.ConnectionStatusChanged += OnBoseConnectStatusChange;
    }

    public bool GetBoseConnected()
    {
        return isBoseConnected;
    }

    public void DisableMenu()
    {
        nextMenu.SetActive(false);
        yesNoMenu.SetActive(false);
        characterSelectMenu.SetActive(false);
    }

    public void SetNextState() 
    {
        if (!isBoseConnected)
        {
            nextMenu.SetActive(true);
            yesNoMenu.SetActive(false);
            characterSelectMenu.SetActive(false);
        } else
        {
            DisableMenu();
        }
        
    }

    public void SetYesNoState() 
    {
        if (!isBoseConnected)
        {
            nextMenu.SetActive(false);
            yesNoMenu.SetActive(true);
            characterSelectMenu.SetActive(false);
        } else
        {
            DisableMenu();
        }
        
    }

    public void SetCharacterSelectState(CharacterSelectEvent chrSelectEvent) 
    {
        if (!isBoseConnected)
        {
            nextMenu.SetActive(false);
            yesNoMenu.SetActive(false);
            characterSelectMenu.SetActive(true);

            // Disable all character buttons 
            foreach (var button in chrButtons)
            {
                button.SetActive(false);
            }

            // Enable buttons that still have unplayed events 
            var chrEvents = chrSelectEvent.chrEvents;
            foreach (var go in chrEvents)
            {
                var chrEvent = go.GetComponent<BaseEvent>();
                var chrButton = GetCharacterButton(chrEvent.chrID);
                if (chrButton != null)
                {
                    chrButton.SetActive(true);
                }
            }
        } else
        {
            DisableMenu();
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

    public void OnBoseConnectStatusChange(Bose.Wearable.ConnectionStatus status, Bose.Wearable.Device? device)
    {
        isBoseConnected = (status == Bose.Wearable.ConnectionStatus.Connected);
        Debug.Log("bose glasses status " + isBoseConnected);
    }
}