using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationSensor : MonoBehaviour
{
    public GameObject detectedDestination;
    public string detectedDestinationState;
    // Start is called before the first frame update
    void Start()
    {
        detectedDestination = null;
        detectedDestinationState = null;
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
            detectedDestinationState = other.gameObject.tag;
        }

        else if (other.tag == "TrainBreak")
        {
            detectedDestination = other.gameObject;
            detectedDestinationState = other.gameObject.tag;
        }

        else if (other.tag == "terminate_breaktrack")
        {
            detectedDestination = other.gameObject;
            detectedDestinationState = other.gameObject.tag;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TrainBreak")
        {
            other.tag = "TrainDestination";
        }
    }
}
