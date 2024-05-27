using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondaryCamera;
    public Camera trainCamera;

    void Start()
    {
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
        trainCamera.enabled = false;
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
            /**
            train 탑승 시 카메라 트레인으로 전환?
            */
        }
    }

    void SwitchToMainCamera()
    {
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
    }

    void SwitchToSecondaryCamera()
    {
        mainCamera.enabled = false;
        secondaryCamera.enabled = true;
    }
}
