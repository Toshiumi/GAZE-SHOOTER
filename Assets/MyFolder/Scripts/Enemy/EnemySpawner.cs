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
            Debug.LogWarning("�G���ݒ肳��Ă��܂���");
            return;
        }

        for(int i = 0; i < numOfEnemies; i++)
        {
            //�����_���ȕ����x�N�g��
            Vector3 randomDir = Random.onUnitSphere;

            //�X�|�[���ʒu�v�Z
            Vector3 spawnPos = playerTransform.position + randomDir * spawnRadius;
            spawnPos.y += spawnHeightOffset;

            //�G�̐���
            Instantiate(enemyPrefab, spawnPos, Quaternion.LookRotation(-randomDir));

            await UniTask.Delay(TimeSpan.FromSeconds(spawnWaitTime));

        }
    }
}
