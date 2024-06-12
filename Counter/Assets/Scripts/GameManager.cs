using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector3 spawnRange = new(0,10,10);
    public int rangeYMin = 6;
    public int rangeYMax = 20;
    public float minSpawnTime = 1.0f;
    public float maxSpawnTime = 5.0f;
    private float timeToNextSpawn;

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
            SpawnObject();
            SetRandomTimeToNextSpawn();
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
        Instantiate(objectToSpawn,randomPosition,Quaternion.identity);
    }
}
