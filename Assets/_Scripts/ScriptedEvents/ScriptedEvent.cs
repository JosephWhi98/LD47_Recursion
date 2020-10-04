using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptedEvent : MonoBehaviour
{
    public bool playOnce;
    public string uniqueEventName;
    public UnityEvent additionalEvent; 

    public virtual void TryTriggerEvent()
    {
        bool hasBeenPlayed = ShipManager.instance.HasEventBeenPlayed(uniqueEventName);

        if (!hasBeenPlayed || (hasBeenPlayed && !playOnce))
        {
            PlayEvent();
        }
    }

    public virtual void PlayEvent()
    {
        additionalEvent.Invoke();
        ShipManager.instance.AddPlayedEvent(uniqueEventName);
    }
}
