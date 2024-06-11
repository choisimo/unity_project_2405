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
    
    
    // Vehicle UI Elements
    public TextMeshProUGUI vehicleSpeedText;
    public TextMeshProUGUI vehicleFuelText;
    public RawImage IconFuel;
    
    
    public userController player;
    public CarController car;


    void Start()
    {
        UpdateHealthUI();
        UpdateAmmoUI();
        UpdateFuelUI();
        UpdateVehicleUI(false); // Initially, vehicle UI should be hidden
    }

    public void UpdateHealthUI()
    {
        healthSlider.value = player.currentHealth;
    }

    public void UpdateAmmoUI()
    {
        ammoText.text = "Ammo: " + player.ammo.ToString();
    }

    public void UpdateFuelUI()
    {
        fuelText.text = "Fuel: " + player.fuel.ToString();
    }

    public void UpdateVehicleUI(bool isDriving)
    {
        if (isDriving)
        {
            // Enable vehicle UI
            vehicleSpeedText.gameObject.SetActive(true);
            vehicleFuelText.gameObject.SetActive(true);
            IconFuel.gameObject.SetActive(true);

            // Disable player UI
            ammoText.gameObject.SetActive(false);
            fuelText.gameObject.SetActive(false);
            healthSlider.gameObject.SetActive(false);
        }
        else
        {
            // Enable player UI
            ammoText.gameObject.SetActive(true);
            IconFuel.gameObject.SetActive(true);

            // Disable vehicle UI
            vehicleSpeedText.gameObject.SetActive(false);
            vehicleFuelText.gameObject.SetActive(false);
            healthSlider.gameObject.SetActive(true);
        }
    }

    public void UpdateVehicleSpeedUI(float speed)
    {
        vehicleSpeedText.text = "Speed: " + speed.ToString("F1") + " km/h";
    }

    public void UpdateVehicleFuelUI(int fuel)
    {
        vehicleFuelText.text = "Fuel: " + fuel.ToString();
    }
}
