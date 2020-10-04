using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class TextEvent
{
    public int segmentToTrigger;
    public bool triggered;
    public string textToSay;
    public float timeToHide = 5f;
}


public class UIManager : MonoBehaviour
{
    public TMP_Text displayText;
    public float timeToHideText = 0;

    public Image screenFader;
    public Coroutine fadeRoutine;

    public TMP_Text titleEnd;
    public AudioSource endSource;
    public AudioClip endClip; 

    private void Awake()
    {
        screenFader.color = Color.black;

        ScreenFade(Color.clear, 1f);
    }

    public void Update()
    {
        if (displayText != null && Time.time >= timeToHideText)
        {
            displayText.text = "";
        }
    }

    public void TriggerTextEvent(TextEvent textEvent)
    {
        if (!textEvent.triggered)
        {
            timeToHideText = Time.time + textEvent.timeToHide;

            displayText.text = textEvent.textToSay;
            textEvent.triggered = true;
        }
    }

    public void ScreenFade(Color colour, float time)
    {
        if (time > 0)
        {
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);

            fadeRoutine = StartCoroutine(FadeScreen(colour, time));
        }
        else
        {
            screenFader.color = colour; 
        }
    }

    public void EndTitle( bool on)
    {
        if (on)
        {
            endSource.clip = endClip;
            titleEnd.transform.gameObject.SetActive(true);
            endSource.Play();
        }
        else
        {
            StartCoroutine(FadeTitle(Color.clear, 2f));
        }
    }

    public IEnumerator FadeTitle(Color colour, float time)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = 0.02f / time; //The amount of change to apply.
        Color currentColor = titleEnd.color;
        Color startColor = titleEnd.color;
        while (progress < 1)
        {
            currentColor = Color.Lerp(startColor, colour, progress);
            progress += increment;
            titleEnd.color = currentColor;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public IEnumerator FadeScreen(Color colour, float time)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = 0.02f / time; //The amount of change to apply.
        Color currentColor = screenFader.color;
        Color startColor = screenFader.color;
        while (progress < 1)
        {
            currentColor = Color.Lerp(startColor,  colour, progress);
            progress += increment;
            screenFader.color = currentColor;
            yield return new WaitForSeconds(0.02f);
        }
    }
    
}
