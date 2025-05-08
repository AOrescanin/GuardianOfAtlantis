using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeUI : MonoBehaviour
{
    private StateManager stateManager;
    private ResourceManager resourceManager;
    [SerializeField] GameObject playerUpgradeUI, boonBoughtText, bootsBoughtText, swordBoughtText;
    [SerializeField] Button playerButton, boonButton, bootsButton, swordButton;
    [SerializeField] PlayerController player;
    [SerializeField] WeaponController sword;
    [SerializeField] float cost = 33;

    private bool isOn = false, boonBought = false, bootsBought = false, swordBought = false;

    void Start()
    {
        stateManager = StateManager.instance;
        resourceManager = ResourceManager.instance;
    }

    void Update()
    {   
        // Turns off menu when round starts
        if(stateManager.getState() != StateManager.SpawnState.WAITING && isOn)
        {
            isOn = false;
            playerUpgradeUI.SetActive(isOn);
            playerButton.interactable = false;
            return;
        }

        // Makes the button interactable when round is done
        playerButton.interactable = true;

        // Updates buttons based on if the player has enough resources
        if(isOn)
        {
            if(resourceManager.getFood() >= cost && !boonBought)
            {
                boonButton.interactable = true;
            }
            else
            {
                boonButton.interactable = false;
                bootsButton.interactable = false;
                swordButton.interactable = false;
            }

            if(resourceManager.getFood() >= cost && resourceManager.getWood() >= cost && !bootsBought)
            {
                bootsButton.interactable = true;
            }
            else
            {
                bootsButton.interactable = false;
            }

            if(resourceManager.getFood() >= cost && resourceManager.getAtlantium() >= cost && !swordBought)
            {
                swordButton.interactable = true;
            }
            else
            {
                swordButton.interactable = false;
            }
        }
    }

    public void togglePlayerUpgradeMenu()
    {
        isOn = !isOn;
        playerUpgradeUI.SetActive(isOn);
    }

    public void buyBoon()
    {
        resourceManager.setFood(-cost);
        player.buffHealth();
        boonBought = true;
        boonBoughtText.SetActive(true);
    }

    public void buyBoots()
    {
        resourceManager.setFood(-cost);
        resourceManager.setWood(-cost);
        player.buffSpeed();
        bootsBought = true;
        bootsBoughtText.SetActive(true);
    }

    public void buySword()
    {
        resourceManager.setFood(-cost);
        resourceManager.setAtlantium(-cost);
        sword.damage *= 3;
        swordBought = true;
        swordBoughtText.SetActive(true);
    }
}
