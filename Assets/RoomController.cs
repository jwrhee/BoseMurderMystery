using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public Game game;

    public GameObject player;

    public Suspect currectSuspect = null;
    public string curSuspectString = null;

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

    public static RoomController instance;

    public AudioSource musicbox;

    public AudioClip bgmSelecting;
    public AudioClip bgmQuestioning;

    void Awake() 
    {
        instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        CharacterSelectEvent.OnCharacterSelectBegan += StartCharacterSelecting;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = RoomState.INSTRUCTIONS;
        // Wait untill sysnced up

        //StartInstructionSequence();
    }

    void StartCharacterSelecting(CharacterSelectEvent e)
    {
        List<string> charOptions = e.GetSelectableCharacterIDs();

        // Turn of all suspect then turn on the one we need
        Agatha.gameObject.GetComponent<Collider>().enabled = false;
        Cammish.gameObject.GetComponent<Collider>().enabled = false;
        Draguer.gameObject.GetComponent<Collider>().enabled = false;
        Watts.gameObject.GetComponent<Collider>().enabled = false;

        foreach (string charName in charOptions)
        {
            switch (charName)
            {
                case "Agatha":
                     
                    Agatha.gameObject.GetComponent<Collider>().enabled = true;
                    curSuspectString = charName;
                    break;

                case "Draguer":
                    Draguer.gameObject.GetComponent<Collider>().enabled = true;
                    curSuspectString = charName;
                    break;

                case "Watts":
                    Watts.gameObject.GetComponent<Collider>().enabled = true;
                    curSuspectString = charName;
                    break;

                case "Cammish":
                    Cammish.gameObject.GetComponent<Collider>().enabled = true;
                    curSuspectString = charName;
                    break;

                default:
                    break;
            }


          

        }



        SetState(RoomState.SELECTING);

        PlayBgm(bgmSelecting);
    }

    void StartInstructionSequence()
    {
        state = RoomState.INSTRUCTIONS;


        LeanTween.delayedCall(6f, () => { SetState(RoomState.SELECTING); });

    }

    void PlayBgm( AudioClip clip)
    {

        musicbox.clip = clip;
        musicbox.Play();
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

                            currectSuspect.audio.PlayOneShot(clipSelectSuspect, 0.5f);

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
     
        if (state == RoomState.SELECTING)
        {
            //PlaySoundEffectOnSuspect(currectSuspect);

            PlayBgm(bgmQuestioning);

            if (game)
            {
                game.OnCharacterSelect(curSuspectString);
            }

            state = RoomState.QUESTIONING;
        }

       
    }




    void PlaySoundEffectOnSuspect(Suspect suspect)
    {
        state = RoomState.QUESTIONING;

        currectSuspect.audio.PlayOneShot(currectSuspect.clipSelectedForQuestioning);



     //   LeanTween.delayedCall(10f, () => { StartInstructionSequence(); });
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
