using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;


public class StormManager : MonoBehaviour
{
    public GameObject[] highlights;
    public MMFeedbacks StormFeedbacks;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeWeather();
        }
    }

    public void HighlightsOn()
    {
        foreach (GameObject highlight in highlights)
        {
            if (highlight != null)
            {
                highlight.SetActive(true);
                StormFeedbacks.PlayFeedbacks(); 
            }

        }
    }
    public void HighlightsOff()
    {
        foreach (GameObject highlight in highlights)
        {
            if (highlight != null)
            {
                highlight.SetActive(false);
            }

        }
    }
    public void ChangeWeather()
    {
        Debug.Log("Weather changed to...");
    }
}
