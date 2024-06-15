using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonUITrigger : MonoBehaviour
{

    public string targetTag = "train";
    public GameObject panel; // 활성화할 패널
    public GameObject[] TrainText;
    public GameObject[] TrainName;

    public GameObject Camera_Main;
    public GameObject Camera_Sub;
    public GameObject[] wpA;
    public GameObject[] wpB;
    int randomIndex;
    private Train train;

    private void OnTriggerEnter(Collider other)
    {
        randomIndex = StaticVal.Instance.GetRandomIndex();

        if (other.CompareTag(targetTag))
        {
            // 패널 활성화
            if (!panel.activeSelf)
            {
                panel.SetActive(true);
                TrainText[randomIndex].SetActive(true);
                TrainName[randomIndex].SetActive(true);
                Camera_Main.SetActive(false);
                Camera_Sub.SetActive(true);
            }

            train = other.GetComponent<Train>();
        }
    }
    public void PushYesButton()
    {
        TrainText[randomIndex].SetActive(false);
        TrainName[randomIndex].SetActive(false);
        panel.SetActive(false);
 
        Camera_Main.SetActive(true);
        Camera_Sub.SetActive(false);
        
        foreach (GameObject point in wpA)
        {
            point.SetActive(true);
        }
        foreach (GameObject point in wpB)
        {
            point.SetActive(false);
        }

        train.Go = true;

        //2번 트리거 되는거 방지
        gameObject.SetActive(false);
    }
    public void PushNoButton()
    {
        panel.SetActive(false);
        TrainText[randomIndex].SetActive(false);
        Camera_Main.SetActive(true);
        Camera_Sub.SetActive(false);

        foreach (GameObject point in wpA)
        {
            point.SetActive(false);
        }
        foreach (GameObject point in wpB)
        {
            point.SetActive(true);
        }

        train.Go = true;

        gameObject.SetActive(false);
    }
    

}
