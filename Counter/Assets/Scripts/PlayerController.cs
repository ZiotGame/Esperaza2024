using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive == true)
        {
            PlayerControl();
        }
    }

    void PlayerControl()
    {
        Vector3 position = transform.position;
        Debug.Log("PlayerControl method called.");

        // Handle touch input for keyboard devices
        if (Input.GetKey(KeyCode.A))
        {
            position.z -= speed * Time.deltaTime;
            Debug.Log("Pushed A");


        }
        else if (Input.GetKey(KeyCode.D))
        {
            position.z += speed * Time.deltaTime;
            
        }
    
        transform.position = position;
    }
}
