using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

//기차 스크립트
public class Train : MonoBehaviour
{
    //기차속도
    public float speed;
    
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
    public float Maxenergy = 100;
    public float energyRate = 0.01f;

    //기차 짐칸
    public GameObject[] Wagon;

    private bool isControlled = false;

    public bool Go = false;

    public bool Connected = false;

    int randomIndex;
    int MaxSpeed;

    bool trainsound;
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        speed = TNTrain.TNTrainspeed;
    }

    //현재 짐칸 연결 상태
    bool IsWagonConnected = false;

    void Update()
    {

        randomIndex = StaticVal.Instance.GetRandomIndex();
        MaxSpeed = StaticVal.Instance.GetMS();
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
                    Go = false;
                    speed = TNTrain.TNTrainspeed;
                    rig.velocity = transform.forward * speed;
                }
                else if (trainState == "TrainBreak")
                {
                    if(speed > 1f)
                        speed = Mathf.Lerp(speed, 1, decelerate * Time.deltaTime);
                    else
                        speed = TNTrain.TNTrainspeed;
                    rig.velocity = transform.forward * speed;
                }
                else if (trainState == "terminate_breaktrack")
                {
                    if(Go == false)
                    {
                        speed = 0;
                        TNTrain.Instance.SetSliderValue(0);
                        rig.velocity = transform.forward * speed;
                    }
                    else
                    {
                        speed = 1;
                        rig.velocity = transform.forward * speed;
                    }
                }

                energy -= (speed * 0.05f)  * energyRate * Time.deltaTime;
            }
            else
            {
                energy = 0;
                speed = Mathf.Lerp(speed, 1, 0.9f * Time.deltaTime);
                rig.velocity = transform.forward * speed;
            }

            if(speed < 7f && 1f < speed)
            {
                if(!trainsound)
                {
                    TrainSound.ts.Play_Basic();
                    trainsound = true;
                }
            }
            else if(speed >= 7f && speed < 14f)
            {
                if (trainsound)
                {
                    TrainSound.ts.Play_Middle();
                    trainsound = false;
                }
            }
            else if(speed >= 14f)
            {
                if (!trainsound)
                {
                    TrainSound.ts.Play_Max();
                    trainsound = true;
                }
            }

        }

        if (Connected == true)
        {
            if (randomIndex == 0)
            {
                energyRate = 0.7f;
            }
            else if (randomIndex == 1)
            {
                Maxenergy = 200;
            }
            else if (randomIndex == 2)
            {
                Maxenergy = 150;
            }
            else if (randomIndex == 4)
            {
                StaticVal.Instance.SetMS(25);
            }
            else if (randomIndex == 5)
            {
                StaticVal.Instance.SetMS(23);
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

    public void MoveForward()
    {
        rig.velocity = transform.forward * 2;
    }

    /*public void SetControl(bool control)
    {
        isControlled = control;
    } */

}
