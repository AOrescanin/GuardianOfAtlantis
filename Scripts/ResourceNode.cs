using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceNode : MonoBehaviour
{
    private StateManager stateManager;
    private ResourceManager resourceManager;
    private GameObject resourceToBuild; 

    // Resource Generator Properties
    [SerializeField] private bool isWoodMill, isFarm, isMine, isForge;
    [SerializeField] private GameObject woodMill, farm;
    private float woodProduction = 3f, foodProduction = 6f, forgeCost = 1f;
    [SerializeField] private GameObject [] mines;
    [SerializeField] TextMeshProUGUI mineLvlText;
    private int mineLvl = 0, resourceLvl = 0, maxLvl = 6;
    private bool isBuilt = false, canCollect = false, check = false;
    private float roundBuilt = -1;

    // Visuals and SFX
    [SerializeField] private GameObject HintBG, hintText, tutorialText;
    [SerializeField] Animator anim;
    [SerializeField] AudioClip buildSound, hoverSound;
    private AudioClip currentClip;
    private AudioSource source;

    
    private void Start() 
    {
        if(isWoodMill)
        {
            resourceToBuild = woodMill;
        }
        else if(isFarm)
        {
            resourceToBuild = farm;
        }

        stateManager = StateManager.instance;
        resourceManager = ResourceManager.instance;
        
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    // Handles mouse hovering a resource node
    private void OnMouseEnter() 
    {
        if(stateManager.getState() != StateManager.SpawnState.WAITING || !stateManager.getBuiltInitialTower() || Time.timeScale == 0) {return;}

        HintBG.SetActive(true);
        hintText.SetActive(true);
        
        if(isBuilt || (!stateManager.getCanBuild()) || (isForge && resourceManager.getOre() < 1)) {return;}
        
        anim.SetBool("isShining", false);
        currentClip = hoverSound;
        source.clip = currentClip;
        source.Play();
    }

    // Handles mouse clicking a resource node depending on type
    private void OnMouseDown() 
    {
        if(stateManager.getState() != StateManager.SpawnState.WAITING || !stateManager.getBuiltInitialTower() || isBuilt || (!stateManager.getCanBuild())  || (isForge && resourceManager.getOre() < 1) || Time.timeScale == 0) {return;}

        if(isMine)
        {
            mines[mineLvl].SetActive(true);
            mineLvl++;
            mineLvlText.text = mineLvl.ToString();
            stateManager.incrementResourceLvl();
            
            if(mineLvl >= maxLvl)
            {
                isBuilt = true;
            }
        }
        else if(isForge)
        {
            if(resourceManager.getOre() >= 0 && roundBuilt != stateManager.getRound())
            {
                float oreAmount = resourceManager.getOre();
                resourceManager.setOre(-oreAmount);
                resourceManager.setAtlantium(oreAmount);
                roundBuilt = stateManager.getRound();
            }
            else
            {
                return;
            }
        }
        else
        {
            resourceToBuild.SetActive(true);
            isBuilt = true;
            canCollect = true;
            roundBuilt = stateManager.getRound();
            stateManager.incrementResourceLvl();
        }

        stateManager.setCanBuildFalse();
        currentClip = buildSound;
        source.clip = currentClip;
        source.Play();
    }

    // Handles mouse exiting a resource node
    private void OnMouseExit() 
    {
        anim.SetBool("isShining", true);
        hintText.SetActive(false);
 
        HintBG.SetActive(false);
    }

    private void Update() 
    {
        // Toggles animations on and off between rounds
        if(stateManager.getState() != StateManager.SpawnState.WAITING || !stateManager.getBuiltInitialTower() || isBuilt || (!stateManager.getCanBuild()) || (isForge && resourceManager.getOre() < 1)) 
        {
            anim.SetBool("isStopped", true);
        }
        else
        {
            anim.SetBool("isStopped", false);
        }

        // Deals with resource generation issue when the power core is first build
        if(stateManager.getPowerCoreIsActive() && !check)
        {
            roundBuilt = stateManager.getRound();
            check = true;
        }

        if(isWoodMill)
        {
            collectWood();
        }
        else if(isFarm)
        {
            collectFood();
        }

        if(stateManager.getResourceLvl() == 18 && resourceManager.getOre() == 0)
        {
            stateManager.setCanBuildFalse();
        }
    }

    private void collectWood()
    {
        if(stateManager.getState() == StateManager.SpawnState.WAITING && isBuilt && canCollect && roundBuilt != stateManager.getRound())
        {
            resourceManager.setWood(woodProduction);
            canCollect = false;
            roundBuilt = stateManager.getRound();
        }

        if(roundBuilt < stateManager.getRound())
        {
            canCollect = true;
        }
    }

    private void collectFood()
    {
        if(stateManager.getState() == StateManager.SpawnState.WAITING && isBuilt && canCollect && roundBuilt != stateManager.getRound() && stateManager.getRound() % 3 == 0)
        {
            resourceManager.setFood(foodProduction);
            canCollect = false;
            roundBuilt = stateManager.getRound();
        }

        if(roundBuilt < stateManager.getRound())
        {
            canCollect = true;
        }
    }
}
