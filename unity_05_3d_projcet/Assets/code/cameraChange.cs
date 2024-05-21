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
        // �ʱ� ���� ����: ���� ī�޶� Ȱ��ȭ, ���� ī�޶� ��Ȱ��ȭ
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
    }

    void Update()
    {
        // 1�� Ű�� ������ �� ���� ī�޶�� ��ȯ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                SwitchToMainCamera();
            }
        }
        // 2�� Ű�� ������ �� ���� ī�޶�� ��ȯ
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
