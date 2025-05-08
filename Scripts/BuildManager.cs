using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    [SerializeField] private GameObject archerPrefab, cannonPrefab, laserPrefab, teslaPrefab, barracksPrefab;
    private GameObject towerToBuild;
    [SerializeField] private Shop shop;
    [SerializeField] private TowerSelectUI towerSelectUI;
    private Tower towerSelected;

    void Awake() 
    {
        if(instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }

        instance = this;
    }

    private void Update() 
    {
        // Sets the towerToBuild to null once a round starts
        if(StateManager.instance.getState() != StateManager.SpawnState.WAITING  && towerToBuild != null)
        {
            towerToBuild = null;
        }
    }

    public GameObject getArcherPrefab()
    {
        return archerPrefab;
    }

    public GameObject getCannonPrefab()
    {
        return cannonPrefab;
    }

    public GameObject getLaserPrefab()
    {
        return laserPrefab;
    }

    public GameObject getTeslaPrefab()
    {
        return teslaPrefab;
    }
    public GameObject getBarracksPrefab()
    {
        return barracksPrefab;
    }

    public void SetTowerToBuild(GameObject tower)
    {
        towerToBuild = tower;
        DeselectTower();
    }

    public GameObject GetTowerToBuild()
    {
        return towerToBuild;
    }

    public void TowerSelected(Tower tower)
    {
        // Toggles a towers upgrade menu on and off
        if(towerSelected == tower)
        {
            DeselectTower();
            return;
        }

        towerSelected = tower;
        towerToBuild = null;
        towerSelectUI.setTarget(towerSelected);
    }

    public void DeselectTower()
    {
        towerSelected = null;
        towerSelectUI.Hide();
        shop.HideOutlines();
    }
}
