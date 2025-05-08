using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerSelectUI : MonoBehaviour
{
    ResourceManager resourceManager;
    StateManager stateManager;
    private Tower target;
    [SerializeField] GameObject selectUI; 
    [SerializeField] GameObject ArcherUpgrades, ArcherLvl1UpgradeMenu, ArcherLvl2UpgradeMenu, ArcherLvl3UpgradeMenu, ArcherMaxLvl1UpgradeMenu, ArcherMaxLvl2UpgradeMenu;
    [SerializeField] GameObject CannonUpgrades, CannonLvl1UpgradeMenu, CannonLvl2UpgradeMenu, CannonLvl3UpgradeMenu, CannonMaxLvl1UpgradeMenu, CannonMaxLvl2UpgradeMenu; 
    [SerializeField] GameObject LaserUpgrades, LaserLvl1UpgradeMenu, LaserLvl2UpgradeMenu, LaserLvl3UpgradeMenu, LaserMaxLvl1UpgradeMenu, LaserMaxLvl2UpgradeMenu;
    [SerializeField] GameObject TeslaUpgrades, TeslaLvl1UpgradeMenu, TeslaLvl2UpgradeMenu, TeslaLvl3UpgradeMenu, TeslaMaxLvl1UpgradeMenu, TeslaMaxLvl2UpgradeMenu;
    [SerializeField] GameObject BarracksUpgrades, BarracksLvl1UpgradeMenu, BarracksLvl2UpgradeMenu, BarracksLvl3UpgradeMenu, BarracksMaxLvl1UpgradeMenu, BarracksMaxLvl2UpgradeMenu;    
    [SerializeField] GameObject radius9, radius12, radius13_5, radius15, radius16_5, radius18, radius19_5, radius21, radius24, radius27;
    [SerializeField] float RadiusOffset = -.81f;
    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] Button ArcherUpg1Button, ArcherUpg2Button, ArcherUpg3Button, ArcherUpg4Button, CannonUpg1Button, CannonUpg2Button, CannonUpg3Button, CannonUpg4Button,
    LaserUpg1Button, LaserUpg2Button, LaserUpg3Button, LaserUpg4Button, TeslaUpg1Button, TeslaUpg2Button, TeslaUpg3Button, TeslaUpg4Button, 
    BarracksUpg1Button, BarracksUpg2Button, BarracksUpg3Button, BarracksUpg4Button;
    [SerializeField] GameObject deselectButton;
    private int type;
    private bool isActive = true;

    void Start()
    {
        resourceManager = ResourceManager.instance;
        stateManager = StateManager.instance;
    }

    void Update() 
    {
        if(target == null) {return;}

        type = target.getType();

        // Toggles Upgrade UI  off when round starts
        if(stateManager.getState() != StateManager.SpawnState.WAITING && isActive)
        {
            Hide();
            isActive = false;
        }

        UpdateButtons();
    }

    // Makes buttons interactable and uninteraactable based on if the player has enough resources
    private void UpdateButtons()
    {
        if(type == 1)
        {
            if(target.getUpgradeLvl() == 0)
            {
                if(resourceManager.getWood() < 4f)
                {
                    ArcherUpg1Button.interactable = false;
                }
                else
                {
                    ArcherUpg1Button.interactable = true;
                }
            }
            else if(target.getUpgradeLvl() == 1)
            {
                if(resourceManager.getWood() < 6f)
                {
                    ArcherUpg2Button.interactable = false;
                }
                else
                {
                    ArcherUpg2Button.interactable = true;
                }
            }
            else if(target.getUpgradeLvl() == 2)
            {
                if(resourceManager.getWood() < 9f)
                {
                    ArcherUpg3Button.interactable = false;
                }
                else
                {
                    ArcherUpg3Button.interactable = true;
                }

                if(resourceManager.getWood() < 8f || resourceManager.getAtlantium() < 1f)
                {
                    ArcherUpg4Button.interactable = false;
                }
                else
                {
                    ArcherUpg4Button.interactable = true;
                }
            }
        }
        else if(type == 2)
        {
            if(target.getUpgradeLvl() == 0)
            {
                if(resourceManager.getWood() < 6f || resourceManager.getAtlantium() < 2f)
                {
                    CannonUpg1Button.interactable = false;
                }
                else
                {
                    CannonUpg1Button.interactable = true;
                }
            }
            else if(target.getUpgradeLvl() == 1)
            {
                if(resourceManager.getWood() < 8f || resourceManager.getAtlantium() < 3f)
                {
                    CannonUpg2Button.interactable = false;
                }
                else
                {
                    CannonUpg2Button.interactable = true;
                }
            }
            else if(target.getUpgradeLvl() == 2)
            {
                if(resourceManager.getWood() < 10f || resourceManager.getAtlantium() < 4f)
                {
                    CannonUpg3Button.interactable = false;
                }
                else
                {
                    CannonUpg3Button.interactable = true;
                }

                if(resourceManager.getWood() < 8f || resourceManager.getAtlantium() < 5f)
                {
                    CannonUpg4Button.interactable = false;
                }
                else
                {
                    CannonUpg4Button.interactable = true;
                }
            }
        }
        else if(type == 3)
        {
            if(target.getUpgradeLvl() == 0)
            {
                if(resourceManager.getWood() < 4f || resourceManager.getAtlantium() < 1f || resourceManager.getFood() < 2f)
                {
                    LaserUpg1Button.interactable = false;
                }
                else
                {
                    LaserUpg1Button.interactable = true;
                }
            }
            else if(target.getUpgradeLvl() == 1)
            {
                if(resourceManager.getWood() < 5f || resourceManager.getAtlantium() < 2f || resourceManager.getFood() < 3f)
                {
                    LaserUpg2Button.interactable = false;
                }
                else
                {
                    LaserUpg2Button.interactable = true;
                }
            }
            else if(target.getUpgradeLvl() == 2)
            {
                if(resourceManager.getWood() < 6f || resourceManager.getAtlantium() < 2f || resourceManager.getFood() < 4f)
                {
                    LaserUpg3Button.interactable = false;
                    LaserUpg4Button.interactable = false;
                }
                else
                {
                    LaserUpg3Button.interactable = true;
                    LaserUpg4Button.interactable = true;
                }
            }
        }
        else if(type == 4)
        {
            if(target.getUpgradeLvl() == 0)
            {
                if(resourceManager.getWood() < 3f || resourceManager.getAtlantium() < 3f || resourceManager.getFood() < 3f)
                {
                    TeslaUpg1Button.interactable = false;
                }
                else
                {
                    TeslaUpg1Button.interactable = true;
                }
            }
            else if(target.getUpgradeLvl() == 1)
            {
                if(resourceManager.getWood() < 4f || resourceManager.getAtlantium() < 4f || resourceManager.getFood() < 4f)
                {
                    TeslaUpg2Button.interactable = false;
                }
                else
                {
                    TeslaUpg2Button.interactable = true;
                }
            }
            else if(target.getUpgradeLvl() == 2)
            {
                if(resourceManager.getWood() < 5f || resourceManager.getAtlantium() < 5f || resourceManager.getFood() < 5f)
                {
                    TeslaUpg3Button.interactable = false;
                }
                else
                {
                    TeslaUpg3Button.interactable = true;
                }

                if(resourceManager.getWood() < 6f || resourceManager.getAtlantium() < 6f || resourceManager.getFood() < 6f)
                {
                    TeslaUpg4Button.interactable = false;
                }
                else
                {
                    TeslaUpg4Button.interactable = true;
                }
            }
        }
        else if(type == 5)
        {
            if(target.getUpgradeLvl() == 0)
            {
                if(resourceManager.getWood() < 2f || resourceManager.getAtlantium() < 1f || resourceManager.getFood() < 2f)
                {
                    BarracksUpg1Button.interactable = false;
                }
                else
                {
                    BarracksUpg1Button.interactable = true;
                }
            }
            else if(target.getUpgradeLvl() == 1)
            {
                if(resourceManager.getWood() < 3f || resourceManager.getAtlantium() < 1f || resourceManager.getFood() < 3f)
                {
                    BarracksUpg2Button.interactable = false;
                }
                else
                {
                    BarracksUpg2Button.interactable = true;
                }
            }
            else if(target.getUpgradeLvl() == 2)
            {
                if(resourceManager.getWood() < 4f || resourceManager.getAtlantium() < 1f || resourceManager.getFood() < 4f)
                {
                    BarracksUpg3Button.interactable = false;
                }
                else
                {
                    BarracksUpg3Button.interactable = true;
                }

                if(resourceManager.getWood() < 4f || resourceManager.getFood() < 6f)
                {
                    BarracksUpg4Button.interactable = false;
                }
                else
                {
                    BarracksUpg4Button.interactable = true;
                }
            }
        }
    }
    // Sets the Upgrade UI to match the tower that was selected
    public void setTarget(Tower _target)
    {
        if(stateManager.getState() != StateManager.SpawnState.WAITING)
        {
            return;
        }

        selectUI.SetActive(true);
        deselectButton.SetActive(true);
        isActive = true;
        target = _target;
        type = target.getType();

        if(type == 1)
        {
            HideUpgrades();
            ArcherUpgrades.SetActive(true);

            if(target.getUpgradeLvl() == 0)
            {
                headerText.text = "Level 1 Archers";

                HideArcherUpgrades();
                ArcherLvl1UpgradeMenu.SetActive(true);

                HideRadii();
                radius12.transform.position = target.transform.position;
                radius12.transform.position = new Vector3(radius12.transform.position.x, RadiusOffset, radius12.transform.position.z);
                radius12.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 1)
            {
                headerText.text = "Level 2 Archers";

                HideArcherUpgrades();
                ArcherLvl2UpgradeMenu.SetActive(true);

                HideRadii();
                radius15.transform.position = target.transform.position;
                radius15.transform.position = new Vector3(radius15.transform.position.x, RadiusOffset, radius15.transform.position.z);
                radius15.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 2)
            {
                headerText.text = "Level 3 Archers";

                HideArcherUpgrades();
                ArcherLvl3UpgradeMenu.SetActive(true);

                HideRadii();
                radius18.transform.position = target.transform.position;
                radius18.transform.position = new Vector3(radius18.transform.position.x, RadiusOffset, radius18.transform.position.z);
                radius18.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 3)
            {
                headerText.text = "Adept Bowmen";
                
                HideArcherUpgrades();
                ArcherMaxLvl1UpgradeMenu.SetActive(true);

                HideRadii();
                radius21.transform.position = target.transform.position;
                radius21.transform.position = new Vector3(radius21.transform.position.x, RadiusOffset, radius21.transform.position.z);
                radius21.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 4)
            {
                headerText.text = "Crossbowmen";

                HideArcherUpgrades();
                ArcherMaxLvl2UpgradeMenu.SetActive(true);

                HideRadii();
                radius24.transform.position = target.transform.position;
                radius24.transform.position = new Vector3(radius24.transform.position.x, RadiusOffset, radius24.transform.position.z);
                radius24.SetActive(true);
            }
        }
        else if(type == 2)
        {
            HideUpgrades();
            CannonUpgrades.SetActive(true);

            if(target.getUpgradeLvl() == 0)
            {
                headerText.text = "Level 1 Dwarven Artillery";

                HideCannonUpgrades();
                CannonLvl1UpgradeMenu.SetActive(true);

                HideRadii();
                radius15.transform.position = target.transform.position;
                radius15.transform.position = new Vector3(radius15.transform.position.x, RadiusOffset, radius15.transform.position.z);
                radius15.SetActive(true);
            }
            
            else if(target.getUpgradeLvl() == 1)
            {
                headerText.text = "Level 2 Dwarven Artillery";

                HideCannonUpgrades();
                CannonLvl2UpgradeMenu.SetActive(true);

                HideRadii();
                radius16_5.transform.position = target.transform.position;
                radius16_5.transform.position = new Vector3(radius16_5.transform.position.x, RadiusOffset, radius16_5.transform.position.z);
                radius16_5.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 2)
            {
                headerText.text = "Level 3 Dwarven Artillery";

                HideCannonUpgrades();
                CannonLvl3UpgradeMenu.SetActive(true);

                HideRadii();
                radius18.transform.position = target.transform.position;
                radius18.transform.position = new Vector3(radius18.transform.position.x, RadiusOffset, radius18.transform.position.z);
                radius18.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 3)
            {
                headerText.text = "a.o.e.";

                HideCannonUpgrades();
                CannonMaxLvl1UpgradeMenu.SetActive(true);

                HideRadii();
                radius19_5.transform.position = target.transform.position;
                radius19_5.transform.position = new Vector3(radius19_5.transform.position.x, RadiusOffset, radius19_5.transform.position.z);
                radius19_5.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 4)
            {
                headerText.text = "Dwarven g.o.l.e.m.";

                HideCannonUpgrades();
                CannonMaxLvl2UpgradeMenu.SetActive(true);

                HideRadii();
                radius13_5.transform.position = target.transform.position;
                radius13_5.transform.position = new Vector3(radius13_5.transform.position.x, RadiusOffset, radius13_5.transform.position.z);
                radius13_5.SetActive(true);
            }
        }
        else if(type == 3)
        {
            HideUpgrades();
            LaserUpgrades.SetActive(true);

            if(target.getUpgradeLvl() == 0)
            {
                headerText.text = "Level 1 Sunbeam Laser";

                HideLaserUpgrades();
                LaserLvl1UpgradeMenu.SetActive(true);

                HideRadii();
                radius12.transform.position = target.transform.position;
                radius12.transform.position = new Vector3(radius12.transform.position.x, RadiusOffset, radius12.transform.position.z);
                radius12.SetActive(true);
            }
            
            else if(target.getUpgradeLvl() == 1)
            {
                headerText.text = "Level 2 Sunbeam Laser";

                HideLaserUpgrades();
                LaserLvl2UpgradeMenu.SetActive(true);

                HideRadii();
                radius15.transform.position = target.transform.position;
                radius15.transform.position = new Vector3(radius15.transform.position.x, RadiusOffset, radius15.transform.position.z);
                radius15.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 2)
            {
                headerText.text = "Level 3 Sunbeam Laser";

                HideLaserUpgrades();
                LaserLvl3UpgradeMenu.SetActive(true);

                HideRadii();
                radius18.transform.position = target.transform.position;
                radius18.transform.position = new Vector3(radius18.transform.position.x, RadiusOffset, radius18.transform.position.z);
                radius18.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 3)
            {
                headerText.text = "Solray";

                HideLaserUpgrades();
                LaserMaxLvl1UpgradeMenu.SetActive(true);

                HideRadii();
                radius21.transform.position = target.transform.position;
                radius21.transform.position = new Vector3(radius21.transform.position.x, RadiusOffset, radius21.transform.position.z);
                radius21.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 4)
            {
                headerText.text = "Sunburst Cannon";

                HideLaserUpgrades();
                LaserMaxLvl2UpgradeMenu.SetActive(true);

                HideRadii();
                radius21.transform.position = target.transform.position;
                radius21.transform.position = new Vector3(radius21.transform.position.x, RadiusOffset, radius21.transform.position.z);
                radius21.SetActive(true);
            }
        }
        else if(type == 4)
        {
            HideUpgrades();
            TeslaUpgrades.SetActive(true);

            if(target.getUpgradeLvl() == 0)
            {
                headerText.text = "Level 1 Tesla";

                HideTeslaUpgrades();
                TeslaLvl1UpgradeMenu.SetActive(true);

                HideRadii();
                radius15.transform.position = target.transform.position;
                radius15.transform.position = new Vector3(radius15.transform.position.x, RadiusOffset, radius15.transform.position.z);
                radius15.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 1)
            {
                headerText.text = "Level 2 Tesla";

                HideTeslaUpgrades();
                TeslaLvl2UpgradeMenu.SetActive(true);

                HideRadii();
                radius16_5.transform.position = target.transform.position;
                radius16_5.transform.position = new Vector3(radius16_5.transform.position.x, RadiusOffset, radius16_5.transform.position.z);
                radius16_5.SetActive(true);
                
            }
            else if(target.getUpgradeLvl() == 2)
            {
                headerText.text = "Level 3 Tesla";

                HideTeslaUpgrades();
                TeslaLvl3UpgradeMenu.SetActive(true);

                HideRadii();
                radius18.transform.position = target.transform.position;
                radius18.transform.position = new Vector3(radius18.transform.position.x, RadiusOffset, radius18.transform.position.z);
                radius18.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 3)
            {
                headerText.text = "thor";

                HideTeslaUpgrades();
                TeslaMaxLvl1UpgradeMenu.SetActive(true);

                HideRadii();
                radius19_5.transform.position = target.transform.position;
                radius19_5.transform.position = new Vector3(radius19_5.transform.position.x, RadiusOffset, radius19_5.transform.position.z);
                radius19_5.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 4)
            {
                headerText.text = "Storm Charge";

                HideTeslaUpgrades();
                TeslaMaxLvl2UpgradeMenu.SetActive(true);

                HideRadii();
                radius27.transform.position = target.transform.position;
                radius27.transform.position = new Vector3(radius27.transform.position.x, RadiusOffset, radius27.transform.position.z);
                radius27.SetActive(true);
            }
        }
        else if(type == 5)
        {
            HideUpgrades();
            BarracksUpgrades.SetActive(true);

            if(target.getUpgradeLvl() == 0)
            {
                headerText.text = "Level 1 Garrison";

                HideBarracksUpgrades();
                BarracksLvl1UpgradeMenu.SetActive(true);
            }
            
            else if(target.getUpgradeLvl() == 1)
            {
                headerText.text = "Level 2 Garrison";

                HideBarracksUpgrades();
                BarracksLvl2UpgradeMenu.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 2)
            {
                headerText.text = "Level 3 Garrison";

                HideBarracksUpgrades();
                BarracksLvl3UpgradeMenu.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 3)
            {
                headerText.text = "Atlantean Honor Guard";

                HideBarracksUpgrades();
                BarracksMaxLvl1UpgradeMenu.SetActive(true);
            }
            else if(target.getUpgradeLvl() == 4)
            {
                headerText.text = "Storm-bred Warband";

                HideBarracksUpgrades();
                BarracksMaxLvl2UpgradeMenu.SetActive(true);
            }

            HideRadii();
            radius9.transform.position = target.transform.position;
            radius9.transform.position = new Vector3(radius9.transform.position.x, RadiusOffset, radius9.transform.position.z);
            radius9.SetActive(true);
        }
    }

    // Shows radius of the possible upgrades depending on tower type and level
    public void showUpgradeRadius()
    {
        if(type == 1)
        {
            if(target.getUpgradeLvl() == 0)
            {
                radius15.transform.position = target.transform.position;
                radius15.transform.position = new Vector3(radius15.transform.position.x, RadiusOffset, radius15.transform.position.z);
                radius15.SetActive(true);
            }
            if(target.getUpgradeLvl() == 1)
            {
                radius18.transform.position = target.transform.position;
                radius18.transform.position = new Vector3(radius18.transform.position.x, RadiusOffset, radius18.transform.position.z);
                radius18.SetActive(true);
            }
            if(target.getUpgradeLvl() == 2)
            {
                radius21.transform.position = target.transform.position;
                radius21.transform.position = new Vector3(radius21.transform.position.x, RadiusOffset, radius21.transform.position.z);
                radius21.SetActive(true);
            }
        }
        else if(type == 2)
        {
            if(target.getUpgradeLvl() == 0)
            {
                radius16_5.transform.position = target.transform.position;
                radius16_5.transform.position = new Vector3(radius16_5.transform.position.x, RadiusOffset, radius16_5.transform.position.z);
                radius16_5.SetActive(true);
            }
            if(target.getUpgradeLvl() == 1)
            {
                radius18.transform.position = target.transform.position;
                radius18.transform.position = new Vector3(radius18.transform.position.x, RadiusOffset, radius18.transform.position.z);
                radius18.SetActive(true);
            }
            if(target.getUpgradeLvl() == 2)
            {
                radius19_5.transform.position = target.transform.position;
                radius19_5.transform.position = new Vector3(radius19_5.transform.position.x, RadiusOffset, radius19_5.transform.position.z);
                radius19_5.SetActive(true);
            }
        }
        else if(type == 3)
        {
            if(target.getUpgradeLvl() == 0)
            {
                radius15.transform.position = target.transform.position;
                radius15.transform.position = new Vector3(radius15.transform.position.x, RadiusOffset, radius15.transform.position.z);
                radius15.SetActive(true);
            }
            if(target.getUpgradeLvl() == 1)
            {
                radius18.transform.position = target.transform.position;
                radius18.transform.position = new Vector3(radius18.transform.position.x, RadiusOffset, radius18.transform.position.z);
                radius18.SetActive(true);
            }
            if(target.getUpgradeLvl() == 2)
            {
                radius21.transform.position = target.transform.position;
                radius21.transform.position = new Vector3(radius21.transform.position.x, RadiusOffset, radius21.transform.position.z);
                radius21.SetActive(true);
            }
        }
        else if(type == 4)
        {
            if(target.getUpgradeLvl() == 0)
            {
                radius16_5.transform.position = target.transform.position;
                radius16_5.transform.position = new Vector3(radius16_5.transform.position.x, RadiusOffset, radius16_5.transform.position.z);
                radius16_5.SetActive(true);
            }
            if(target.getUpgradeLvl() == 1)
            {
                radius18.transform.position = target.transform.position;
                radius18.transform.position = new Vector3(radius18.transform.position.x, RadiusOffset, radius18.transform.position.z);
                radius18.SetActive(true);
            }
            if(target.getUpgradeLvl() == 2)
            {
                radius19_5.transform.position = target.transform.position;
                radius19_5.transform.position = new Vector3(radius19_5.transform.position.x, RadiusOffset, radius19_5.transform.position.z);
                radius19_5.SetActive(true);
            }
        }

    }

    public void showMaxUpgradeRadius2()
    {
        if(type == 1)
        {
            radius24.transform.position = target.transform.position;
            radius24.transform.position = new Vector3(radius24.transform.position.x, RadiusOffset, radius24.transform.position.z);
            radius24.SetActive(true);
        }
        if(type == 2)
        {
            radius13_5.transform.position = target.transform.position;
            radius13_5.transform.position = new Vector3(radius13_5.transform.position.x, RadiusOffset, radius13_5.transform.position.z);
            radius13_5.SetActive(true);
        }
        if(type == 4)
        {
            radius27.transform.position = target.transform.position;
            radius27.transform.position = new Vector3(radius27.transform.position.x, RadiusOffset, radius27.transform.position.z);
            radius27.SetActive(true);
        }
    }

    public void hideUpgradeRadius()
    {
        if(type == 1)
        {
            if(target.getUpgradeLvl() == 0)
            {
                radius15.SetActive(false);
            }
            if(target.getUpgradeLvl() == 1)
            {
                radius18.SetActive(false);
            }
            if(target.getUpgradeLvl() == 2)
            {
                radius21.SetActive(false);
            }
        }
        else if(type == 2)
        {
            if(target.getUpgradeLvl() == 0)
            {
                radius16_5.SetActive(false);
            }
            if(target.getUpgradeLvl() == 1)
            {
                radius18.SetActive(false);
            }
            if(target.getUpgradeLvl() == 2)
            {
                radius19_5.SetActive(false);
            }
        }
        if(type == 3)
        {
            if(target.getUpgradeLvl() == 0)
            {
                radius15.SetActive(false);
            }
            if(target.getUpgradeLvl() == 1)
            {
                radius18.SetActive(false);
            }
            if(target.getUpgradeLvl() == 2)
            {
                radius21.SetActive(false);
            }
        }
        else if(type == 4)
        {
            if(target.getUpgradeLvl() == 0)
            {
                radius16_5.SetActive(false);
            }
            if(target.getUpgradeLvl() == 1)
            {
                radius18.SetActive(false);
            }
            if(target.getUpgradeLvl() == 2)
            {
                radius19_5.SetActive(false);
            }
        }
    }

// handles one of the branching tower paths
    public void hideMaxUpgradeRadius2()
    {
        if(type == 1)
        {
            radius24.SetActive(false);
        }
        if(type == 2)
        {
            radius13_5.SetActive(false);
        }
        if(type == 4)
        {
            radius27.SetActive(false);
        }
    }

    public void buyNormalUpgrade()
    {
        target.upgrade(false);
        setTarget(target);
    }

    // handles one of the branching tower paths
    public void buyUpgrade2()
    {
        target.upgrade(true);
        setTarget(target);
    }

    public void sellTower()
    {
        resourceManager.setWood(target.getSellWoodCost());
        resourceManager.setOre(target.getSellAtlCost());

        target.Deconstruct();

        Hide();
    }

    public void Hide()
    {
        selectUI.SetActive(false);
        deselectButton.gameObject.SetActive(false);
        HideRadii();
    }

    private void HideRadii()
    {
        radius9.SetActive(false);
        radius12.SetActive(false);
        radius13_5.SetActive(false);
        radius15.SetActive(false);
        radius16_5.SetActive(false);
        radius18.SetActive(false);
        radius19_5.SetActive(false);
        radius21.SetActive(false);
        radius24.SetActive(false);
        radius27.SetActive(false);
    }

    private void HideUpgrades()
    {
        ArcherUpgrades.SetActive(false);
        CannonUpgrades.SetActive(false);
        LaserUpgrades.SetActive(false);
        TeslaUpgrades.SetActive(false);
        BarracksUpgrades.SetActive(false);
    }

    private void HideArcherUpgrades()
    {
        ArcherLvl1UpgradeMenu.SetActive(false);
        ArcherLvl2UpgradeMenu.SetActive(false);
        ArcherLvl3UpgradeMenu.SetActive(false);
        ArcherMaxLvl1UpgradeMenu.SetActive(false);
        ArcherMaxLvl2UpgradeMenu.SetActive(false);
    }

    private void HideCannonUpgrades()
    {
        CannonLvl1UpgradeMenu.SetActive(false);
        CannonLvl2UpgradeMenu.SetActive(false);
        CannonLvl3UpgradeMenu.SetActive(false);
        CannonMaxLvl1UpgradeMenu.SetActive(false);
        CannonMaxLvl2UpgradeMenu.SetActive(false);
    }

    private void HideLaserUpgrades()
    {
        LaserLvl1UpgradeMenu.SetActive(false);
        LaserLvl2UpgradeMenu.SetActive(false);
        LaserLvl3UpgradeMenu.SetActive(false);
        LaserMaxLvl1UpgradeMenu.SetActive(false);
        LaserMaxLvl2UpgradeMenu.SetActive(false);
    }    

    private void HideTeslaUpgrades()
    {
        TeslaLvl1UpgradeMenu.SetActive(false);
        TeslaLvl2UpgradeMenu.SetActive(false);
        TeslaLvl3UpgradeMenu.SetActive(false);
        TeslaMaxLvl1UpgradeMenu.SetActive(false);
        TeslaMaxLvl2UpgradeMenu.SetActive(false);
    }

    private void HideBarracksUpgrades()
    {
        BarracksLvl1UpgradeMenu.SetActive(false);
        BarracksLvl2UpgradeMenu.SetActive(false);
        BarracksLvl3UpgradeMenu.SetActive(false);
        BarracksMaxLvl1UpgradeMenu.SetActive(false);
        BarracksMaxLvl2UpgradeMenu.SetActive(false);
    }
}
