using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private GameManager gameManager;
    private bool counted = false;
    private float destructionDelay = 3;
    public int countPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag you are looking for, e.g., "Ground"
        if ((!counted) && other.CompareTag("Counter"))
        {
            // Call the IncrementCount method on the Counter instance
            Counter.Instance.IncrementCount(countPoint);
            counted = true;
        }
        if (other.CompareTag("Counter") && gameManager.numberOfSpawn == gameManager.spawned)
        {

            Invoke("CollectableTriggerEnd", destructionDelay);
            Debug.Log("max spawn and Collectable collided to Counter");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Invoke("DestroyItself", destructionDelay);
        }
        /* if (collision.collider.CompareTag("Ground") && gameManager.numberOfSpawn == gameManager.spawned)
         {
             CollectableTriggerEnd();
         }*/


    }
    void DestroyItself()
    {
        Destroy(gameObject);
    }
    void CollectableTriggerEnd()
    {
        gameManager.GameEnd();
    }
}
