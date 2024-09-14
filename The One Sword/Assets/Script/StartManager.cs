using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{

    [SerializeField] GameObject mainPage;
    [SerializeField] GameObject levelPage;

    private void Awake()
    {
        SwitchMainPage();
    }

    public void SwitchLevelPage()
    {
        mainPage.SetActive(false);
        levelPage.SetActive(true);
    }

    public void SwitchMainPage()
    {
        mainPage.SetActive(true);
        levelPage.SetActive(false);
    }

    public void SwitchSceneLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void SwitchSceneLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void SwitchSceneLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void SwitchSceneLevel4()
    {
        SceneManager.LoadScene("Level 4");
    }

}
