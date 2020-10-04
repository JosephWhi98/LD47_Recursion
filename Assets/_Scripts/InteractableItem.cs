using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public Transform player;
    public GameObject InteractText;

    public virtual void Start()
    {
        player = ShipManager.instance.playerTransform;
    }

    public virtual void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 10f)
            InteractText.SetActive(true);
        else
            InteractText.SetActive(false);
    }

    public virtual void OnUse()
    { }
}
