using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("spawnParameters")]
    public int enemiesPerWave = 5;
    public float spawnInterval = 5f;


    [Header("Control Flags")]
    public bool autoSpawn = true;
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
        if (timer <= 0f || isDeadAllEnemies)
        {
            enemySpawner.SpawnEnemies().Forget(); 
            timer = spawnInterval;

        }
    }
}
