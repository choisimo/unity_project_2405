using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

//짐칸 스크립트
public class Wagon : MonoBehaviour
{
    bool Connected = false;

    public GameObject trainObject;
    public float WagonSpeed;
    private Train trainScript;

    public GameObject nextdestination;
    
    //경로 탐지기
    public GameObject destination;

    //기차 리지드 바디
    public Rigidbody Wrig;

    public float TurningSpeed = 1f;

    void Update()
    {
        
        if (!Connected) 
        {
            initialize();
        }
        else
        {
            if (trainScript != null)
            {
                nextdestination = destination.GetComponent<DestinationSensor>().detectedDestination;
                if (nextdestination != null)
                {
                    //방향 전환은 자기 센서 사용
                    Vector3 look = nextdestination.transform.position - transform.position;
                    look.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(look);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurningSpeed);
                }

                // 부모 오브젝트의 속도 값을 따라 이동
                WagonSpeed = trainScript.speed;
                Wrig.velocity = transform.forward * WagonSpeed;
                Debug.Log("w" + Wrig.velocity);
            }
        }
    }

    void initialize()
    {
        if (trainObject != null)
        {
            Connected = true;
            trainScript = trainObject.GetComponent<Train>();

            TurningSpeed = trainScript.TurningSpeed;
            Wrig = GetComponent<Rigidbody>();

            //코너를 돌면 짐칸과 기차가 멀어지는 오류 수정
            SpringJoint sj = gameObject.AddComponent<SpringJoint>();
            sj.connectedBody = trainObject.GetComponent<Rigidbody>();
            sj.spring = 100;  // 스프링 강도
            sj.damper = 30;    // 감쇠 값
            sj.minDistance = 0.2f;  // 최소 거리
            sj.maxDistance = 0.5f;  // 최대 거리
        }
    }

}
