using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Player UI Elements
    public Slider healthSlider;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI fuelText;

    public startSceneUserController player;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player object is not assigned in UIManager");
            return;
        }
        
        if (healthSlider == null || ammoText == null || fuelText == null)
        {
            Debug.LogError("One or more player UI elements are not assigned in UIManager");
            return;
        }

        UpdateHealthUI();
        UpdateAmmoUI();
        UpdateFuelUI();
    }

    public void UpdateHealthUI()
    {
        if (healthSlider != null && player != null)
        {
            healthSlider.value = player.currentHealth;
        }
        else
        {
            Debug.LogError("Health slider or player object is null in UpdateHealthUI");
        }
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null && player != null)
        {
            ammoText.text = player.ammo.ToString();
        }
        else
        {
            Debug.LogError("Ammo text or player object is null in UpdateAmmoUI");
        }
    }

    public void UpdateFuelUI()
    {
        if (fuelText != null && player != null)
        {
            fuelText.text = player.fuel.ToString();
        }
        else
        {
            Debug.LogError("Fuel text or player object is null in UpdateFuelUI");
        }
    }
}