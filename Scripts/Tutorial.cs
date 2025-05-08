using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles that hints that pop up throughout the game based on the round
public class Tutorial : MonoBehaviour
{
    StateManager stateManager;
    ResourceManager resourceManager;
    private float startingWoodAmount, currentAtlAmount;
    private bool gotAtlAmount = false, started = false;

    [SerializeField] GameObject lvl1Tutorial, lvl1BuildTower, lvl1BuildResource, lvl1Play, playButton, lvl2Tutorial, 
    lvl3Tutorial, lvl3BuildReosurce, lvl3UpgradeTower, lvl4Tutorial, lvl4BuildResource, lvl4ArmoredEnemies, 
    lvl5Tutorial, lvl6Tutorial, lvl10Tutorial, lvl13Tutorial, lvl16Tutorial, lvl19Tutorial, lvl22Tutorial, 
    lvl25Tutorial, lvl28Tutorial, lvl31Tutorial, lvl33FinalWave, WinLossUI, ConstructPrompt, WinPrompt, LostPromt;
    [SerializeField] SceneTransition sceneTransition;

    void Start()
    {
        stateManager = StateManager.instance;
        resourceManager = ResourceManager.instance;

        startingWoodAmount = resourceManager.getWood();
    }   

    void Update()
    {        
        if(stateManager.getRound() == 0)
        {
            if(resourceManager.getWood() < startingWoodAmount)
            {
                lvl1BuildTower.SetActive(false);
                lvl1BuildResource.SetActive(true);
                stateManager.setBuiltInitialTowerTrue();

                if(!stateManager.getCanBuild() && stateManager.getBuiltInitialTower())
                {
                    lvl1BuildResource.SetActive(false);
                    lvl1Play.SetActive(true);
                    playButton.SetActive(true);
                }
            }
        }
        else if(stateManager.getRound() == 1)
        {
            lvl1Tutorial.SetActive(false);

            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                if(!stateManager.getCanBuild())
                {
                    lvl2Tutorial.SetActive(false);
                    return;
                }

                lvl2Tutorial.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 2 && stateManager.getState() == StateManager.SpawnState.WAITING)
        {
            lvl3Tutorial.SetActive(true);

            if(!stateManager.getCanBuild())
            {
                lvl3BuildReosurce.SetActive(false);
                lvl3UpgradeTower.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 3)
        {
            lvl3Tutorial.SetActive(false);

            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl4Tutorial.SetActive(true);

                if(!stateManager.getCanBuild())
                {
                    lvl4BuildResource.SetActive(false);
                    lvl4ArmoredEnemies.SetActive(true);
                }
            }
        }
        else if(stateManager.getRound() == 4)
        {
            lvl4Tutorial.SetActive(false);

            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                if(!gotAtlAmount)
                {
                    currentAtlAmount = resourceManager.getAtlantium();
                    gotAtlAmount = true;
                    lvl5Tutorial.SetActive(true);
                }

                if(resourceManager.getAtlantium() > currentAtlAmount)
                {
                    lvl5Tutorial.SetActive(false);
                }
            }
        
        }
        else if(stateManager.getRound() == 5)
        {
            lvl5Tutorial.SetActive(false);

            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl6Tutorial.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 6)
        {
            lvl6Tutorial.SetActive(false);
        }
        else if(stateManager.getRound() == 9)
        {
            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl10Tutorial.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 10)
        {
            lvl10Tutorial.SetActive(false);
        }
        else if(stateManager.getRound() == 12)
        {
            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl13Tutorial.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 13)
        {
            lvl13Tutorial.SetActive(false);
        }
         else if(stateManager.getRound() == 15)
        {
            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl16Tutorial.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 16)
        {
            lvl16Tutorial.SetActive(false);
        }
         else if(stateManager.getRound() == 18)
        {
            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl19Tutorial.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 19)
        {
            lvl19Tutorial.SetActive(false);
        }
        else if(stateManager.getRound() == 21)
        {
            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl22Tutorial.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 22)
        {
            lvl22Tutorial.SetActive(false);
        }
         else if(stateManager.getRound() == 24)
        {
            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl25Tutorial.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 25)
        {
            lvl25Tutorial.SetActive(false);
        }
         else if(stateManager.getRound() == 27)
        {
            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl28Tutorial.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 28)
        {
            lvl28Tutorial.SetActive(false);
        }
         else if(stateManager.getRound() == 30)
        {
            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl31Tutorial.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 31)
        {
            lvl31Tutorial.SetActive(false);
        }
         else if(stateManager.getRound() == 32)
        {
            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                lvl33FinalWave.SetActive(true);
            }
        }
        else if(stateManager.getRound() == 33)
        {
            lvl33FinalWave.SetActive(false);

            if(stateManager.getState() == StateManager.SpawnState.WAITING)
            {
                WinLossUI.SetActive(true);

                if(!started)
                {
                    started = true;
                    StartCoroutine(WinLoss());
                }

                if(stateManager.getPowerCoreIsActive())
                {
                    sceneTransition.LoadScreen(3);
                }
            }
        }      
    }

    // Handles will loss
    IEnumerator WinLoss()
    {
        yield return new WaitForSeconds(1f);
        if(!stateManager.getPowerCoreIsActive())
        {
            if(stateManager.getEnoughFood() && stateManager.getEnoughWood() && stateManager.getEnoughOreAndAtlantium())
            {
                ConstructPrompt.SetActive(true);
            }
            else
            {
                LostPromt.SetActive(true);
                sceneTransition.LoadScreen(2);      
            }
        }
        else
        {
            ConstructPrompt.SetActive(false);
            WinPrompt.SetActive(true);
            sceneTransition.LoadScreen(3);
        }
    }
}
