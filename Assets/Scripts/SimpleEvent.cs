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

    // Used for preventing skip and complete events from both firing next event 
    private bool didFinish = false;

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
        // Set portrait 
        GameUI.instance.SetActivePortrait(chrID);

        // Wait for input 
        Game.OnNextEvent += OnNextEvent;

        didFinish = false; 

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

            // Wait a beat between clips so they don't play back to back 
            yield return new WaitForSeconds(.3f); 

            // Basic events should automatically resume when audio completes 
            OnNextEvent();
        }
        else 
        {
            Debug.LogWarning(chrID + " not found"); 
        }
    }

    private void OnNextEvent()
    {
        if (didFinish)
            return;

        didFinish = true;

        Game.OnNextEvent -= OnNextEvent;

        if (source)
            source.Stop();

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
