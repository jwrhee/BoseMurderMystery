using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject player;

    public Suspect currectSuspect = null;
    public Suspect.SuspectName currentSuspectName;

    public Suspect butler;
    public Suspect[] suspects; // Array

    public Suspect Agatha;
    public Suspect Cammish;
    public Suspect Draguer;
    public Suspect Watts;

   

    public AudioClip clipSelectSuspect;
    //public AudioClip clipSelectSuspectVoice;
    public AudioClip clipStartQuestioning;

    public enum RoomState { INSTRUCTIONS, SELECTING ,QUESTIONING }
    public RoomState state = RoomState.INSTRUCTIONS;


    public AudioSource musicbox;

    public AudioClip bgmQuestioning;
    public AudioClip bgmSelecting;


    // Start is called before the first frame update
    void Start()
    {
        // Wait untill sysnced up
        StartInstructionSequence();
    }

    void SetBgm(AudioClip setClip)
    {
        musicbox.clip = setClip;
        musicbox.Play();
    }

    void StartInstructionSequence()
    {
        state = RoomState.INSTRUCTIONS;

        SetBgm(bgmSelecting);

        butler.audio.clip = clipStartQuestioning;
        butler.audio.Play();

        LeanTween.delayedCall(6f, () => { SetState(RoomState.SELECTING); });

    }

    void SetState(RoomState setState)
    {
        state = setState;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case RoomState.INSTRUCTIONS:



                break;

            case RoomState.SELECTING:

                int layerMask = 1 << 8;
                layerMask = ~layerMask;


                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(player.transform.position, player.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    Debug.Log("Did Hit");

                    //Destroy(hit.collider.gameObject);

                    // Play sound effect at that location.

                    currectSuspect = hit.collider.gameObject.GetComponent<Suspect>();

                    Debug.Log(currectSuspect);

                    if (currectSuspect)
                    {
                        if (currentSuspectName != currectSuspect.suspectName)
                        {


                            currentSuspectName = currectSuspect.suspectName;

                            currectSuspect.audio.clip = currectSuspect.clipRollOverForQuestioning;
                            currectSuspect.audio.volume = 1.0f;
                            currectSuspect.audio.Play();

                            currectSuspect.audio.PlayOneShot(clipSelectSuspect, 0.3f);

                            SilenceAllSuspectExpect(currectSuspect);
                        }
                    }
                }

                break;

            case RoomState.QUESTIONING:

                break;

            default:
                break;
        }



       
    }

    // Nod head while selected play a effect or enter the questioning phase
    void Confirm()
    {
        PlaySoundEffectOnSuspect(currectSuspect);
    }


    void PlaySoundEffectOnSuspect(Suspect suspect)
    {
        state = RoomState.QUESTIONING;

        SetBgm(bgmQuestioning);

        currectSuspect.audio.PlayOneShot(currectSuspect.clipSelectedForQuestioning);



        LeanTween.delayedCall(10f, () => { StartInstructionSequence(); });
    }

    void SilenceAllSuspectExpect( Suspect suspect)
    {
        foreach (Suspect sus in suspects)
        {
            if (sus.suspectName != suspect.suspectName)
            {
                // Turn off audio of others
                sus.audio.Stop();
            }
        }
    }


    public AudioSource GetSuspectAudioSource(string id)
    {

        AudioSource audio;

        switch (id)
        {
            case "Bosely":
                audio = butler.audio;
                break;

            case "Agatha":
                audio = butler.audio;
                break;

            case "Cammish":
                audio = butler.audio;
                break;

            case "Draguer":
                audio = butler.audio;
                break;

            case "Watts":
                audio = butler.audio;
                break;


            default:

                audio = butler.audio;
                break;
        }

        return audio;
    }

    private void OnEnable()
    {
        GestureControl.OnHeadNod += Confirm;
        GestureControl.OnDoubleTap += Confirm;
        //GestureControl.OnHeadShake += PlayResponceShake;
    }

    private void OnDisable()
    {
        GestureControl.OnHeadNod -= Confirm;
        GestureControl.OnDoubleTap -= Confirm;
        //GestureControl.OnHeadShake -= PlayResponceShake;
    }
}
