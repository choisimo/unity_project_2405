using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Train : MonoBehaviour
{
    //기차속도
    protected float speed;
    
    //경로 탐지기
    public GameObject destination;

    //기차 리지드 바디
    private Rigidbody rig;
    //기차 회전속도
    public float TurningSpeed = 1f;

    //철로에 깔린 경로 블럭
    GameObject nextdestination;
    //기차 후레쉬
    public GameObject TrainLight;

    //기차 후레쉬 버튼 눌렸나?
    bool LDown;

    public float decelerate = 0;
    //자원
    public float energy = 0;
    public float energyRate = 0.01f;

    //기차 짐칸
    public GameObject[] Wagon;

    private bool isControlled = false;
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        speed = TNTrain.TNTrainspeed;
    }

    void Update()
    {
        GetInput();

        Light();

        nextdestination = destination.GetComponent<DestinationSensor>().detectedDestination;
        string trainState = destination.GetComponent<DestinationSensor>().detectedDestinationState;

        if (nextdestination != null)
        {
            
            Vector3 look = nextdestination.transform.position - transform.position;
            look.y = 0;
            Quaternion rotation = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurningSpeed);

            if(energy > 0)
            {
                if (trainState == "TrainDestination")
                {
                    speed = TNTrain.TNTrainspeed;
                    rig.velocity = transform.forward * speed;
                }
                else if (trainState == "TrainBreak")
                {
                    speed = Mathf.Lerp(speed, 1, decelerate * Time.deltaTime);
                    rig.velocity = transform.forward * speed;
                }
                else if (trainState == "terminate_breaktrack")
                {
                    rig.velocity = Vector3.zero;
                }
                energy -= (speed * 0.05f)  * energyRate * Time.deltaTime;

            }
            else
            {
                energy = 0;
                speed = Mathf.Lerp(speed, 1, 0.9f * Time.deltaTime);
                rig.velocity = transform.forward * speed;
            }

        }

    }
    //키 입력
    void GetInput()
    {
        LDown = Input.GetButtonDown("Light");
    }

    //불 키기
    void Light()
    {
        if ((LDown))
        {
            Debug.Log("L");
            if (!TrainLight.activeSelf)
                TrainLight.SetActive(true);
            else
                TrainLight.SetActive(false);
        }
    }
    
    public void SetControl(bool control)
    {
        isControlled = control;
    } 

}
