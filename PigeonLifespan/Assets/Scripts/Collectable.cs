using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collectable : MonoBehaviour
{
    public int points;
    public int destroyRate;
    public GameObject mesh;
    public GameObject collectable;
    private int destroyAfterCollect = 2;
    public int iD = 0;
    public ParticleSystem fXPickup;
    private SpawnManager spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyRate());
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DestroyAfterCollected());
            spawnManager.UpdateScore(points);
            fXPickup.Play();
            mesh.SetActive(false);
            Destroy(gameObject.GetComponent<BoxCollider>());
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
        if (collision.gameObject.CompareTag("Opponent"))
        {
            StartCoroutine(DestroyAfterCollected());
            fXPickup.Play();
            mesh.SetActive(false);
            Destroy(gameObject.GetComponent<BoxCollider>());
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
    }
    IEnumerator DestroyRate()
    {
        while (true)
        {
            yield return new WaitForSeconds(destroyRate);
            Destroy(gameObject); 
            
        }
    }

    IEnumerator DestroyAfterCollected()
    {
        while (true)
        {
            yield return new WaitForSeconds(destroyAfterCollect);
            Destroy(gameObject);
            //Debug.Log("the Coroutine DestroyAfterCollected is finished");
        }
    }
}
