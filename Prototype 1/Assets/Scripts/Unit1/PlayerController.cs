using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Private variables
    [SerializeField]private float speed = 10.0f;
    private const float turnSpeed = 50.0f;
    private float horizontalInput;
    private float forwardInput;


    // Update is called once per frame
    void FixedUpdate()
    {
        // This is where we get the player input
        horizontalInput = Input.GetAxis ("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
        // We move the vehicule forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        // I keep this line of code for remebenring but it's obsolete
        //transform.Translate(Vector3.right * Time.deltaTime * turnSpeed * horizontalInput);
        // We rotate the player
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput);
    }
}
