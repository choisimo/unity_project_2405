using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStartScene : MonoBehaviour
{
        public int maxHealth;
        public int currentHealth;
        public Transform attackTarget;
        public Animator animator;
        
        private Rigidbody _rigid;
        private CapsuleCollider _capsuleCollider;
        private Material _mat;
        UnityEngine.AI.NavMeshAgent _navgate;

        /**
          animator boolean settings
          */
        private bool isWalk1;
        private bool isDead;
        private bool takeDamage;
        private bool isAttack;

        public int attackDamange = 10;
        public float attackRange = 2.0f;
        public float attackRate = 1.0f;

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
            _navgate = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (_navgate)
            {
                Debug.Log("_navgate is not null");   
            }
            StartCoroutine(AttackRoutine());  // AttackRoutine 코루틴 시작
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
                _navgate.SetDestination(attackTarget.position);
                updateAnim();
            }
            else if (currentHealth > 0)
            {
                
            }

        }

        void OnTriggerEnter(Collider other)
        {       
            if (isDead)
            {
                return;
            }
            
            /*
            if (other.tag.Equals("Melee"))
            {
                weapon weapon = other.GetComponent<weapon>();
                currentHealth -= weapon.damage;
                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamange(reactVec));
                takeDamage = true;
            }
            else 
            */ 
            if (other.tag.Equals("Bullet"))
            {
                Ammo bullet = other.GetComponent<Ammo>();
                currentHealth -= bullet.damage;
                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamange(reactVec));
                takeDamage = true;
                
                SetAttackTarget(GameObject.FindWithTag("Player").transform);
            }
            takeDamage = false;
        }

        IEnumerator OnDamange(Vector3 reactVec)
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

        IEnumerator AttackRoutine()
        {
            while (!isDead)
            {
                if (attackTarget != null && Vector3.Distance(transform.position, attackTarget.position) <= attackRange)
                {
                    Attack();
                }

                isAttack = false;
                yield return new WaitForSeconds(attackRate);
            }
        }

        void Attack()
        {
            if (attackTarget != null)
            {
                PlayerController1 player = attackTarget.GetComponent<PlayerController1>();
                if (player != null)
                {
                    Debug.Log("Attack player !");
                    player.TakeDamange(attackDamange);
                    isAttack = true;
                }
            }
        }
        
        public void SetAttackTarget(Transform target)
        {
            attackTarget = target;
        }
}
