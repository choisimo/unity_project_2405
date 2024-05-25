using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float turnSpeed = 720f; 
    public float jumpForce = 4f;
    public float jumpMoveSpeed = 10f;
    public GameObject[] weapons;
    public bool[] hasWeapons;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isJumping;
    private bool iDown;
    private bool fDown;
    private bool runDown;
    private bool isFireReady;
    private bool isSwap;
    
    private bool ItemSwapDown1;
    private bool ItemSwapDown2;
    private bool ItemSwapDown3;
    // item object
    GameObject nearObject;
    weapon equipWeapon;

    float fireDelay;
    Animator animator;
    Vector3 moveVec3;

    void GetInput()
    {
        iDown = Input.GetButtonDown("Interaction");
        fDown = Input.GetButtonDown("Fire1");
        runDown = Input.GetButtonDown("Run"); //running active
        ItemSwapDown1 = Input.GetButtonDown("ItemSwap1");
        ItemSwapDown2 = Input.GetButtonDown("ItemSwap2");
        ItemSwapDown3 = Input.GetButtonDown("ItemSwap3");
    }
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        //rb.useGravity = true;
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // X, Z�� ȸ�� ����
    }

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        GetInput();
        MovePlayer();
        Jump();
        Attack();
        Interaction();
        ItemSwap();
        //UpdateAnimator();
    }

    void MovePlayer()
    {
        if (!isGrounded) { 
            if (isJumping){
                Debug.Log("moving while jumping");
            } else {
                Debug.Log("not grounded"); return ;
            }
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveVec3 = new Vector3(moveX, 0, moveZ).normalized;


        animator.SetBool("isWalk", moveVec3 != Vector3.zero);
        animator.SetBool("isRun", runDown);

        float currentMoveSpeed = isGrounded ? jumpMoveSpeed : walkSpeed;

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
            isGrounded = false; 
            isJumping = true;
        }
    }

    void Attack(){
        if (equipWeapon == null){
            return;
        }

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isSwap){
            equipWeapon.Use();
            animator.SetTrigger("doSwing");
            fireDelay = 0;
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

/*
    void UpdateAnimator()
    {
        isJumping = false;
        float speed = moveVec3.magnitude * (isGrounded ? moveSpeed : jumpMoveSpeed);
        animator.SetFloat("Speed", speed);
    }
*/

    void ItemSwap()
    {
        isSwap = true;
        int weaponIndex = -1;
        if (ItemSwapDown1) weaponIndex = 0;
        if (ItemSwapDown2) weaponIndex = 1;
        if (ItemSwapDown3) weaponIndex = 2;

        if ((ItemSwapDown1 || ItemSwapDown2 || ItemSwapDown3) && !isJumping)
        {
            if (weaponIndex >= 0 && hasWeapons[weaponIndex])
            {
                if (equipWeapon != null)
                {
                    // Toggle off if the same weapon is selected
                    if (equipWeapon == weapons[weaponIndex])
                    {
                        equipWeapon.gameObject.SetActive(false);
                        equipWeapon = null;
                    }
                    else
                    {
                        equipWeapon.gameObject.SetActive(false);
                        equipWeapon = weapons[weaponIndex].GetComponent<weapon>();
                        equipWeapon.gameObject.SetActive(true);
                    }
                }
                else
                {
                    // Equip new weapon if none is currently equipped
                    equipWeapon = weapons[weaponIndex].GetComponent<weapon>();
                    equipWeapon.gameObject.SetActive(true);
                }
            }
        }
    }
    void Interaction()
    {
        if(iDown && nearObject != null){
            if (nearObject.CompareTag("weapon")){
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.itemIndex;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
                nearObject = null;
            }
        }
    }
    void OnTriggerStay(Collider other){
        if(other.tag == "weapon"){
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other){
        nearObject = null;
    }

    
}
