﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor; 
#endif

public class CharacterSelectEvent : BaseEvent
{
    public delegate void CharacterSelectEventHandler(CharacterSelectEvent characterSelectEvent);
    public static event CharacterSelectEventHandler OnCharacterSelectBegan;

    public List<GameObject> chrEvents;

    [Tooltip("Plays when all chr events are completed")]
    public GameObject nextEvent; 

    public override void Play()
    {
        StartCoroutine(PlayCR());
    }
    IEnumerator PlayCR()
    {
        // If there are character events that haven't run, wait for a character selection 
        if (chrEvents.Count > 0) 
        {
            OnCharacterSelectBegan?.Invoke(this);

            if (GameUI.instance.GetBoseConnected()) // Deactivate portrait layer if using bose ar 
                GameUI.instance.portraitContainer.SetActive(false);

            // Set UI 
            GameUI.instance.SetCharacterSelectState(this);

            // Set on screen text 
            if (!string.IsNullOrEmpty(text))
            {
                GameUI.instance.text.text = text;
            }

            // Set portrait 
            GameUI.instance.SetActivePortrait(chrID);

            Game.OnCharacterSelectEvent += OnCharacterSelectEvent;

            // Play clip and wait for it to complete
            // TODO: or player skip 
            source = RoomController.instance.GetSuspectAudioSource(chrID);
            if (source && clip)
            {
                source.clip = clip;
                source.Play();
                while (source.isPlaying)
                {
                    yield return null;
                }
            }
        }
        // Otherwise just play the next event 
        else 
        {
            nextEvent.GetComponent<BaseEvent>().Play(); 
        }
    }

    private void OnCharacterSelectEvent(string selectedChrID)
    {
        Game.OnCharacterSelectEvent -= OnCharacterSelectEvent;

        GameUI.instance.portraitContainer.SetActive(true);

        if (source)
            source.Stop(); 

        BaseEvent eventToPlay = null; 

        foreach(var go in chrEvents) 
        {
            var chrEvent = go.GetComponent<BaseEvent>(); 
            if (chrEvent.chrID == selectedChrID) 
            {
                eventToPlay = chrEvent;
                break; 
            }
        }

        if (eventToPlay != null) 
        {
            eventToPlay.Play();
            chrEvents.Remove(eventToPlay.gameObject); 
        }
        else 
        {
            Debug.LogError("No event found for ID " + selectedChrID);
        }
    }

    public List<string> GetSelectableCharacterIDs()
    {
        var result = new List<string>(chrEvents.Count);
        foreach (var chrEventGO in chrEvents)
        {
            var chrEvent = chrEventGO.GetComponent<BaseEvent>();
            if (chrEvent.chrID != null)
            {
                result.Add(chrEvent.chrID);
            }
        }
        return result;
    }


#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        bool isMissing = false;

        for (int i = 0; i < chrEvents.Count; i++)
        {
            if (chrEvents[i] == null) 
            {
                isMissing = true; 
            }
            else 
            {
                Gizmos.DrawLine(transform.position, chrEvents[i].transform.position);
            }
        }

        if (nextEvent != null) 
        {
            Gizmos.DrawLine(transform.position, nextEvent.transform.position);
        }
        else 
        {
            isMissing = true; 
        }

        if (isMissing)
        {
            labelStyle.normal.textColor = Color.red; 
        }
        else
        {
            labelStyle.normal.textColor = Color.green;
        }

        base.OnDrawGizmos();
    }
#endif
}
