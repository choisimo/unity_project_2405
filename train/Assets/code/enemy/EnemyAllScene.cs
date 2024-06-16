using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAllScene : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Transform attackTarget;
    public Animator animator;

    private Rigidbody _rigid;
    private CapsuleCollider _capsuleCollider;
    private Material _mat;
    public UnityEngine.AI.NavMeshAgent _navgate;

    private bool isWalk1;
    private bool isDead;
    private bool takeDamage;
    private bool isAttack;

    public int attackDamage = 10;
    public float attackRange = 2.0f;
    public float attackRate = 1.0f;

    private int walkCycleIndex;
    private int attackIndex;

    public List<MonoBehaviour> players; // 여러 플레이어 컨트롤러를 받을 리스트

    void updateAnim()
    {
        // 무작위로 Walk Cycle 선택
        walkCycleIndex = Random.Range(1, 3); // 1, 2 중 무작위로 선택
        animator.SetBool($"Walk_Cycle_{walkCycleIndex}", isWalk1);

        animator.SetBool("Die", isDead);
        animator.SetBool("Take_Damage_1", takeDamage);

        // 무작위로 Attack 선택
        attackIndex = Random.Range(1, 6); // 1, 2, 3, 4, 5 중 무작위로 선택
        animator.SetBool($"Attack_{attackIndex}", isAttack);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        _navgate = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (_navgate)
        {
            Debug.Log("_navgate is not null");
        }

        StartCoroutine(AttackRoutine());
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

        FindClosestPlayer(); // 가장 가까운 플레이어 찾기

        if (attackTarget != null && currentHealth > 0)
        {
            _navgate.SetDestination(attackTarget.position);
            isWalk1 = _navgate.velocity.magnitude > 0.1f;
            updateAnim();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead)
        {
            return;
        }

        if (other.tag.Equals("Bullet"))
        {
            Ammo bullet = other.GetComponent<Ammo>();
            if (bullet != null)
            {
                currentHealth -= bullet.damage;
                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamage(reactVec));
                takeDamage = true;

                SetAttackTarget(GameObject.FindWithTag("Player").transform);
            }
        }
        else
        {
            MonoBehaviour playerComponent = other.GetComponent<MonoBehaviour>();
            if (playerComponent is IPlayerController)
            {
                SetAttackTarget(other.transform);
            }
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
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

            yield return new WaitForSeconds(attackRate);
        }
    }

    void Attack()
    {
        if (attackTarget != null)
        {
            var playerComponent = attackTarget.GetComponent<IPlayerController>();
            if (playerComponent != null)
            {
                Debug.Log("Attack player!");
                playerComponent.TakeDamage(attackDamage);
                isAttack = true;
                updateAnim();
                StartCoroutine(ResetAttackState()); // Attack 상태 리셋을 위한 코루틴 실행
            }
        }
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(0.5f); // 공격 애니메이션 실행을 위한 시간 대기
        isAttack = false; // Attack 상태 리셋
        updateAnim(); // 애니메이션 업데이트
    }

    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    // 가장 가까운 플레이어를 찾는 함수
    void FindClosestPlayer()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (var player in players)
        {
            float distance = Vector3.Distance(transform.position, ((MonoBehaviour)player).transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = ((MonoBehaviour)player).transform;
            }
        }

        if (closestPlayer != null)
        {
            attackTarget = closestPlayer;
        }
    }

    // 여러 플레이어를 설정하는 함수
    public void SetPlayers(List<MonoBehaviour> playerList)
    {
        players = playerList;
    }
}

public interface IPlayerController
{
    void TakeDamage(int damage);
}
