using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject player;

    public Suspect currectSuspect = null;
    public Suspect.SuspectName currentSuspectName;

    public Suspect butler;
    public Suspect[] suspects;

    public AudioClip clipSelectSuspect;
    //public AudioClip clipSelectSuspectVoice;
    public AudioClip clipStartQuestioning;

    public enum RoomState { INSTRUCTIONS, SELECTING ,QUESTIONING }
    public RoomState state = RoomState.INSTRUCTIONS;
     

    // Start is called before the first frame update
    void Start()
    {
        // Wait untill sysnced up

        butler.audio.clip = clipStartQuestioning;
        butler.audio.Play();

        LeanTween.delayedCall(6f, () => { SetState(RoomState.QUESTIONING); });
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

            case RoomState.QUESTIONING:

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

                            currectSuspect.audio.PlayOneShot(clipSelectSuspect, 0.5f);

                            SilenceAllSuspectExpect(currectSuspect);
                        }
                    }
                }

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
        currectSuspect.audio.PlayOneShot(currectSuspect.clipSelectedForQuestioning);
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
