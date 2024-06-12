using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MonoBehaviour
{

    private SpawnManager spawnManagerInOpponent;
    private GameObject[] collectable;
    public float speed = 20;
    public ParticleSystem fxDeath;
    private Rigidbody opponentRb;
    private GameObject instantiatedCollectable;
    private bool isAlive;
    [SerializeField] public Animator _animator;
    [SerializeField] private float zBound = 30;



    // Start is called before the first frame update
    void Start()
    {
        opponentRb = GetComponent<Rigidbody>();
        spawnManagerInOpponent = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        TargetCollectable();
        opponentAnimations();
        StreetBoundaries();
        //Debug.Log("the opponent is alive"+ isAlive);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Opponent touched the Player");
        }
        if (collision.gameObject.CompareTag("Collectable"))
        {
            //Debug.Log("Opponent ate some crumbs!");
        }
        if (collision.gameObject.CompareTag("Car"))
        {
            _animator.SetBool("isDead", true);
            fxDeath.Play();
            isAlive = false;
        }
    }

    void TargetCollectable()
    {
        for (int i = 0; i < collectable.Length; i++)
        {
       
        }
        Debug.Log(collectable.Length);
        //int collectableIndex = Random.Range(0, collectable.Length);
        //collectable[collectableIndex] = GameObject.FindGameObjectWithTag("Collectable");

        if (isAlive == true && collectable != null)
        {
            Vector3 lookDirection = (collectable[1].transform.position - transform.position).normalized;
            /* This code works only if the object has a sphere collider
            opponentRb.AddForce(lookDirection * speed * Time.deltaTime);
            */
            transform.Translate(lookDirection * Time.deltaTime * speed);
            // Debug.Log("la vitesse: "+ opponentRb.velocity.magnitude);

            //gameObject.transform.LookAt(collectable.transform);
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }


    }
    void opponentAnimations()
    {
        /*
        if (opponentRb.velocity.magnitude > 0.1)
        {
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
        */

    }
    void StreetBoundaries()
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

}
