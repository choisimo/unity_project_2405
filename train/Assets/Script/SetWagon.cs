using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetWagon : MonoBehaviour
{
    private Train train;
    public Wagon wagon;
    public GameObject[] wp;


    public float moveDuration = 3.0f;
    private Vector3 initialPosition; 
    private Vector3 targetPosition;
    private bool isMoving = false;

    int randomIndex;

    public void Initialize(Wagon newWagon)
    {
        wagon = newWagon;
    }

    void Update()
    {
        randomIndex = StaticVal.Instance.GetRandomIndex();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "train")
        {
            GameObject train = other.gameObject;
            Train Train = train.GetComponent<Train>();
            Train.speed = 1;
            foreach (GameObject point in wp)
            {
                point.SetActive(false);
            }

            initialPosition = wagon.transform.position;
            Debug.Log(randomIndex);
            if(randomIndex == 1)
            {
                targetPosition = initialPosition + wagon.transform.forward * 11f;
                StartCoroutine(MoveOverTime(wagon.gameObject, 11f));
            }
            else if (randomIndex == 0)
            {
                targetPosition = initialPosition + wagon.transform.forward * 9.5f;
                StartCoroutine(MoveOverTime(wagon.gameObject, 9.5f));
            }
            else if (randomIndex == 2 || randomIndex == 3)
            {
                targetPosition = initialPosition + wagon.transform.forward * 9.8f;
                StartCoroutine(MoveOverTime(wagon.gameObject, 9f));
            }
            else
            {
                targetPosition = initialPosition + wagon.transform.forward * 10f;
                StartCoroutine(MoveOverTime(wagon.gameObject, 10f));
            }

            StartCoroutine(DelayedTrainObjectAssignment(train));
            StartCoroutine(DelayedTrain(Train));
        }

    }

    
    IEnumerator DelayedTrainObjectAssignment(GameObject train)
    {

        yield return new WaitForSeconds(3.0f);
        wagon.trainObject = train;
        
    }
    IEnumerator DelayedTrain(Train train)
    {

        yield return new WaitForSeconds(7.0f);
        train.Go = true;
        wagon.WagonSpeed = 0;

    }
    IEnumerator MoveOverTime(GameObject wagonObject, float ff)
    {
        isMoving = true;
        float elapsedTime = 0;
        Vector3 startPosition = wagonObject.transform.position;
        Vector3 endPosition = startPosition + wagonObject.transform.forward * ff;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            wagonObject.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        wagonObject.transform.position = endPosition;

        isMoving = false;
    }

}
