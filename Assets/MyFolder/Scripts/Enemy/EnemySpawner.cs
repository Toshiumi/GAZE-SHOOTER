using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyEntry
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float weight;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform playerTransform;
    public List<EnemyEntry> enemyEntries;
    public int numOfEnemies = 5;
    public float spawnHeightOffset = 0f;
    public float spawnWaitTime = 0.2f;

    [Header("Spawn Area")]
    public float minDistance = 5f;
    public float maxDistance = 10f;
    public float maxHorizontalAngle = 50f;
    public float minVerticalAngle = 20f;
    public float maxVerticalAngle = 70f;

    public SpawnManager spawnManger;


    public async UniTaskVoid SpawnEnemies()
    {
        if (enemyEntries == null || enemyEntries.Count == 0 || playerTransform == null)
        {
            Debug.LogWarning("敵の設定が不正です");
            return;
        }

        for (int i = 0; i < numOfEnemies; i++)
        {
            Vector3 spawnPos = GetRandomSpawnPosition();

            GameObject prefab = ChooseEnemyByWeight();
            if (prefab == null)
            {
                Debug.LogWarning("敵の選択に失敗しました");
                continue;
            }

            if (spawnManger.autoSpawn)
            {
                Instantiate(prefab, spawnPos, Quaternion.identity);
                GameManager.Instance.RegisterEnemy();
            }
            

            await UniTask.Delay(TimeSpan.FromSeconds(spawnWaitTime));
        }
    }

    private GameObject ChooseEnemyByWeight()
    {
        float totalWeight = 0f;
        foreach (var entry in enemyEntries)
        {
            totalWeight += entry.weight;
        }

        float rand = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in enemyEntries)
        {
            cumulative += entry.weight;
            if (rand <= cumulative)
                return entry.prefab;
        }

        return null;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float distance = Random.Range(minDistance, maxDistance);
        float pitch = Random.Range(-minVerticalAngle, -maxVerticalAngle);  // X軸（上下方向）
        float yaw = Random.Range(-maxHorizontalAngle, maxHorizontalAngle); // Y軸（左右方向）

        // プレイヤーの正面を基準に、ピッチとヨーを加えた方向ベクトルを作る
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 direction = playerTransform.rotation * (rotation * Vector3.forward);

        Vector3 spawnPos = playerTransform.position + direction.normalized * distance;
        return spawnPos;
    }
}
