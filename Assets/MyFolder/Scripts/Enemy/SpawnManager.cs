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

        // 1回目：即スポーン
        if (!hasSpawnedInitially)
        {
            Debug.Log("pe");

            enemySpawner.SpawnEnemies().Forget();
            hasSpawnedInitially = true;
            //timer = spawnInterval;
            return;
        }

        // 通常スポーン
        if (timer <= 0f || (isDeadAllEnemies && hasSpawnedInitially))
        {
            Debug.Log("ges");

            enemySpawner.SpawnEnemies().Forget();
            timer = spawnInterval;
        }
    }

    public void TimeSpanChanged(float gameTimer, float gameDuration)
    {
        // 時間経過に応じてスポーン間隔を短縮（例: 30秒で半分になる）
        float t = 1f - (gameTimer / gameDuration);
        spawnInterval = Mathf.Lerp(5f, 1.5f, t); // 初期5秒 → 最小1.5秒
    }
}
