    using TMPro;
    using Unity.VisualScripting;
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        // Player resources
        public int ammo;       // Amount of ammo the player has
        public int mineral;    // Amount of minerals the player has
        public int life;       // Player's life points
        public int fuel;
        

        /**
        * Speed-related variables
        */
        public float walkSpeed = 1f;   // Walking speed
        public float runSpeed = 2f;   // Running speed
        public float turnSpeed = 720f; // Turning speed (degrees per second)
        public float jumpForce = 4f;   // Force applied when jumping
        public float jumpMoveSpeed = 2f; // Movement speed while jumping
        
        /*********************************/

        // Weapon management
        public GameObject[] weapons;  // Array of weapon game objects
        public bool[] hasWeapons;     // Array indicating which weapons the player possesses

        // Private fields
        private Rigidbody rb;            // Rigidbody component for physics calculations
        private bool isGrounded;         // Whether the player is on the ground
        private bool isJumping;          // Whether the player is currently jumping
        private bool iDown;              // Input flag for interaction
        private bool fDown;              // Input flag for firing
        private bool runDown;            // Input flag for running
        private bool isFireReady;        // Whether the weapon is ready to fire
        private bool isSwap;             // Whether the player is swapping weapons
        private bool ItemSwapDown1;      // Input flag for weapon swap slot 1
        private bool ItemSwapDown2;      // Input flag for weapon swap slot 2
        private bool ItemSwapDown3;      // Input flag for weapon swap slot 3
        private bool isRifleEquip;
        // Item object
        GameObject nearObject;           // Reference to a nearby interactable object

        // Other variables
        float fireDelay;                 // Delay between firing
        Animator animator;               // Animator component for handling animations
        Vector3 moveVec3;                // Movement vector for player movement

        // class instance

        weapon equipWeapon;              // Currently equipped weapon


    /// <summary>
    /// functions 
    /// </summary>
    /// 
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
            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // X, Z�� ȸ�� ����
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
            UpdateAnimator();
        }

        void MovePlayer()
        {
            if (rb == null || animator == null)
            {
                Debug.LogError("Rigidbody 또는 Animator가 할당되지 않았습니다.");
                return;
            }

            if (!isGrounded) { 
                if (isJumping){
                    Debug.Log("moving while jumping");
                } else {
                    Debug.Log("not grounded"); return ;
                }
            }

            animator.SetBool("Grounded", true);
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            moveVec3 = new Vector3(moveX, 0, moveZ).normalized;


            animator.SetBool("isWalk", moveVec3 != Vector3.zero);
            animator.SetBool("isRun", runDown);

            float currentMoveSpeed = isGrounded ? jumpMoveSpeed : walkSpeed;

            if (moveVec3 == Vector3.zero){
                animator.SetFloat("Speed", 0f);
                animator.SetFloat("MotionSpeed", 0f);
            }

            if (moveVec3 != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveVec3);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                rb.MovePosition(transform.position + moveVec3 * currentMoveSpeed * Time.deltaTime);
                /**
                Idle: Speed = 0
                Walk_N: Speed = 0.1 ~ 0.5
                Run_N: Speed = 0.5 이상
                */
                if(!runDown){
                    animator.SetFloat("Speed", 0.3f);
                    animator.SetFloat("MotionSpeed", 10f);
                } else {
                    animator.SetFloat("Speed", 2f);
                    animator.SetFloat("MotionSpeed", 10f);
                }
                
            }
        }

        void Jump()
        {
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false; 
                isJumping = true;
                animator.SetBool("Jump", true);
            } else {
                animator.SetBool("Jump", false);
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
            if (equipWeapon.weapontype == weapon.weaponType.Melee) {
                animator.SetTrigger("doSwing");
            } else {
                animator.SetTrigger("doShot");
            }
            fireDelay = 0;
        }
    }

        void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("wall"))
            {
                isGrounded = true; // �ٴ��̳� ���� ��� ������ �ٴڿ� �ִ� ���·� ����
                isJumping = false;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("wall"))
            {
                isGrounded = false; // �ٴ��̳� ������ ����� �ٴڿ� ���� ���� ���·� ����
            }
        }

        public void FootStep()
        {
            Debug.Log("footStep");
        }


        void UpdateAnimator()
        {
            animator.SetBool("isRifleEquip", isRifleEquip);
        }


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
                            isRifleEquip = false;
                        }
                        else
                        {
                            equipWeapon.gameObject.SetActive(false);
                            equipWeapon = weapons[weaponIndex].GetComponent<weapon>();
                            equipWeapon.gameObject.SetActive(true);
                            isRifleEquip = true;

                        }
                    }
                    else
                    {
                        // Equip new weapon if none is currently equipped
                        equipWeapon = weapons[weaponIndex].GetComponent<weapon>();
                        equipWeapon.gameObject.SetActive(true);
                        isRifleEquip = true;
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

        void OnTriggerEnter(Collider other){
            if(other.tag == "item" ){
                Item item = other.GetComponent<Item>();
                switch(item.itemtype){
                    case Item.ItemType.Ammo:
                    ammo += item.num;
                        break;
                    case Item.ItemType.life:
                    life += item.num;
                        break;
                    case Item.ItemType.fuel:
                    fuel += item.num;
                        break;
                }
                Destroy(other.gameObject);
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
