using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the function of the pause menu
public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu, gameOverviewMenu, objMenu, roundMenu, tower1Menu, tower2Menu, 
    resource1Menu, resource2Menu, playerMenu, winLossMenu, settingsMenu;
    [SerializeField] AudioSource pauseSound;
    private bool isPaused = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            if(isPaused)
            {
                pauseGame();
            }
            else
            {
                resumeGame();
            }

            pauseSound.Play();
        }
    }

    public void togglePause()
    {
        if(isPaused)
        {
            resumeGame();
            return;
        }

        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void pauseGame()
    {       
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        settingsMenu.SetActive(false);
        winLossMenu.SetActive(false);
        playerMenu.SetActive(false);
        resource2Menu.SetActive(false);
        resource1Menu.SetActive(false);
        tower2Menu.SetActive(false);
        tower1Menu.SetActive(false);
        roundMenu.SetActive(false);
        objMenu.SetActive(false);
        gameOverviewMenu.SetActive(false);
        pauseMenu.SetActive(false);
        isPaused = false;
    }
}
