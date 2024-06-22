using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private bool counted = false;
    private float destructionDelay = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag you are looking for, e.g., "Ground"
        if ((!counted) && other.CompareTag("Counter") )
        {
            // Call the IncrementCount method on the Counter instance
            Counter.Instance.IncrementCount();
            counted = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {

            Invoke("DestroyItself", destructionDelay);
        }
    }
    void DestroyItself()
    {
        Destroy(gameObject);
    }
}
