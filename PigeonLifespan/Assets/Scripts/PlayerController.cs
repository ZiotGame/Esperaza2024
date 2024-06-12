using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float inputForward;
    private float inputSide;
    public float speed = 1.0f;
    // variable pour le Jump
    private Rigidbody rBody;
    public float forceUp = 10;
    public float forceForward = 5;
    public float gravityMultiplier = 5;
    // variable pour detecter les collisions
    private bool isOnGround = true;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        Physics.gravity *= gravityMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        // deplacement Player
        inputSide = Input.GetAxis("Vertical");
        inputForward = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * Time.deltaTime * inputSide * speed);
        transform.Translate(Vector3.right * Time.deltaTime * inputForward * speed);

        // le Jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rBody.AddForce(0, forceUp, forceForward, ForceMode.Impulse);
            isOnGround = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        isOnGround = true;

    }
}
