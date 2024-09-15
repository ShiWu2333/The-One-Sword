using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;

    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject deadUI;

    public void Start()
    {
        playerController.OnPlayerDie += PlayerController_OnPlayerDie;
        Time.timeScale = 1;
        isPaused = false;
        pauseMenu.SetActive(false);
        deadUI.SetActive(false);
    }

    private void PlayerController_OnPlayerDie(object sender, System.EventArgs e)
    {
        Debug.Log("Player is dead");
        deadUI.SetActive(true);
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

    public void GameOver()
    {
        deadUI.SetActive(true);
    }

    public void ReturnToLevelPage()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
