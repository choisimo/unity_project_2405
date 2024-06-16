using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startSceneUserController : MonoBehaviour
{
    
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
    
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 10f;
    public Transform cameraTransform; // 카메라의 Transform을 참조
    public Image crosshair; // UI 조준점을 참조
    public Transform aimPoint; // 조준점의 Transform을 참조

    private Rigidbody rb;
    private bool isGrounded;

    public float mouseSensitivity = 300f;
    private float xRotation = 0f;
    private float yRotation = 0f; // 추가된 변수: y축 회전 값을 저장
    
    private float cameraHeight = 2f; // 플레이어의 머리 높이
    private float cameraDistance = 0f; // 카메라를 플레이어 앞쪽으로 이동시키는 거리

    // Weapon management
    public GameObject[] weapons;  // Array of weapon game objects
    public bool[] hasWeapons;     // Array indicating which weapons the player possesses
    private bool isSwap;             // Whether the player is swapping weapons
    private bool isFireReady;        // Whether the weapon is ready to fire
    private float fireDelay;         // Delay between firing
    private bool fDown;              // Input flag for firing
    private bool ItemSwapDown1;      // Input flag for weapon swap slot 1
    private bool ItemSwapDown2;      // Input flag for weapon swap slot 2
    private bool ItemSwapDown3;      // Input flag for weapon swap slot 3
    private bool isRifleEquip;
    private bool isWeaponPistol;
    private startSceneWeapon equipWeapon;              // Currently equipped weapon
    public Camera mainCamera;
    public Animator animator;               // Animator component for handling animations
    // Private fields
    private bool isJump;          // Whether the player is currently jumping
    private bool isJumpOnAir;
    private bool iDown;              // Input flag for interaction
    private bool runDown;            // Input flag for running
    private bool isOnDamage;
    private bool isDead;
    private bool isStunned;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        LockCursor();
        ResetPlayerPositionAndRotation();
        InitializeRigidbody();
        // 조준점을 화면 중앙에 배치합니다.
        if (crosshair != null)
        {
            crosshair.rectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager not found in the scene.");
            }
        }
    }

    private void Update()
    {
        Move();
        Jump();
        RotateView();
        GetInput();
        Attack();
        ItemSwap();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
            }
        }
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal"); // A와 D
        float moveZ = Input.GetAxis("Vertical");   // W와 S

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.y = 0f; // Y축 이동을 제거하여 플레이어가 공중으로 뜨는 것을 방지

        // Transform을 사용하여 위치를 변경합니다.
        transform.Translate(move * walkSpeed * Time.deltaTime, Space.World);

        /*Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.y = 0f; // Y축 이동을 제거하여 플레이어가 공중으로 뜨는 것을 방지
        Vector3 moveVelocity = move.normalized * walkSpeed;

        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);*/
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

        // aimPoint를 crosshair 위치로 업데이트
        if (aimPoint != null)
        {
            Vector3 aimPointScreenPosition = new Vector3(Screen.width / 2, Screen.height / 2, mainCamera.nearClipPlane);
            aimPoint.position = mainCamera.ScreenToWorldPoint(aimPointScreenPosition);
        }
    }

    private void LateUpdate()
    {
        // 카메라 위치를 플레이어의 위치 앞쪽에 고정합니다.
        Vector3 cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);
        cameraTransform.position = transform.position + transform.rotation * cameraOffset;
        // 카메라의 회전을 플레이어의 회전과 동일하게 설정합니다.
        cameraTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
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
    
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ResetPlayerPositionAndRotation()
    {
        // 캐릭터의 초기 위치와 회전을 설정합니다.
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    void GetInput()
    {
        fDown = Input.GetButtonDown("Fire1");
        ItemSwapDown1 = Input.GetButtonDown("ItemSwap1");
        ItemSwapDown2 = Input.GetButtonDown("ItemSwap2");
        ItemSwapDown3 = Input.GetButtonDown("ItemSwap3");
    }

    void Attack()
    {
        if (equipWeapon == null)
        {
            Debug.Log("No weapon equipped");
            return;
        }

        Debug.Log("Attack attempted");

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isSwap)
        {
            if (ammo > 0) // 탄약이 있는지 확인
            {
                Debug.Log("start Attack");

                equipWeapon.Use();
                if (equipWeapon.weapontype == startSceneWeapon.weaponType.Melee) {
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
                    Debug.Log("animator doShot");
                }
                ammo--; // 총을 쏜 후 탄약 감소
                //uiManager.UpdateAmmoUI(ammo); // UI 업데이트
                fireDelay = 0;
            }
            else
            {
                Debug.Log("No ammo left");
            }
        }
    }


    void ItemSwap() 
    {
        isSwap = true;
        int weaponIndex = -1;
        if (ItemSwapDown1) weaponIndex = 0;
        if (ItemSwapDown2) weaponIndex = 1;
        if (ItemSwapDown3) weaponIndex = 2;

        if (ItemSwapDown1 || ItemSwapDown2 || ItemSwapDown3)
        {
            if (weaponIndex >= 0 && weaponIndex < weapons.Length && hasWeapons[weaponIndex])
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
                    if (equipWeapon == weapons[weaponIndex].GetComponent<startSceneWeapon>())
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
                        equipWeapon = weapons[weaponIndex].GetComponent<startSceneWeapon>();
                        equipWeapon.gameObject.SetActive(true);
                        equipWeapon.enabled = true; // 스크립트 활성화
                        isRifleEquip = true;
                        isSwap = false;
                    }
                }
                else
                {
                    // Equip new weapon if none is currently equipped
                    equipWeapon = weapons[weaponIndex].GetComponent<startSceneWeapon>();
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
    
    private void InitializeRigidbody()
    {
        // Rigidbody 초기화
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
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
        StartCoroutine(StunPlayer(1.0f));
    }
    
    IEnumerator StunPlayer(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }
        
    IEnumerator ResetKinematic()
    {
        yield return new WaitForSeconds(1f);
        rb.isKinematic = false;
        isOnDamage = false;
    }
}
