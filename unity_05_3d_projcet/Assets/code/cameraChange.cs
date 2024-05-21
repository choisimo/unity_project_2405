using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondaryCamera;

    void Start()
    {
        // 초기 상태 설정: 메인 카메라 활성화, 보조 카메라 비활성화
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
    }

    void Update()
    {
        // 1번 키를 눌렀을 때 메인 카메라로 전환
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                SwitchToMainCamera();
            }
        }
        // 2번 키를 눌렀을 때 보조 카메라로 전환
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                SwitchToSecondaryCamera();
            }
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
