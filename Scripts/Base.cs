using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Base : MonoBehaviour
{
    [SerializeField] private float startHealth = 100f;
    private float currentHealth;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] SceneTransition sceneTransition;

    private void Start()
    {
        // Sets the starting health and displays it
        currentHealth = startHealth;
        healthText.text = currentHealth.ToString();
    }
    
    public void takeDamage(float _damage)
    {
        // Is called when enemies reach the base and updates it
        currentHealth -= _damage;

        // Checks if all lives are lost and loads the losing screen
        if(currentHealth <= 0f)
        {
            sceneTransition.LoadScreen(1);
        }

        healthText.text = currentHealth.ToString();
    }
}
