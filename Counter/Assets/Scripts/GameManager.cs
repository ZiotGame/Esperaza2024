using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject[] objectToSpawn;
    // Define weights for each object
    public int[] weights;
    public Vector3 spawnRange = new(0, 10, 10);
    public int numberOfSpawn;
    public int rangeYMin = 6;
    public int rangeYMax = 20;
    public float minSpawnTime = 1.0f;
    public float maxSpawnTime = 5.0f;
    public GameObject[] obstacleToSpawn;
    public Vector3 spawnObstacleRange = new(0, 0, 3.5f);
    public float rangeObstacleYMin;
    public float rangeObstacleYMax;

    public bool isGameActive = true;
    public int spawned;
    private float timeToNextSpawn;
    private int objectArrayId;
    private int obstacleArradyId;


    // Start is called before the first frame update
    void Start()
    {
        SetRandomTimeToNextSpawn();
        SetRandomObstacleToSpawn();
        spawned = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeToNextSpawn -= Time.deltaTime;
        if (timeToNextSpawn <= 0)
        {
            SetRandomObjectToSpawn();
            SpawnObject();
            SetRandomTimeToNextSpawn();
        }
    }
    void SetRandomObjectToSpawn()
    {

        objectArrayId = GetWeightedRandomIndex(weights);
        // Debug.Log("objectArrayId:" + objectArrayId);
        //objectArrayId = Random.Range(0,objectToSpawn.Length);
        //Debug.Log("Selected Object ID: " + objectArrayId + " (Prefab: " + objectToSpawn[objectArrayId].name + ")");
    }
    void SetRandomObstacleToSpawn()
    {
        for (int i = 0; i < obstacleToSpawn.Length; i++)
        {
            Vector3 randomPosition = new(
                spawnObstacleRange.x,
                Random.Range(rangeObstacleYMin, rangeObstacleYMax),
                Random.Range(-spawnObstacleRange.z, spawnObstacleRange.z)
                );
            obstacleArradyId = Random.Range(0, obstacleToSpawn.Length);

            Instantiate(obstacleToSpawn[obstacleArradyId], randomPosition, Quaternion.identity);
        }
    }
    void SetRandomTimeToNextSpawn()
    {
        timeToNextSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnObject()
    {
        if (spawned < numberOfSpawn)
        {
            Vector3 randomPosition = new(
                Random.Range(-spawnRange.x, spawnRange.x),
                Random.Range(rangeYMin, rangeYMax),
                Random.Range(-spawnRange.z, spawnRange.z)
                );
            //Debug.Log("Spawning Object ID: " + objectArrayId + " at Position: " + randomPosition);
            Instantiate(objectToSpawn[objectArrayId], randomPosition, Quaternion.identity);
            spawned++;
            //Debug.Log("Number of Spawns:" + spawned);
        }

    }
    int GetWeightedRandomIndex(int[] weights)
    {
        int totalWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];
        }

        int randomWeight = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomWeight < cumulativeWeight)
            {
                return i;
            }
        }

        // Fallback in case something goes wrong
        return 0;
    }
    public void GameEnd()
    {
        Debug.Log("Game End");
        isGameActive = false;
    }
    public void RestartGame()
    {

        Debug.Log("Game Restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
