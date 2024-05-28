using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    private float speed;
    public GameObject destination;

    private Rigidbody rig;
    public float TurningSpeed = 1f;

    GameObject nextdestination;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        speed = TNTrain.TNTrainspeed;
    }

    void Update()
    {
        speed = TNTrain.TNTrainspeed;

        nextdestination = destination.GetComponent<DestinationSensor>().detectedDestination;
        
        if (nextdestination != null ) 
        { 
            Vector3 look = nextdestination.transform.position - transform.position;
            look.y = 0;
            Quaternion rotation = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurningSpeed);

            rig.velocity = transform.forward * speed;
        }
        
    }
}
