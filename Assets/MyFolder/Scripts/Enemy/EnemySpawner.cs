using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform playerTransform;
    public GameObject enemyPrefab;
    public int numOfEnemies = 5;
    public float spawnRadius = 5f;
    public float spawnHeightOffset = 0f;
    public float spawnWaitTime = 0.2f;


    public async UniTaskVoid SpawnEnemies()
    {
        if(enemyPrefab == null || playerTransform == null)
        {
            Debug.LogWarning("敵が設定されていません");
            return;
        }

        for(int i = 0; i < numOfEnemies; i++)
        {
            //ランダムな方向ベクトル
            Vector3 randomDir = Random.onUnitSphere;

            //スポーン位置計算
            Vector3 spawnPos = playerTransform.position + randomDir * spawnRadius;
            spawnPos.y += spawnHeightOffset;

            //敵の生成
            Instantiate(enemyPrefab, spawnPos, Quaternion.LookRotation(-randomDir));

            await UniTask.Delay(TimeSpan.FromSeconds(spawnWaitTime));

        }
    }
}
