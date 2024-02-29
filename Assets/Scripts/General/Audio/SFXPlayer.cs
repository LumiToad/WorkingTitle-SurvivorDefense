using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum PlayMode 
{ 
    SINGLE,
    SHUFFLE,
    IN_ORDER
}

public class SFXPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private int inOrderTrackIndex = 0;

    [Header("Set a trigger key. This is being used to attach the SFX to a function")]
    [Header("Use CAPS LOCK and underscore!")]
    [SerializeField]
    private string triggerKey;

    [Header("Set the size of the playlist, then place SFX individually")]
    [SerializeField]
    private AudioClip[] audioClips;

    [Header("SINGLE just plays first SFX")]
    [Header("SHUFFLE will pick one randomly")]
    [Header("IN_ORDER will play the next SFX, each time")]
    [SerializeField]
    private PlayMode playMode;

    [Header("Will randomize the pitch if checked.")]
    [SerializeField]
    private bool randomizePitch;

    [SerializeField, Range(-0.2f, 0.0f)]
    private float minPitch;

    [SerializeField, Range(0.0f, 3.0f)]
    private float maxPitch;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOnTriggerKey(string key)
    {
        if (triggerKey != key)
        {
            return;
        }

        switch (playMode)
        {
            case (PlayMode.SINGLE):
                audioSource.clip = audioClips[0];
                break;
            case (PlayMode.SHUFFLE):
                audioSource.clip = PickRandomAudioClip();
                break;
            case (PlayMode.IN_ORDER):
                audioSource.clip = audioClips[inOrderTrackIndex];
                inOrderTrackIndex++;
                
                if (inOrderTrackIndex >= audioClips.Length)
                {
                    inOrderTrackIndex = 0;
                }
                break;
        }        

        if (randomizePitch)
        {
            audioSource.pitch = RandomizePitch();
        }
        PlayAudio();
    }

    private AudioClip PickRandomAudioClip()
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        return audioClips[randomIndex];
    }

    private float RandomizePitch()
    {
        float randomPitchValue = Random.Range(minPitch, maxPitch);
        return randomPitchValue;
    }

    private void PlayAudio()
    {
        audioSource.Play();
    }
}
