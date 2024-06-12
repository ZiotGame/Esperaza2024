using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemiesCount;
    private int waveNumber = 1;
    public GameObject powerUpPrefab;
    // Start is called before the first frame update
    void Start()
    {
        spawnEnemyWave(waveNumber);
        Instantiate(powerUpPrefab, GenerateRandomPosition(), powerUpPrefab.transform.rotation);
        
    }

    // Update is called once per frame
    void Update()
    {
        enemiesCount = FindObjectsOfType<Enemy>().Length;
        if (enemiesCount == 0)
        {
            waveNumber++;
            spawnEnemyWave(waveNumber);
            Instantiate(powerUpPrefab, GenerateRandomPosition(), powerUpPrefab.transform.rotation);
        }
    }
    void spawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateRandomPosition(), enemyPrefab.transform.rotation);
            
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        float randomX = Random.Range(-9, 9);
        float randomZ = Random.Range(-9, 9);
        Vector3 randomPos = new Vector3(randomX, 0, randomZ);
        return randomPos;
    }
}
