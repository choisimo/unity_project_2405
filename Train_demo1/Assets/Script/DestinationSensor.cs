using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationSensor : MonoBehaviour
{
    public GameObject detectedDestination;
    // Start is called before the first frame update
    void Start()
    {
        detectedDestination = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TrainDestination")
        {
            detectedDestination = other.gameObject;
            Debug.Log("d");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TrainDestination")
        {
            Debug.Log("un");
        }
    }
}
