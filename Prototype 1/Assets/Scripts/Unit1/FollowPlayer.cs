using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -7); 

  
    // Update is called once per frame
    void LateUpdate()
    {
        //offset the camera position by adding the player's position
        //transform.position = player.transform.position + offset;
        
        // cam following without offset ???
        transform.position = player.transform.position + offset;
    }
}
