using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundDestroy : MonoBehaviour
{
    private float boundTop = 50f;
    private float boundBottom = -50f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > boundTop)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < boundBottom)
        {
            Destroy(gameObject);
        }
    }
}
