using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    SpawnManager spawnManager;
    public float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       transform.Translate(Vector3.forward * Time.deltaTime * speed);

       /*this code purpose was to change translation direction depending on the Z position
        if (transform.position.z < 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            
        }else
        {
            transform.Translate(Vector3.back* Time.deltaTime * speed);
        }
       */
        
        
    }
}
