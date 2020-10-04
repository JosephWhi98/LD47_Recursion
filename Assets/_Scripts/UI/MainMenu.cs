using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UIManager uiManager;

    bool loading = false;

    public void QuitGame()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }

    public void PlayGame()
    {
        if(!loading)
            StartCoroutine(PlayGameRoutine());
    }


    public IEnumerator PlayGameRoutine()
    {
        uiManager.ScreenFade(Color.black, 1.5f);
        yield return new WaitForSeconds(1.5f);


        SceneManager.LoadScene("SampleScene");
    }
}
