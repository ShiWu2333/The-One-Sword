using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;

    [SerializeField] GameObject pauseMenu;

    public void Start()
    {
        Time.timeScale = 1;
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        } 
    }

    public void PauseGame()
    { 
        Time.timeScale = 0;
        isPaused = true ;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false ;
        pauseMenu.SetActive(false);
    }

    public void ReturnToLevelPage()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
