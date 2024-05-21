using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 720f; // 회전 속도 (도/초)
    public float jumpForce = 4f;
    public float jumpMoveSpeed = 10f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isJumping;


    Animator animator;
    Vector3 moveVec3;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // X, Z축 회전 고정
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        MovePlayer();
        Jump();
    }

    void MovePlayer()
    {
        if (!isGrounded) { Debug.Log("not grounded"); return ;}
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveVec3 = new Vector3(moveX, 0, moveZ).normalized;

        animator.SetBool("isWalk", moveVec3 != Vector3.zero);

        float currentMoveSpeed = isGrounded ? jumpMoveSpeed : moveSpeed;

        if (moveVec3 != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVec3);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            rb.MovePosition(transform.position + moveVec3 * currentMoveSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // 점프를 수행한 후에는 바닥에 있지 않음
            isJumping = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = true; // 바닥이나 벽에 닿아 있으면 바닥에 있는 상태로 설정
            isJumping = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = false; // 바닥이나 벽에서 벗어나면 바닥에 있지 않은 상태로 설정
        }
    }

    public void FootStep()
    {
        Debug.Log("footStep");
    }

}
