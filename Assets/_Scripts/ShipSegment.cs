using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

[System.Serializable]
public class EventRoomLayout
{
    public GameObject layout;
    public int triggerSegmentNumber;
    public ScriptedEvent scriptedEvent;
}


public class ShipSegment : MonoBehaviour
{
    public int currentSegmentNumber;

    public ShipSegment leftSegment; //++
    public ShipSegment rightSegment; //--

    public Door rightDoor;

    public TMP_Text nextSectorText;
    public TMP_Text currentSectorText;
    public TMP_Text previousSectorText;
    public Material sectorMat;
    public Light[] sectorLights;

    public GameObject[] roomLayouts;

    public EventRoomLayout[] eventRoomLayouts;


    private ScriptedEvent eventToTrigger;


    public void Start()
    {
        SetSegmentNumbers(currentSegmentNumber);
        SetSegmentColours();
    }

    public void Update()
    {
        if (ShipManager.instance.currentPlayerSegment != currentSegmentNumber && ShipManager.instance.currentPlayerSegment != leftSegment.currentSegmentNumber && ShipManager.instance.currentPlayerSegment != rightSegment.currentSegmentNumber)
        {
            foreach (Light light in sectorLights)
                light.enabled = false;
        }
        else
        {
            foreach (Light light in sectorLights)
                light.enabled = true;
        }
    }

    public void SetUpSegments()
    {
        ShipManager.instance.CheckForEvents(currentSegmentNumber);

        int leftSegmentNumb = currentSegmentNumber < 99 ? currentSegmentNumber + 1 : 0;
        int rightSegmentNumb = currentSegmentNumber > 0 ? currentSegmentNumber - 1 : 99;
        //Debug.Log(leftSegmentNumb + " " + rightSegmentNumb);

        leftSegment.SetSegmentNumbers(leftSegmentNumb);
        rightSegment.SetSegmentNumbers(rightSegmentNumb);

        if (eventToTrigger != null)
        {
            eventToTrigger.TryTriggerEvent();
            eventToTrigger = null;
        }

    }

    public void SetSegmentColours()
    {
        if (ShipManager.instance)
        {
            Color sectorCol = ShipManager.instance.GetColorForSector(currentSegmentNumber);

            sectorMat.SetColor("_EmissionColor", sectorCol);

            foreach (Light light in sectorLights)
                light.color = sectorCol;

            sectorCol.a = 0.5f;
            nextSectorText.color = sectorCol;
            currentSectorText.color = sectorCol;
            previousSectorText.color = sectorCol;

            int actualSector = currentSegmentNumber - (roomLayouts.Length * Mathf.FloorToInt(currentSegmentNumber / roomLayouts.Length));
            for (int i = 0; i < roomLayouts.Length; i++)
            {
                roomLayouts[i].SetActive(false);
            }

            foreach (EventRoomLayout layout in eventRoomLayouts)
                layout.layout.SetActive(false);

            EventRoomLayout eventLayout = eventRoomLayouts.FirstOrDefault(e => currentSegmentNumber == e.triggerSegmentNumber);

            if (eventLayout != null)
            {
                eventLayout.layout.SetActive(true);

                if (eventLayout.scriptedEvent != null)
                {
                    eventToTrigger = eventLayout.scriptedEvent;
                }
            }
            else
            {
                roomLayouts[actualSector].SetActive(true);
            }
        }
    }

    public void SetSegmentNumbers(int number)
    {
        rightDoor.isLocked = false;

        currentSegmentNumber = number;
        SetSegmentColours();

        int leftSegmentNumb = currentSegmentNumber < 99 ? currentSegmentNumber + 1 : 0;
        int rightSegmentNumb = currentSegmentNumber > 0 ? currentSegmentNumber - 1 : 99;
        nextSectorText.text = "Sector " + leftSegmentNumb + ">";
        previousSectorText.text = "<Sector " + rightSegmentNumb;
        currentSectorText.text = "Sector " + currentSegmentNumber;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ShipManager.instance.currentPlayerSegment = currentSegmentNumber;
            ShipManager.instance.currentRoom = this;

          
            SetUpSegments();
        }
    }

    public void LockRoomDoors(bool locked)
    {
        ShipManager.instance.currentRoom.rightDoor.Lock(locked);
       leftSegment.rightDoor.Lock(locked);
    }

    public void LockDoor()
    {
        ShipManager.instance.currentRoom.rightDoor.Lock(true);
        ProgressManager.instance.lockedDoorRooms.Add(ShipManager.instance.currentPlayerSegment);
    }

    public void DisableMonster()
    {
        ShipManager.instance.DisableMonster();
    }
}
