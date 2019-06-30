using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

/// <summary>
/// Simple event completes, then starts next event. 
/// </summary>
public class SimpleEvent : BaseEvent
{
    public GameObject nextEvent;

    public override void Play()
    {
        StartCoroutine(PlayCR());
    }
    IEnumerator PlayCR()
    {
        // Set UI 
        GameUI.instance.SetNextState();

        // Set on screen text 
        if (!string.IsNullOrEmpty(text))
        {
            GameUI.instance.text.text = text;
        }

        // Play clip and wait for it to complete
        // TODO: or player skip 
        var source = RoomController.instance.GetSuspectAudioSource(chrID); 
        if (source)
        {
            source.clip = clip;
            source.Play();
            while (source.isPlaying)
            {
                yield return null;
            }
        }
        else 
        {
            Debug.LogWarning(chrID + " not found"); 
        }

        // Wait for input 
        GameUI.OnNextEvent += OnNextEvent;
    }

    private void OnNextEvent()
    {
        GameUI.OnNextEvent -= OnNextEvent;

        // Play next event 
        nextEvent.GetComponent<BaseEvent>().Play();
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        if (nextEvent != null)
        {
            Gizmos.DrawLine(transform.position, nextEvent.transform.position);
            Handles.color = Color.green;
            labelStyle.normal.textColor = Color.green;
        }
        else 
        {
            Handles.color = Color.red;
            labelStyle.normal.textColor = Color.red;
        }

        base.OnDrawGizmos();
    }
#endif
}
