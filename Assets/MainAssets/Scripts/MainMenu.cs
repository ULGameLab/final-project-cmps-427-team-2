using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject TitleScreen;
    public CanvasGroup TitleGroup;
    private bool showUI;
    public int fadeSpeed = 1;
    private bool showInfo;
    public GameObject InfoScreen;
    public CanvasGroup InfoGroup;
    public GameObject credits;

    // Start is called before the first frame update
    void Start()
    {
        TitleGroup.alpha = 0;
        InfoScreen.SetActive(false);
        credits.SetActive(false);
        InfoGroup.alpha = 0;
        showUI = false;
        showInfo = false;
        fadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        //fade stuff for main menu
        if (showUI)
        {
            if (!TitleScreen.activeInHierarchy)
            {
                TitleScreen.SetActive(true);
            }
            if (TitleGroup.alpha < 1)
            {
                TitleGroup.alpha += Time.deltaTime * fadeSpeed;
            }
            
        }
        else
        {
            if (TitleGroup.alpha > 0)
            {
                TitleGroup.alpha -= Time.deltaTime * fadeSpeed;
            }
            if (TitleGroup.alpha == 0 && TitleScreen.activeInHierarchy)
            {
                TitleScreen.SetActive(false);
            }
        }

        //fade stuff for info
        if (showInfo)
        {
            if(!InfoScreen.activeInHierarchy)
            {
                InfoScreen.SetActive(true);
            }
            if (InfoGroup.alpha < 1)
            {
                InfoGroup.alpha += Time.deltaTime * fadeSpeed;
            }
        }
        else
        {
            if (InfoGroup.alpha > 0)
            {
                InfoGroup.alpha -= Time.deltaTime * fadeSpeed;
            }
            if (InfoGroup.alpha == 0 && InfoScreen.activeInHierarchy)
            {
                InfoScreen.SetActive(false);
            }
        }


    }

    public void fadeOut()
    {
        showUI = false;
    }

    public void fadeIn()
    {
        showUI = true;
    }

    public void fadeOutInfo()
    {
        showInfo = false;
    }

    public void fadeInInfo()
    {
        showInfo = true;
    }

    public void OnClickPlay()
    {
        fadeOut();
        StartCoroutine(Play());

    }

    public void OnClickQuit()
    {
        fadeOut();
        StartCoroutine(Quit());
    }

    public void OnClickGameInfo()
    {
        StartCoroutine(OpenInfo());
    }

    public void OnClickCredits()
    {
        StartCoroutine(OpenCredits());
    }
    public void OnClickBackCredits()
    {
        credits.SetActive(false);
        fadeIn();
    }

    public void OnClickInfoBack()
    {
        StartCoroutine(CloseInfo());
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(2);
        Application.Quit();
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }

    IEnumerator OpenInfo()
    {
        fadeOut();
        yield return new WaitForSeconds(1.5f);
        fadeInInfo();
    }

    IEnumerator CloseInfo()
    {
        fadeOutInfo();
        yield return new WaitForSeconds(1.5f);
        fadeIn();
    }

    IEnumerator OpenCredits()
    {
        fadeOut();
        yield return new WaitForSeconds(1.5f);
        credits.SetActive(true);
    }

}
