using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    public enum SpawnState {SPAWNING, WAITING, PLAYING}
    private SpawnState state = SpawnState.WAITING;
    [SerializeField] float round = 0, resourceLvl= 0, roundsLeft = 3;
    [SerializeField] private bool canBuildResource = true;
    private bool powerCoreActive = false;
    private bool builtInitialTower = false;
    [SerializeField] bool enoughWood = false, enoughFood = false, enoughOreAtlantium = false;

    [SerializeField] private TextMeshProUGUI roundNumberText, roundWordText;
    [SerializeField] private GameObject powerCore, roundDisplayButton, powerCoreButton;
    [SerializeField] private Button playButton;

    private void Awake() 
    {
        if(instance != null)
        {
            Debug.LogError("More than one StateManager in scene!");
            return;
        }
        instance = this;
    }
    
    private void Update()
    {
        // Ensures resources are build before a round can start
        if(canBuildResource)
        {
           playButton.interactable = false;
        }
        else
        {
            playButton.interactable = true;
        }
    }

    public void setState(SpawnState _state)
    {
        state = _state;
    }
    
    public SpawnState getState()
    {
        return state;
    }

    public void incrementRound()
    {
        round++;
        canBuildResource = true;
        roundDisplayButton.SetActive(false);
    }

    public void displayRoundNumber()
    {
        if(round == 33)
        {
            return;
        }

        if(!powerCoreActive)
        {
            roundNumberText.text = (round + 1).ToString();
        }
        else
        {
            roundsLeft -= 1;
            roundNumberText.text = roundsLeft.ToString();
        }

        roundDisplayButton.SetActive(true);
    }

    public float getRound()
    {
        return round;
    }

    public void setCanBuildTrue()
    {
        canBuildResource = true;
    }

    public void setCanBuildFalse()
    {
        canBuildResource = false;
    }

    public bool getCanBuild()
    {
        return canBuildResource;
    }

    // Handles the building of the powercore and initiates final game sequence
    public void setPowerCoreActive()
    {
        powerCoreActive = true;
        powerCore.SetActive(true);
        if(round < 30)
        {
            float tempWood = ResourceManager.instance.getWood();
            float tempOre = ResourceManager.instance.getOre();
            float tempFood = ResourceManager.instance.getFood();
            round = 30;
            ResourceManager.instance.resetWood(tempWood);
            ResourceManager.instance.resetOre(tempOre);
            ResourceManager.instance.resetFood(tempFood);
        }
        else if(round == 31)
        {
            roundsLeft = 2;
        }
        else if(round == 32)
        {
            roundsLeft = 1;
        }

        roundNumberText.text = roundsLeft.ToString();
        roundWordText.text = "Rounds Left";
    }

    public bool getPowerCoreIsActive()
    {
        return powerCoreActive;
    }

    public void setBuiltInitialTowerTrue()
    {
        builtInitialTower = true;
    }

    public bool getBuiltInitialTower()
    {
        return builtInitialTower;
    }

    public void incrementResourceLvl()
    {
        resourceLvl += 1;
    }

    public float getResourceLvl()
    {
        return resourceLvl;
    }

    public void EnoughWood()
    {
        enoughWood= true;
    }
    
    public bool getEnoughWood()
    {
        return enoughWood;
    }

    public void EnoughFood()
    {
        enoughFood= true;
    }
    
    public bool getEnoughFood()
    {
        return enoughFood;
    }

    public void enoughOreAndAtlantium()
    {
        enoughOreAtlantium = true;
    }

    public bool getEnoughOreAndAtlantium()
    {
        return enoughOreAtlantium;
    }
}
