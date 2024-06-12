using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerControllerJoystick : MonoBehaviour
{
    [SerializeField] public Rigidbody _rigidbody;
    [SerializeField] public FloatingJoystick _joystick;
    [SerializeField] public Animator _animator;

    [SerializeField] public float _moveSpeed;
    [SerializeField] private float zBound = 30;
    public float forceStrength = 5;

    public SpawnManager spawnManager;
    public ParticleSystem fXDeath;

    // Start is called before the first frame update
    void Start()
    {

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }


    // Update is called once per frame
    private void FixedUpdate()

    {
        if (spawnManager.isGameActive == true)
        { 
        joystickController();
        streetBoundaries();
        }      
    }

    // Move the player with a joystick
    void joystickController()
    {

        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0 )
        {

            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
    }

    // restrict the player from going to far on the street (Z axis)
    void streetBoundaries()
    {
        if (transform.position.z > zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBound);
        }
        if (transform.position.z < -zBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBound);
        }
    }

    // colliders and methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            spawnManager.isGameActive = false;
            spawnManager.GameOver();
            _animator.SetBool("isDead", true);
            fXDeath.Play();
        }
        if (collision.gameObject.CompareTag("Opponent"))
        {
            //Debug.Log("Fight with Opponent!");
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.transform.position - transform.position;
            enemyRigidbody.AddForce(awayFromPlayer * forceStrength, ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag("Collectable"))
        {
           // Destroy(collision.gameObject);
            
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            Destroy(other.gameObject);
            Debug.Log("You found some crumbs");
        }
    }*/

}
