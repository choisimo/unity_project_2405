using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class userController : MonoBehaviour
{

        // Player resources
        public int ammo;       // Amount of ammo the player has
        public int mineral;    // Amount of minerals the player has
        [Header("최대 목숨")]
        public int MaxHealth = 500;      
        [Header("현재 목숨")]
        public int currentHealth = 500;
        [Header("연료")]
        public int fuel;
        [Header("킬 수")]
        public int killCount = 0;
        [Header("UIManager")] public UIManager uiManager;

        /**
        * Speed-related variables
        */
        public float walkSpeed = 5f;   // Walking speed
        public float runSpeed = 10f;   // Running speed
        public float turnSpeed = 720f; // Turning speed (degrees per second)
        public float jumpForce = 10f;   // Force applied when jumping
        public float jumpMoveSpeed = 2f; // Movement speed while jumping
        
        /*********************************/

        // Weapon management
        public GameObject nearObject;           // Reference to a nearby interactable object
        public GameObject[] weapons;  // Array of weapon game objects
        public bool[] hasWeapons;     // Array indicating which weapons the player possesses
        
        // vehicle management
        public GameObject player;
        private GameObject vehicle;
        public Camera vehicleCamera;
        public bool isDriving;
        
        // Private fields
        private Rigidbody rb;            // Rigidbody component for physics calculations
        private bool isGrounded;         // Whether the player is on the ground
        private bool isJump;          // Whether the player is currently jumping
        private bool isJumpOnAir;
        private bool iDown;              // Input flag for interaction
        private bool fDown;              // Input flag for firing
        private bool runDown;            // Input flag for running
        private bool isFireReady;        // Whether the weapon is ready to fire
        private bool isSwap;             // Whether the player is swapping weapons
        private bool ItemSwapDown1;      // Input flag for weapon swap slot 1
        private bool ItemSwapDown2;      // Input flag for weapon swap slot 2
        private bool ItemSwapDown3;      // Input flag for weapon swap slot 3
        private bool isRifleEquip;
        private bool isWeaponPistol;
        private bool isOnDamage;
        private bool isDead;
        // Item object

        // Other variables
        float fireDelay;                 // Delay between firing
        Animator animator;               // Animator component for handling animations
        Vector3 moveVec3;                // Movement vector for player movement

        // class instance

        public weapon equipWeapon;              // Currently equipped weapon
        
        // camera
        public Camera mainCamera;
        
        // sound
        private AudioSource pistol1Sound;
        
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
            pistol1Sound = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; 
        }

        void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }
        void Update()
        {
            UpdateAnimator();
            GetInput();
            UpdateAnimator();
            MovePlayer();
            UpdateAnimator();
            Jump();
            Attack();
            UpdateAnimator();
            Interaction();
            UpdateAnimator();
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
            
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            moveVec3 = new Vector3(moveX, 0, moveZ).normalized;

            animator.SetBool("isWalk", moveVec3 != Vector3.zero);
            animator.SetBool("isRun", runDown);

            // 현재 이동 속도를 설정합니다.
            float currentMoveSpeed;
            if (isJump){
                currentMoveSpeed = jumpMoveSpeed;
            }
            else if (runDown)
            {
                currentMoveSpeed = 20f;
            }
            else
            {
                currentMoveSpeed = 10f;
            }

            Debug.Log($"Current move speed: {currentMoveSpeed}");

            if (moveVec3 == Vector3.zero)
            {
                animator.SetFloat("Speed", 0f);
            }
            else if (moveVec3 != Vector3.zero && !isJumpOnAir)
            {
                Debug.Log("player가 걷는 중입니다.");
                Quaternion targetRotation = Quaternion.LookRotation(moveVec3);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                rb.MovePosition(transform.position + moveVec3 * currentMoveSpeed * Time.deltaTime);

                if (!runDown)
                {
                    animator.SetFloat("Speed", 9f);
                }
                else
                {
                    animator.SetFloat("Speed", 19f);
                }
            } else {
                Quaternion targetRotation = Quaternion.LookRotation(moveVec3);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                rb.MovePosition(transform.position + moveVec3 * 5f * Time.deltaTime);
                
            }
        }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJump = true;
            isJumpOnAir = true;
        } else {
            isJump = false;
        }
    }


    void Attack(){
        if (equipWeapon == null){
            Debug.Log("No weapon equipped");
            return;
        }
        
        Debug.Log("Attack attempted");

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
        isFireReady = true;
        if (fDown){
            Debug.Log("Fire1 input executed");
        }

        if (fDown && isFireReady && !isSwap)
        {   
            Debug.Log("start Attack");
            
            // mouse click position 
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 targetPoint;
            
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(100); // 마우스 클릭 위치가 없을 경우
            }
            
            // 플레이어를 마우스 클릭 방향으로 회전
            Vector3 direction = (targetPoint - transform.position).normalized;
            direction.y = 0; // Y축 고정
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 720f);
            }
            
            equipWeapon.Use();
            if (equipWeapon.weapontype == weapon.weaponType.Melee) {
                animator.SetTrigger("doSwing");
            } else {
                if (equipWeapon.GetComponent<Item>().itemIndex == 2)
                {
                    isWeaponPistol = true;
                }
                else
                {
                    isWeaponPistol = false;
                }
                animator.SetTrigger("doShot");
            }
            fireDelay = 0;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || 
            collision.gameObject.CompareTag("wall"))
        {
            Debug.Log("Ground Tag detected");
            isGrounded = true;
            isJump = false;
            isJumpOnAir = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("wall"))
        {
            isGrounded = false;
            isJump = true;
            isJumpOnAir = true;
        }
    }


        public void FootStep()
        {
            Debug.Log("footStep");
        }


        void UpdateAnimator()
        {
            animator.SetBool("Grounded", isGrounded);
            animator.SetBool("isRifleEquip", isRifleEquip);
            animator.SetBool("isSwap", isSwap);
            animator.SetBool("isJump", isJump);
            animator.SetBool("isJumpOnAir", isJumpOnAir);
            animator.SetBool("isWeaponPistol", isWeaponPistol);
            animator.SetBool("isOnDamage", isOnDamage);
            animator.SetBool("isDead", isDead);
        }


        void ItemSwap() 
        {
            isSwap = true;
            int weaponIndex = -1;
            if (ItemSwapDown1) weaponIndex = 0;
            if (ItemSwapDown2) weaponIndex = 1;
            if (ItemSwapDown3) weaponIndex = 2;

            if ((ItemSwapDown1 || ItemSwapDown2 || ItemSwapDown3) && !isJump)
            {
                if (weaponIndex >= 0 && hasWeapons[weaponIndex])
                {
                    if (weaponIndex == 2)
                    {
                        isWeaponPistol = true;
                    }
                    else
                    {
                        isWeaponPistol = false;
                    }
                    if (equipWeapon != null)
                    {
                        // Toggle off if the same weapon is selected
                        if (equipWeapon == weapons[weaponIndex].GetComponent<weapon>())
                        {
                            equipWeapon.gameObject.SetActive(false);
                            equipWeapon.enabled = false; // 스크립트 비활성화
                            equipWeapon = null;
                            isRifleEquip = false;
                            isSwap = false;
                        }
                        else
                        {
                            equipWeapon.gameObject.SetActive(false);
                            equipWeapon.enabled = false; // 스크립트 비활성화
                            equipWeapon = weapons[weaponIndex].GetComponent<weapon>();
                            equipWeapon.gameObject.SetActive(true);
                            equipWeapon.enabled = true; // 스크립트 활성화
                            isRifleEquip = true;
                            isSwap = false;
                        }
                    }
                    else
                    {
                        // Equip new weapon if none is currently equipped
                        equipWeapon = weapons[weaponIndex].GetComponent<weapon>();
                        equipWeapon.gameObject.SetActive(true);
                        equipWeapon.enabled = true; // 스크립트 활성화
                        isRifleEquip = true;
                        isSwap = false;
                    }
                    
                    
                    Item currentItem = weapons[weaponIndex].GetComponent<Item>();
                    if (currentItem != null && currentItem.itemName.Contains("pistol"))
                    {
                        Debug.Log("pistol을 장착하였습니다.");
                        isWeaponPistol = true;
                    }

                }
            }

            isSwap = false;
        }
        void Interaction()
        {
            if(iDown && nearObject != null){
                if (nearObject.CompareTag("weapon")){
                    Item item = nearObject.GetComponent<Item>();
                    if (item == null)
                    {
                        Debug.LogError("nearObject 에 Item 컴포넌트가 없습니다.");
                    }
                    else
                    {
                        if (item.itemtype == Item.ItemType.weapon)
                        {
                            int weaponIndex = item.itemIndex;
                            hasWeapons[weaponIndex] = true;   
                            Destroy(nearObject);
                            nearObject = null;
                        } else
                        {
                            Debug.LogError("nearObject 의 ItemType 이 weapon 이 아닙니다.");
                        }
                    }
                } else if (nearObject.CompareTag("train"))
                {
                    Debug.Log("near object tag is TRAIN");
                    if (isDriving)
                    {
                        Debug.Log("exit car");
                        ExitCar();
                    }
                    else
                    {
                        Debug.Log("enter car");
                        EnterCar();
                    }
                } 
                
            }
            else if (nearObject == null)
            {
                Debug.Log("nearObject 가 null 입니다.");
            }
        }
        
        IEnumerator OnDamange()
        {
            isOnDamage = true;
            yield return new WaitForSeconds(1f);
            isOnDamage = false;
        }

        void OnTriggerStay(Collider other){
            if(other.tag == "weapon"){
                nearObject = other.gameObject;
            } else if (other.tag == "tools"){
                nearObject = other.gameObject;
            }
        }

        void OnTriggerExit(Collider other){
            nearObject = null;
        }

        void FreezeRotation()
        {
            rb.angularVelocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            FreezeRotation();
        }

        public void TakeDamange(int damage)
        {
            
            if (currentHealth <= 0)
            {
                Debug.Log("남은 목숨이 없어 사망하였습니다.");
                isDead = true;
            }
            currentHealth -= damage;
            isOnDamage = true;
            rb.isKinematic = true;
            StartCoroutine(ResetKinematic());
            uiManager.UpdateHealthUI();
        }

        IEnumerator ResetKinematic()
        {
            yield return new WaitForSeconds(1f);
            rb.isKinematic = false;
            isOnDamage = false;
        }
        
        /*
         * car interaction
         */
        
        
        public void EnterCar()
        {
            Debug.Log("enter car"); 
            isDriving = true;
            vehicle.GetComponent<CarController>().SetDriving(true);
            gameObject.SetActive(false);

        }

        public void ExitCar()
        {
            Debug.Log("exit car");
            isDriving = false;
            if (vehicle != null)
            {
                vehicle.GetComponent<CarController>().SetDriving(false);
            }
            gameObject.SetActive(true);
            transform.position = vehicle.transform.position + new Vector3(2, 0, 0); // 차량 근처에 위치시키기
            Debug.Log("player exited the car and is now at position : " + transform.position);
        
    }
}
