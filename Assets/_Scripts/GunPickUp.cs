using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunPickUp : InteractableItem
{
    public AudioSource audioSource;
    public AudioClip pickUpClip;

    public UnityEvent pickUpAdditional;

    public override void OnUse()
    {
        base.OnUse();

        audioSource.clip = pickUpClip;
        audioSource.Play();


        ShipManager.instance.playerController.PickUpGun();

        pickUpAdditional.Invoke();

        gameObject.SetActive(false);
    }
}
