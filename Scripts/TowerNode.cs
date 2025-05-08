using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerNode : MonoBehaviour
{
    private BuildManager buildManager;
    private StateManager stateManager;
    private ResourceManager resourceManager;

    [SerializeField] GameObject radius9, radius12, radius15;
    private bool isBuilt, canBuild;
    private GameObject tower;
    [SerializeField] ParticleSystem buildParticle;
    [SerializeField] private AudioClip buildSound, hoverSound;
    private AudioClip currentClip;
    private AudioSource source;

    private void Start() 
    {
        buildManager = BuildManager.instance;
        stateManager = StateManager.instance;
        resourceManager = ResourceManager.instance;

        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(tower == null && isBuilt)
        {
            isBuilt = false;
        }
    }

    private void OnMouseEnter() 
    {
        if(stateManager.getState() != StateManager.SpawnState.WAITING || buildManager.GetTowerToBuild() == null || isBuilt || Time.timeScale == 0) {return;}

        hover();
    }

    // Handles building each type of tower
    private void OnMouseDown() 
    {
        if(stateManager.getState() != StateManager.SpawnState.WAITING || buildManager.GetTowerToBuild() == null || tower != null || Time.timeScale == 0) {return;}

        GameObject towerToBuild = buildManager.GetTowerToBuild();

        if(buildManager.GetTowerToBuild() == buildManager.getArcherPrefab())
        {
            canBuild = canBuyArcherTower(true);
        }
        else if(buildManager.GetTowerToBuild() == buildManager.getCannonPrefab())
        {
            canBuild = canBuyCannon(true);
        }
        else if(buildManager.GetTowerToBuild() == buildManager.getLaserPrefab())
        {
            canBuild = canBuyEnergyBeamer(true);
        }
        else if(buildManager.GetTowerToBuild() == buildManager.getTeslaPrefab())
        {
            canBuild = canBuyTeslaCoil(true);
        }
        else if(buildManager.GetTowerToBuild() == buildManager.getBarracksPrefab())
        {
            canBuild = canBuyBarracks(true);
        }

        if(canBuild)
        {
            buildParticle.Play();
            tower = (GameObject)Instantiate(towerToBuild, transform.parent.position, transform.rotation);
            tower.transform.parent = transform;
            isBuilt = true;

            source.clip = buildSound;
            source.Play();
        }
    }

    private void OnMouseExit() 
    {
        radius9.SetActive(false);
        radius12.SetActive(false);
        radius15.SetActive(false);
    }

    private void hover()
    {
        if(buildManager.GetTowerToBuild() == buildManager.getArcherPrefab() && canBuyArcherTower(false))
        {
            radius12.SetActive(true);
        }
        else if(buildManager.GetTowerToBuild() == buildManager.getCannonPrefab() && canBuyCannon(false))
        {
            radius15.SetActive(true);
        }
        else if(buildManager.GetTowerToBuild() == buildManager.getLaserPrefab() && canBuyEnergyBeamer(false))
        {
            radius12.SetActive(true);
        }
        else if(buildManager.GetTowerToBuild() == buildManager.getTeslaPrefab() && canBuyTeslaCoil(false))
        {
            radius15.SetActive(true);
        }
        else if(buildManager.GetTowerToBuild() == buildManager.getBarracksPrefab() && canBuyBarracks(false))
        {
            radius9.SetActive(true);
        }

        source.clip = hoverSound;
        source.Play();
    }

    private bool canBuyArcherTower(bool isBuying)
    {
        if(resourceManager.getWood() < 3f)
        {
            return false;
        }

        if(isBuying)
        {
            resourceManager.setWood(-3f);
        }

        return true;
    }

    private bool canBuyCannon(bool isBuying)
    {
        if(resourceManager.getWood() < 4f || resourceManager.getAtlantium() < 1f)
        {
            return false;
        }

        if(isBuying)   
        { 
            resourceManager.setWood(-4f);
            resourceManager.setAtlantium(-1f);
        }
        
        return true;
    }

    private bool canBuyEnergyBeamer(bool isBuying)
    {
        if(resourceManager.getWood() < 2f || resourceManager.getAtlantium() < 1f || resourceManager.getFood() < 1f)
        {
            return false;
        }
        
        if(isBuying)
        {
            resourceManager.setWood(-2f);
            resourceManager.setAtlantium(-1f);
            resourceManager.setFood(-1f);
        }

        return true;
    }

    private bool canBuyTeslaCoil(bool isBuying)
    {
        if(resourceManager.getWood() < 2f || resourceManager.getAtlantium() < 2f || resourceManager.getFood() < 2f)
        {
            return false;
        }
        
        if(isBuying)
        {
            resourceManager.setWood(-2f);
            resourceManager.setAtlantium(-2f);
            resourceManager.setFood(-2f);
        }

        return true;
    }

    private bool canBuyBarracks(bool isBuying)
    {
        if(resourceManager.getWood() < 1f || resourceManager.getAtlantium() < 1f || resourceManager.getFood() < 1f)
        {
            return false;
        }
        
        if(isBuying)
        {
            resourceManager.setWood(-1f);
            resourceManager.setAtlantium(-1f);
            resourceManager.setFood(-1f);
        }

        return true;
    }

    public void setIsBuiltFalse()
    {
        isBuilt = false;
    }
}
