using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectUser : MonoBehaviour
{
    public EnemyStartScene enemy; // 감지된 적 오브젝트 참조

    private void Start()
    {
        if (enemy == null)
        {
            enemy = GetComponentInParent<EnemyStartScene>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected");
            enemy.SetAttackTarget(other.transform); // 감지된 플레이어를 타겟으로 설정
        }
    }

}
