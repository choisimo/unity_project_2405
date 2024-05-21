using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 720f; // ȸ�� �ӵ� (��/��)
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
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // X, Z�� ȸ�� ����
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
            isGrounded = false; // ������ ������ �Ŀ��� �ٴڿ� ���� ����
            isJumping = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = true; // �ٴ��̳� ���� ��� ������ �ٴڿ� �ִ� ���·� ����
            isJumping = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            isGrounded = false; // �ٴ��̳� ������ ����� �ٴڿ� ���� ���� ���·� ����
        }
    }

    public void FootStep()
    {
        Debug.Log("footStep");
    }

}
