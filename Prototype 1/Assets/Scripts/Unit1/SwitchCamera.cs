using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject FPSCam;
    public GameObject ThirdPersonCam;
    public int Manager;

    public void ManageCamera()
    {
        if (Manager == 0) {
            FPS();
            Manager = 1;
        }
        else
        {
            Third();
            Manager = 0;
        }
    }

    void FPS()
    {
        FPSCam.SetActive(true);
        ThirdPersonCam.SetActive(false);
    }

    void Third()
    {
        FPSCam.SetActive(false);
        ThirdPersonCam.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            {
                ManageCamera();
            }
        }
    }
}
