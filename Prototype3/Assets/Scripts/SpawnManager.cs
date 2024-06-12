using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstacle;
    private Vector3 pos = new Vector3(30, 0, 0);
    private float startDelay = 2;
    private float delay = 2;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnRepeat", startDelay, delay);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnRepeat()
    {

        if (playerControllerScript.gameOver == false)
        {
            Instantiate(obstacle, pos, obstacle.transform.rotation); 
        }



    }
}
