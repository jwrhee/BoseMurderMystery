using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR 
using UnityEditor; 
#endif

public class YesNoEvent : BaseEvent
{
    public GameObject yesEvent;
    public GameObject noEvent;

    public override void Play()
    {
        StartCoroutine(PlayCR());
    }
    IEnumerator PlayCR()
    {
        // Set UI 
        GameUI.instance.SetYesNoState();

        // Set on screen text 
        if (!string.IsNullOrEmpty(text))
        {
            GameUI.instance.text.text = text;
        }

        // Play clip and wait for it to complete
        // TODO: or player skip 
        var source = RoomController.instance.GetSuspectAudioSource(chrID);
        if (source && clip)
        {
            source.clip = clip;
            source.Play();
            while (source.isPlaying)
            {
                yield return null;
            }
        }

        // Wait for input 
        Game.OnYesNoEvent += OnYesNoEvent;
    }

    private void OnYesNoEvent(bool isYes)
    {
        Game.OnYesNoEvent -= OnYesNoEvent;

        if (isYes) 
        {
            yesEvent.GetComponent<BaseEvent>().Play(); 
        }
        else 
        {
            noEvent.GetComponent<BaseEvent>().Play(); 
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        bool isMissing = false; 

        if (yesEvent != null)
        {
            Gizmos.DrawLine(transform.position, yesEvent.transform.position);
        }
        else 
        {
            isMissing = true; 
        }

        if (noEvent != null) 
        {
            Gizmos.DrawLine(transform.position, noEvent.transform.position);
        }
        else 
        {
            isMissing = true; 
        }

        if (isMissing) 
        {
            Handles.color = Color.red;
            labelStyle.normal.textColor = Color.red;
        }
        else 
        {
            Handles.color = Color.green;
            labelStyle.normal.textColor = Color.green;
        }

        base.OnDrawGizmos(); 
    }
#endif
}
