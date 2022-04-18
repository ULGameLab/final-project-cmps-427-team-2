using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public GameObject BlackBox;
    public CanvasGroup BlackGroup;
    public GameObject EndScene;
    public CanvasGroup EndGroup;

    public TextMeshProUGUI EndHeader;
    public TextMeshProUGUI EndDes;

    public Material skyDay;
    public Material skyNight;
    public Light light;
    bool dayTime;
    bool blackness;

    // Start is called before the first frame update
    void Start()
    {
        TitleGroup.alpha = 0;
        InfoScreen.SetActive(false);
        credits.SetActive(false);
        EndScene.SetActive(false);
        InfoGroup.alpha = 0;
        BlackGroup.alpha = 0;
        showUI = false;
        showInfo = false;
        blackness = false;
        setDay();
        fadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        //fade stuff for main menu
        if (showUI)
        {
            if (!TitleScreen.activeInHierarchy && dayTime)
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
            if(!InfoScreen.activeInHierarchy && dayTime)
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

        //fade stuff for black box
        if (blackness)
        {
            if(!BlackBox.activeInHierarchy)
            {
                BlackBox.SetActive(true);
            }
            if (BlackGroup.alpha<1)
            {
                BlackGroup.alpha += Time.deltaTime * fadeSpeed;
            }
        }
        else
        {
            if (BlackGroup.alpha > 0)
            {
                BlackGroup.alpha -= Time.deltaTime * fadeSpeed;
            }
            if (BlackGroup.alpha == 0 && BlackBox.activeInHierarchy)
            {
                BlackBox.SetActive(false);
            }
        }


    }

    public void setDay()
    {
        EndScene.SetActive(false);
        dayTime = true;
        RenderSettings.skybox = skyDay;
        light.intensity = 1;
        fadeIn();
    }

    public void setNight()
    {
        BlackBox.SetActive(true);
        BlackGroup.alpha = 0;

        dayTime = false;
        RenderSettings.skybox = skyNight;
        light.intensity = 0.075f;

        TitleScreen.SetActive(false);
        credits.SetActive(false);
        InfoScreen.SetActive(false);
        EndScene.SetActive(true);

    }

    public void clickOnReturnMain()
    {
        StartCoroutine(NightToDaySmooth());
    }

    public void Win()
    {
        setNight();
        EndHeader.SetText("You have won!");
    }

    public void Lose()
    {
        setNight();
        EndHeader.SetText("You have died!");
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

    IEnumerator NightToDaySmooth()
    {
        blackness = true;
        yield return new WaitForSeconds(1.5f);
        setDay();
        blackness = false;
    }

}
