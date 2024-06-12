using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float destructionDelay = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
