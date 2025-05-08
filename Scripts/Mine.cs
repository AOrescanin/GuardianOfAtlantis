using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private StateManager stateManager;
    private ResourceManager resourceManager;
    private float generateTime = 2f, yield = 3f;
    private float currentRound;
    private bool check = false;

    void Start()
    {
        stateManager = StateManager.instance;
        resourceManager = ResourceManager.instance;

        currentRound = stateManager.getRound();
    }

    // Generates resources depending on what round it is
    void Update()
    {
        if(stateManager.getPowerCoreIsActive() && !check)
        {
            currentRound = stateManager.getRound();
            check = true;
        }

        if(stateManager.getState() == StateManager.SpawnState.WAITING)
        {
            if(currentRound < stateManager.getRound() && stateManager.getRound() % 2 == 0)
            {
                currentRound += generateTime;
                resourceManager.setOre(yield);
                stateManager.setCanBuildTrue();
            }
        }
    }
}
