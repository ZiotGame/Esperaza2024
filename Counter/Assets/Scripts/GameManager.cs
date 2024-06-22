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
    private float timeToNextSpawn;
    private int objectArrayId;


    // Start is called before the first frame update
    void Start()
    {
        SetRandomTimeToNextSpawn();
      
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
        objectArrayId = Random.Range(0,objectToSpawn.Length);
        Debug.Log("Selected Object ID: " + objectArrayId + " (Prefab: " + objectToSpawn[objectArrayId].name + ")");

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
        Debug.Log("Spawning Object ID: " + objectArrayId + " at Position: " + randomPosition);


        Instantiate(objectToSpawn[objectArrayId],randomPosition,Quaternion.identity);
    }

}
