using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor; 
#endif

public abstract class BaseEvent : MonoBehaviour
{
    // Optional speaker ID. Populated from second part of name (i.e. 0.Watts.1) 
    [System.NonSerialized]
    public string chrID;

    [Tooltip("Main clip that needs to end for event to complete")]
    public AudioClip clip;

    public abstract void Play();

    public List<AudioClip> bgClips;

    protected AudioSource source;

    [TextArea]
    public string text;

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Handles.Label(transform.position, name, labelStyle);

        Handles.SphereCap(0, transform.position, Quaternion.identity, 150f); 
    }
#endif

    protected static GUIStyle labelStyle = new GUIStyle(); 

    void Awake() 
    {
        var parts = name.Split('.'); 
        if (parts.Length > 1)
        {
            chrID = parts[1]; 
        }
        else 
        {
            chrID = null; 
        }
    }
}
