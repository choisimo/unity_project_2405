using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getVehicle : MonoBehaviour
{
    private PlayerController1 playerController;
    private CarController carController;

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController1>();
        carController = GetComponent<CarController>();
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

}
