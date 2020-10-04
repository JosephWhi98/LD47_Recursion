using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipManager : MonoBehaviour
{

    public static ShipManager instance;

    public UIManager uiManager; 

    public int currentPlayerSegment;
    public ShipSegment currentRoom;

    public Color[] colourOptions;

    public TextEvent[] textEvents;

    public TextEvent finalTextEvent;
    public TextEvent finalTextEvent2;

    public ScriptedEvent finalAudioEvent;

    public List<string> PlayedEvents;

    public List<int> RoomsToSaveProgress;

    public Transform playerTransform;
    public PlayerController playerController;

    public MonsterAI activeMonster;

    public ShipSegment startSegment;
   

    public bool monsterKilled;

    bool endingPlayed;

    public void Awake()
    {
        if (!instance)
            instance = this;

        if(ProgressManager.instance)
            startSegment.currentSegmentNumber = ProgressManager.instance.previousSaveRoom;
    }

    public void Update()
    {
        if (ProgressManager.instance.lockedDoorRooms.Contains(currentPlayerSegment) && currentRoom)
        {
            currentRoom.rightDoor.isLocked = true;
        }


        if (RoomsToSaveProgress.Contains(currentPlayerSegment) && currentPlayerSegment > ProgressManager.instance.previousSaveRoom)
        {
            ProgressManager.instance.previousSaveRoom = currentPlayerSegment; 
        }

        if (currentPlayerSegment == 90 && !monsterKilled)
        {
            currentRoom.leftSegment.rightDoor.Lock(true);
        }
        else if(currentPlayerSegment == 90)
        {
            currentRoom.leftSegment.rightDoor.Lock(false);
        }


        if (monsterKilled && currentPlayerSegment > textEvents[19].segmentToTrigger)
            activeMonster.gameObject.SetActive(false);


        if (currentPlayerSegment == 0 && monsterKilled && !endingPlayed)
        {
            currentRoom.leftSegment.rightDoor.Lock(true);

            uiManager.TriggerTextEvent(finalTextEvent);
            endingPlayed = true;

            StartCoroutine(FinalSequence());
        }
    }

    public void CheckForEvents(int roomNumber)
    {
        TextEvent textEvent = textEvents.FirstOrDefault(e => e.segmentToTrigger == roomNumber);

        if (textEvent != null)
        {
            uiManager.TriggerTextEvent(textEvent);
        }
    }

    public void AddPlayedEvent(string uniqueEventName)
    {
        PlayedEvents.Add(uniqueEventName);
    }

    public bool HasEventBeenPlayed(string uniqueEventName)
    {
        return PlayedEvents.Contains(uniqueEventName);
    }

    public Color GetColorForSector(int sector)
    {
        int actualSector = sector - (11 * Mathf.FloorToInt(sector / 11));

        return colourOptions[actualSector];
    }

    public void SetMonster(MonsterAI monster)
    {
        this.activeMonster = monster;
    }

    public void DisableMonster()
    {
        if(activeMonster)
            activeMonster.gameObject.SetActive(false);
    }

    public IEnumerator FinalSequence()
    {
        yield return new WaitForSeconds(2f);

        ProgressManager.instance.lockPlayerMovement = true; 


        finalAudioEvent.TryTriggerEvent();

        yield return new WaitForSeconds(3f);

        uiManager.ScreenFade(Color.black, 3f);

        yield return new WaitForSeconds(1f);

        uiManager.TriggerTextEvent(finalTextEvent2);

        yield return new WaitForSeconds(5.5f);

        uiManager.EndTitle(true);

        yield return new WaitForSeconds(4f);

        uiManager.EndTitle(false);

        yield return new WaitForSeconds(3f);



        ProgressManager.instance.ExitToMenu();
    }
}
