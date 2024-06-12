using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float jumpForce = 10;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParicle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        // assign the variable to the rigid body component of the game object
        playerRb = GetComponent<Rigidbody>();
        // assign the variable to the Animator component of the game object
        playerAnim = GetComponent<Animator>();
        // change the physics property (Gravity) of Unity for this object
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;
                playerAnim.SetTrigger("Jump_trig");
                Debug.Log("Jump");
                dirtParicle.Stop();
                playerAudio.PlayOneShot(jumpSound);
            }
        }
    }

    // detect if the gameobject collide with another collider
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParicle.Play();
        }
        else if (collision.collider.CompareTag("Obstacle"))
        {
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParicle.Stop();
            playerAudio.PlayOneShot(crashSound, 0.5f);
            //Debug.Log("Game Over");
        }

    }
}
