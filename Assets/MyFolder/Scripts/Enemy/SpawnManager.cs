using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("spawnParameters")]
    public int enemiesPerWave = 5;
    public float spawnInterval = 5f;
    [HideInInspector] public bool hasSpawnedInitially = false;



    [Header("Control Flags")]
    public bool autoSpawn = false;
    public bool isDeadAllEnemies = false;

    private float timer;

    public EnemySpawner enemySpawner;
    


    void Start()
    {
        timer = spawnInterval;
        
    }

    void Update()
    {
        if (!autoSpawn) return;


        timer -= Time.deltaTime;
        isDeadAllEnemies = GameManager.Instance.AreAllEnemiesDefeated();

        // 1��ځF���X�|�[��
        if (!hasSpawnedInitially)
        {
            Debug.Log("pe");

            enemySpawner.SpawnEnemies().Forget();
            hasSpawnedInitially = true;
            //timer = spawnInterval;
            return;
        }

        // �ʏ�X�|�[��
        if (timer <= 0f || (isDeadAllEnemies && hasSpawnedInitially))
        {
            Debug.Log("ges");

            enemySpawner.SpawnEnemies().Forget();
            timer = spawnInterval;
        }
    }

    public void TimeSpanChanged(float gameTimer, float gameDuration)
    {
        // ���Ԍo�߂ɉ����ăX�|�[���Ԋu��Z�k�i��: 30�b�Ŕ����ɂȂ�j
        float t = 1f - (gameTimer / gameDuration);
        spawnInterval = Mathf.Lerp(5f, 1.5f, t); // ����5�b �� �ŏ�1.5�b
    }
}
