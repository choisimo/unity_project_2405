using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startSceneUserController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 10f;
    public Transform cameraTransform; // 카메라의 Transform을 참조

    private Rigidbody rb;
    private bool isGrounded;

    public float mouseSensitivity = 300f;
    private float xRotation = 0f;
    private float yRotation = 0f; // 추가된 변수: y축 회전 값을 저장

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        Jump();
        RotateView();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal"); // A와 D
        float moveZ = Input.GetAxis("Vertical");   // W와 S

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.y = 0f; // Y축 이동을 제거하여 플레이어가 공중으로 뜨는 것을 방지
        Vector3 moveVelocity = move.normalized * walkSpeed;

        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    private void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void RotateView()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX; // y축 회전 값을 누적

        // 카메라의 로컬 회전을 업데이트합니다.
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 플레이어의 몸체를 회전시킵니다.
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    private void LateUpdate()
    {
        // 카메라 위치를 플레이어의 위치에 고정합니다.
        cameraTransform.position = transform.position + new Vector3(0, 1.6f, 0); // 1.6f는 플레이어의 머리 높이
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
