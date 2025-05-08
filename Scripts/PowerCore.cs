using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCore : MonoBehaviour
{
    private StateManager stateManager;
    private ResourceManager resourceManager;
    [SerializeField] private float atlantiumCost = 9f;
    [SerializeField] private GameObject powerCore;
    private bool isBuilt = false;

    void Start()
    {
        stateManager = StateManager.instance;
        resourceManager = ResourceManager.instance;
    }

    public void buildPowerCore()
    {
        if(stateManager.getState() == StateManager.SpawnState.WAITING && resourceManager.getAtlantium() >= atlantiumCost && !isBuilt)
        {
            resourceManager.setAtlantium(-atlantiumCost);
            powerCore.SetActive(true);
            isBuilt = true;
            stateManager.setPowerCoreActive();
        }
    }
}
