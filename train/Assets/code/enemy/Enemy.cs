using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private bool isPlayerInRange;

    public Transform attackTarget;
    public Animator animator;

    private Rigidbody _rigid;
    private CapsuleCollider _capsuleCollider;
    private Material _mat;

    NavMeshAgent _navgate;

    // Animator boolean settings
    private bool isWalk1;
    private bool isDead;
    private bool takeDamage;
    private bool isAttack;

    public int attackDamage = 10;
    public float attackRange = 2.0f;
    public float attackRate = 1.0f;
    private float nextAttackTime;



    void updateAnim()
    {
        if (_navgate.velocity.magnitude > 0.1f)
        {
            animator.SetBool("Walk_Cycle_1", isWalk1);
        }
        animator.SetBool("Die", isDead);
        animator.SetBool("Take_Damage_1", takeDamage);
        animator.SetBool("Attack_1", isAttack);
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        _navgate = GetComponent<NavMeshAgent>();
        if (_navgate)
        {
            Debug.Log("_navgate is not null");
        }
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            attackTarget = player.transform;
        }
        else
        {
            Debug.LogError("Player not found. Ensure the player has the tag 'Player'.");
        }
    }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _mat = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (attackTarget != null && currentHealth > 0)
        {
            float distanceToPlayer = Vector3.Distance(attackTarget.position, transform.position);
            if (distanceToPlayer <= attackRange)
            {
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
            else
            {
                _navgate.SetDestination(attackTarget.position);
            }
            updateAnim();
        }
        else if (currentHealth > 0)
        {
            Debug.LogError("attackTarget이 할당되지 않았습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }

        if (other.CompareTag("Bullet"))
        {
            Ammo bullet = other.GetComponent<Ammo>();
            if (bullet != null)
            {
                currentHealth -= bullet.damage;
                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamage(reactVec));
                takeDamage = true;
            }
            takeDamage = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private IEnumerator OnDamage(Vector3 reactVec)
    {
        _mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (currentHealth > 0)
        {
            _mat.color = Color.white;
        }
        else
        {
            _mat.color = Color.yellow;
            isDead = true;
            _navgate.isStopped = true;
            _navgate.enabled = false;
            updateAnim();
            gameObject.layer = 10;

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            _rigid.AddForce(reactVec * 2, ForceMode.Impulse);

            Destroy(gameObject, 10);
        }
    }

    private void Attack()
    {
        if (attackTarget != null)
        {
            userController player = attackTarget.GetComponent<userController>();
            if (player != null)
            {
                Debug.Log("Attack player!");
                player.TakeDamange(attackDamage);
                isAttack = true;
                updateAnim();
                StartCoroutine(ResetAttackTrigger());
            }
        }
    }

    private IEnumerator ResetAttackTrigger()
    {
        yield return new WaitForSeconds(0.1f);
        isAttack = false;
        updateAnim();
    }
}
