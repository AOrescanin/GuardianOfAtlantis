using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    [SerializeField] private float wood = 9, food = 6, ore = 0, atlantium = 3;
    [SerializeField] TextMeshProUGUI woodAmountText, foodAmountText, oreAmountText, atlantiumAmountText;

    private void Awake() 
    {
        if(instance != null)
        {
            Debug.LogError("More than one ResourceManager in scene!");
            return;
        }
        
        instance = this;
    }

    // Displays resource amounts
    void Start() 
    {
        woodAmountText.text = wood.ToString();
        foodAmountText.text = food.ToString();
        oreAmountText.text = ore.ToString();
        atlantiumAmountText.text = atlantium.ToString();
    }

    public void setWood(float amount)
    {
        wood += amount;
        woodAmountText.text = wood.ToString();

        // Checks for if there is enough resources for powercore
        if(StateManager.instance.getRound() == 33 && StateManager.instance.getState() == StateManager.SpawnState.WAITING)
        {
            if(wood >= 33)
            {
                StateManager.instance.EnoughWood();
            }

            if(ore + atlantium >= 33)
            {
                StateManager.instance.enoughOreAndAtlantium();
            }
        }
    }

    public void resetWood(float amount)
    {
        wood = amount;
        woodAmountText.text = wood.ToString();
    }

    public float getWood()
    {
        return wood;
    }

    public void setFood(float amount)
    {
        food += amount;
        foodAmountText.text = food.ToString();

        // Checks for if there is enough resources for powercore
        if(StateManager.instance.getRound() == 33 && StateManager.instance.getState() == StateManager.SpawnState.WAITING)
        {
            if(food >= 33)
            {
                StateManager.instance.EnoughFood();
            }

            if(ore + atlantium >= 33)
            {
                StateManager.instance.enoughOreAndAtlantium();
            }
        }
    }

    public void resetFood(float amount)
    {
        food = amount;
        foodAmountText.text = food.ToString();
    }

    public float getFood()
    {
        return food;
    }

    public void setOre(float amount)
    {
        ore += amount;
        oreAmountText.text = ore.ToString();
    }

    public void resetOre(float amount)
    {
        ore = amount;
        oreAmountText.text = ore.ToString();
    }

    public float getOre()
    {
        return ore;
    }

    public void setAtlantium(float amount)
    {
        atlantium += amount;
        atlantiumAmountText.text = atlantium.ToString();
    }
    
    public float getAtlantium()
    {
        return atlantium;
    }
}
