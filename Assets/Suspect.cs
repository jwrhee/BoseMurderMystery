using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspect : MonoBehaviour
{
    public enum SuspectName { NONE, WATTS, DRAGUER, CAMMISH, AGATHA, BUTLER };
    public SuspectName suspectName;

    public AudioSource audio;

    public AudioClip clipSelectedForQuestioning;
    public AudioClip clipRollOverForQuestioning;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();   
    }
}
