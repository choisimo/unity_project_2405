using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 생성할 적 프리팹
    public Transform playerTransform; // 플레이어의 위치
    public float spawnRadius = 10f; // 적 생성 반경
    public int enemyCount = 5; // 생성할 적의 수

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionAroundPlayer();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionAroundPlayer()
    {
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPoint.x, 0, randomPoint.y) + playerTransform.position;
        return spawnPosition;
    }

}
