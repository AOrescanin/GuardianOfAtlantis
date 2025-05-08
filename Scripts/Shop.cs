using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject shopUI, ArcherTowerDesc, CannonDesc, LaserDesc, TeslaDesc, BarracksDesc, PowerCoreDesc;
    [SerializeField] private Button ArcherButton, CannonButton, LaserButton, TeslaButton, BarracksButton, PowercoreButton;
    private Outline archerOutline, cannonOutline, laserOutline, teslaOutline, barracksOutline, powercoreOutline;
    private bool isActive = true;
    BuildManager buildManager;
    StateManager stateManager;
    ResourceManager resourceManager;

    private void Start() 
    {
        buildManager = BuildManager.instance;
        stateManager = StateManager.instance;
        resourceManager = ResourceManager.instance;

        archerOutline = ArcherButton.GetComponent<Outline>();
        cannonOutline = CannonButton.GetComponent<Outline>();
        laserOutline = LaserButton.GetComponent<Outline>();
        teslaOutline = TeslaButton.GetComponent<Outline>();
        barracksOutline = BarracksButton.GetComponent<Outline>();
        powercoreOutline = PowercoreButton.GetComponent<Outline>();
    }

    private void Update() 
    {
        // Toggles shop on and off between rounds
        if(stateManager.getState() != StateManager.SpawnState.WAITING && isActive)
        {
            HideDesc();
            HideOutlines();
            shopUI.SetActive(false);
            isActive = false;
        }
        else if(stateManager.getState() == StateManager.SpawnState.WAITING && !isActive)
        {
            shopUI.SetActive(true);
            isActive = true;
        }

        UpdateButtons();
    }

    // Checks to see if buttons should be disables or enabled depending on if the player has enough resources
    private void UpdateButtons()
    {
        if(resourceManager.getWood() < 3f)
        {
            ArcherButton.interactable = false;
            archerOutline.enabled = false;
            ArcherTowerDesc.SetActive(false);
        }
        else
        {
            ArcherButton.interactable = true;
        }

        if(resourceManager.getWood() < 4f || resourceManager.getAtlantium() < 1f)
        {
            CannonButton.interactable = false;
            cannonOutline.enabled = false;
            CannonDesc.SetActive(false);
        }
        else
        {
            CannonButton.interactable = true;
        }

        if(resourceManager.getWood() < 2f || resourceManager.getAtlantium() < 1f || resourceManager.getFood() < 1f)
        {
            LaserButton.interactable = false;
            laserOutline.enabled = false;
            LaserDesc.SetActive(false);
        }
        else
        {
            LaserButton.interactable = true;
        }

        if(resourceManager.getWood() < 2f || resourceManager.getAtlantium() < 2f || resourceManager.getFood() < 2f)
        {
            TeslaButton.interactable = false;
            teslaOutline.enabled = false;
            TeslaDesc.SetActive(false);
        }
        else
        {
            TeslaButton.interactable = true;
        }

        if(resourceManager.getWood() < 1f || resourceManager.getAtlantium() < 1f || resourceManager.getFood() < 1f)
        {
            BarracksButton.interactable = false;
            barracksOutline.enabled = false;
            BarracksDesc.SetActive(false);
        }
        else
        {
            BarracksButton.interactable = true;
        }

        if(resourceManager.getWood() < 33f || resourceManager.getAtlantium() < 33f || resourceManager.getFood() < 33f)
        {
            PowercoreButton.interactable = false;
            powercoreOutline.enabled = false;
            PowerCoreDesc.SetActive(false);
        }
        else
        {
            PowercoreButton.interactable = true;
        }
    }

    public void selectArcherTower()
    {
        if(stateManager.getState() == StateManager.SpawnState.WAITING)
        {
            buildManager.SetTowerToBuild(buildManager.getArcherPrefab());

            HideDesc();
            ArcherTowerDesc.SetActive(true);
            HideOutlines();
            archerOutline.enabled = true;
        }
    }

    public void selectCannon()
    {
        if(stateManager.getState() == StateManager.SpawnState.WAITING)
        {
            buildManager.SetTowerToBuild(buildManager.getCannonPrefab());

            HideDesc();
            CannonDesc.SetActive(true);
            HideOutlines();
            cannonOutline.enabled = true;
        }
    }

    public void selectEnergyBeamer()
    {
        if(stateManager.getState() == StateManager.SpawnState.WAITING)
        {
            buildManager.SetTowerToBuild(buildManager.getLaserPrefab());

            HideDesc();
            LaserDesc.SetActive(true);
            HideOutlines();
            laserOutline.enabled = true;
        }
    }

    public void selectTeslaCoil()
    {
        if(stateManager.getState() == StateManager.SpawnState.WAITING)
        {
            buildManager.SetTowerToBuild(buildManager.getTeslaPrefab());

            HideDesc();
            TeslaDesc.SetActive(true);
            HideOutlines();
            teslaOutline.enabled = true;
        }
    }

    public void selectBarracks()
    {
        if(stateManager.getState() == StateManager.SpawnState.WAITING)
        {
            buildManager.SetTowerToBuild(buildManager.getBarracksPrefab());

            HideDesc();
            BarracksDesc.SetActive(true);
            HideOutlines();
            barracksOutline.enabled = true;
        }
    }

    public void selectPowerCore()
    {
        if(stateManager.getState() == StateManager.SpawnState.WAITING)
        {
            HideDesc();
            PowerCoreDesc.SetActive(true);
            HideOutlines();
            powercoreOutline.enabled = true;
        }
    }

    public void setPowerCoreActive()
    {
        resourceManager.setAtlantium(-33f);
        stateManager.setPowerCoreActive();
        HideDesc();
    }

    public void HideDesc()
    {
        ArcherTowerDesc.SetActive(false);
        CannonDesc.SetActive(false);
        LaserDesc.SetActive(false);
        TeslaDesc.SetActive(false);
        BarracksDesc.SetActive(false);
        PowerCoreDesc.SetActive(false);
    }

    public void HideOutlines()
    {
        archerOutline.enabled = false;
        cannonOutline.enabled = false;
        laserOutline.enabled = false;
        teslaOutline.enabled = false;
        barracksOutline.enabled = false;
        powercoreOutline.enabled = false;
    }
}
