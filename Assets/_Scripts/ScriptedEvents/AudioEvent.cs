using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioEvent : ScriptedEvent
{
    public AudioSource audioSource;
    public AudioClip audioClip;


    public override void PlayEvent()
    {
        if (audioClip && audioSource)
        {
            audioSource.transform.SetParent(null);
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        base.PlayEvent();
    }
}
