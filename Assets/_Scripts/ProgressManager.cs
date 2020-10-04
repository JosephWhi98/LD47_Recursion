using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance;
    public int previousSaveRoom;
    public List<int> lockedDoorRooms;

    bool reloadingScene;

    public bool lockPlayerMovement;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        lockedDoorRooms = new List<int>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        lockedDoorRooms.Add(0);
    }

    public void ExitToMenu()
    {
        previousSaveRoom = 0;
        lockedDoorRooms = new List<int>();
        lockPlayerMovement = false;

        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadScene()
    {
        if (!reloadingScene)
        {
            lockPlayerMovement = true;
            reloadingScene = true;
            StartCoroutine(ReloadingSceneRoutine());

        }
    }

    IEnumerator ReloadingSceneRoutine()
    {
        ShipManager.instance.uiManager.ScreenFade(Color.black, 1f);

        yield return new WaitForSeconds(1f);

        reloadingScene = false;
        lockPlayerMovement = false;
        SceneManager.LoadScene("SampleScene");
    }


}
