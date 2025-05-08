using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthbarImage;
    private GameObject camPivot;

    void Start()
    {
        camPivot = GameObject.FindGameObjectWithTag("Pivot");
    }

    void Update() 
    {
        // Updates the health bar to always face the camera
        transform.rotation = camPivot.transform.rotation;
    }

    public void updateHealthBar(float maxHealth, float currentHealth)
    {
        // Updates the health bar to reflect current health percentage
        healthbarImage.fillAmount = currentHealth / maxHealth;
    }
}
