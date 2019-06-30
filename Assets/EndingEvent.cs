using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor; 
#endif

public class EndingEvent : BaseEvent
{
    public override void Play()
    {
        StartCoroutine(PlayCR());
    }
    IEnumerator PlayCR()
    {
        // Set UI 
        GameUI.instance.SetNextState();

        // Set portrait 
        GameUI.instance.SetActivePortrait(chrID);

        // Wait for input 
        Game.OnNextEvent += OnNextEvent;

        yield break; 
    }

    private void OnNextEvent()
    {
        Game.OnNextEvent -= OnNextEvent;

        if (source)
            source.Stop();

        // Restart game 
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene"); 
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        Handles.color = Color.green;
        labelStyle.normal.textColor = Color.green;

        base.OnDrawGizmos();
    }
#endif
}
