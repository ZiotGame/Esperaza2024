using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] objectToSpawn;
    // Define weights for each object
    public int[] weights;
    public Vector3 spawnRange = new(0,10,10);
    public int rangeYMin = 6;
    public int rangeYMax = 20;
    public float minSpawnTime = 1.0f;
    public float maxSpawnTime = 5.0f;
    public GameObject[] obstacleToSpawn;
    public Vector3 spawnObstacleRange = new(0, 0, 3.5f);
    public float rangeObstacleYMin;
    public float rangeObstacleYMax;

    private float timeToNextSpawn;
    private int objectArrayId;
    private int obstacleArradyId;


    // Start is called before the first frame update
    void Start()
    {
        SetRandomTimeToNextSpawn();
        SetRandomObstacleToSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        timeToNextSpawn -= Time.deltaTime;
        if (timeToNextSpawn <=0)
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
        for (int i=0; i < obstacleToSpawn.Length; i++) {
            Vector3 randomPosition = new(
                Random.Range(-spawnObstacleRange.x, spawnObstacleRange.x),
                Random.Range(rangeObstacleYMin, rangeObstacleYMax),
                Random.Range(-spawnObstacleRange.z, spawnObstacleRange.z)
                ) ;
        obstacleArradyId = Random.Range(0, obstacleToSpawn.Length);

        Instantiate(obstacleToSpawn[obstacleArradyId], randomPosition, Quaternion.identity);
            Debug.Log(i);
        }
    }
    void SetRandomTimeToNextSpawn()
    {
        timeToNextSpawn = Random.Range(minSpawnTime,maxSpawnTime);
    }

    void SpawnObject()
    {
        Vector3 randomPosition = new(
            Random.Range(-spawnRange.x, spawnRange.x),
            Random.Range(rangeYMin, rangeYMax),
            Random.Range(-spawnRange.z, spawnRange.z)
            );
        //Debug.Log("Spawning Object ID: " + objectArrayId + " at Position: " + randomPosition);


        Instantiate(objectToSpawn[objectArrayId],randomPosition,Quaternion.identity);
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
}
