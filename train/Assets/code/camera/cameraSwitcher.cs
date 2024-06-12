using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondaryCamera;
    public Camera trainCamera;
    public Camera userCamera;
    
    void Start()
    {
        mainCamera.enabled = false;
        secondaryCamera.enabled = false;
        trainCamera.enabled = false;
        userCamera.enabled = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
               
                    SwitchToMainCamera();
                
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                
                    SwitchToSecondaryCamera();
                
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchToThirdCamera();
            }
            /**
            train 탑승 시 카메라 트레인으로 전환?
            */
        }
    }

    void SwitchToMainCamera()
    {
        mainCamera.enabled = true;
        userCamera.enabled = false;
        secondaryCamera.enabled = false;
        trainCamera.enabled = false;
    }

    void SwitchToSecondaryCamera()
    {
        mainCamera.enabled = false;
        userCamera.enabled = false;
        secondaryCamera.enabled = true;
        trainCamera.enabled = false;
    }

    void SwitchToThirdCamera()
    {
        mainCamera.enabled = false;
        secondaryCamera.enabled = false;
        userCamera.enabled = true;
        trainCamera.enabled = false;
    }

    public void SwitchToTrainCamera()
    {
        mainCamera.enabled = false;
        secondaryCamera.enabled = false;
        userCamera.enabled = false;
        trainCamera.enabled = true;
    }

}
