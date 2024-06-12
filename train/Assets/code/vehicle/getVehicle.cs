using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getVehicle : MonoBehaviour
{
    private userController playerController;
    private CarController carController;
    private Train trainController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<userController>();
        carController = GetComponent<CarController>();
        trainController = GetComponent<Train>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController.nearObject = this.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController.nearObject = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerController.isDriving)
            {
                ExitVehicle();
            }
            else if (playerController.nearObject == this.gameObject)
            {
                EnterVehicle();
            }
        }
    }
    
    void EnterVehicle()
    {
        if (carController != null)
        {
            carController.SetDriving(true);
            if (trainController != null) trainController.SetControl(false);
        }
        else if (trainController != null)
        {
            trainController.SetControl(true);
            if (carController != null) carController.SetDriving(false);
        }
        playerController.isDriving = true;
    }

    void ExitVehicle()
    {
        if (carController != null)
        {
            carController.SetDriving(false);
        }
        if (trainController != null)
        {
            trainController.SetControl(false);
        }
        playerController.isDriving = false;
    }


}
