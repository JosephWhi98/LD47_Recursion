using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableItem
{
    [System.Serializable]
    public class SlidingDoor
    {
        public GameObject doorObject;
        public float closedPosition;
        public float openPosition;
    }

    public SlidingDoor[] doors;

    // Smoothly open a door
    public float openSpeed = 2.0f; //Increasing this value will make the door open faster

    bool open = false;
    public bool isLocked;

  
    public AudioSource audioSource;
    public AudioSource altAudioSource;

    public AudioClip openClip;
    public AudioClip closeClip;
    public AudioClip closeSlamClip;
    public AudioClip lockedClip;
    public AudioClip lockEventClip;

    public MeshRenderer[] lockedIndicator;
    public Material lockedMat;
    public Material unlockedMat;

    bool moving;

    Coroutine OpenCloseRoutine;

    public override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 10f && !open && !LeanTween.isTweening(doors[0].doorObject))
            InteractText.SetActive(true);
        else
            InteractText.SetActive(false);

        foreach (MeshRenderer rend in lockedIndicator)
            rend.sharedMaterial = isLocked ? lockedMat : unlockedMat;

    }

    public override void OnUse()
    {
        base.OnUse();

        if (OpenCloseRoutine != null)
            StopCoroutine(OpenCloseRoutine);

        OpenCloseRoutine = StartCoroutine(OpenClose());
    }

    public void Lock(bool locked)
    {
        isLocked = locked;

        if (locked)
        {
            altAudioSource.clip = lockEventClip;
            altAudioSource.Play();

            if (open)
            {
                if (OpenCloseRoutine != null)
                    StopCoroutine(OpenCloseRoutine);

                OpenCloseRoutine = StartCoroutine(OpenClose());
            }
        }
    }

    IEnumerator OpenClose()
    {
        moving = true; 
        if (!isLocked || open)
        {
            if (!open)
            {
                if (audioSource)
                {
                    audioSource.clip = openClip;
                    audioSource.Play();
                }

                foreach (SlidingDoor door in doors)
                {
                    LeanTween.cancel(door.doorObject );
                    LeanTween.moveLocalX(door.doorObject, door.openPosition, openSpeed).setEase(LeanTweenType.easeOutSine);
                }


                open = !open;
                yield return new WaitForSeconds(2f);
                OpenCloseRoutine = StartCoroutine(OpenClose());
            }
            else
            {
                if (audioSource)
                {
                    audioSource.clip = isLocked == false ?  closeClip : closeSlamClip;
                    audioSource.Play();
                }
                    

                foreach (SlidingDoor door in doors)
                {
                    LeanTween.cancel(door.doorObject);

                    LeanTween.moveLocalX(door.doorObject, door.closedPosition, isLocked == false ? openSpeed : openSpeed/2).setEase(LeanTweenType.easeOutSine);
                }


                open = !open;
            }
        }
        else
        {
            if (audioSource)
            {
                audioSource.clip = lockedClip;
                audioSource.Play();
            }
        }

        moving = false; 
    }
    
  
}
