using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public static Counter Instance; // Singleton instance
    private bool counted = false;
    public Text CounterText;
    private static int Count = 0;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of the Counter exists
        if (Instance==null)
        {
            Instance = this;
        }else
        {
           // Destroy(gameObject);
        }
    }
    private void Start()
    {
        Count = 0;
    }
    public void IncrementCount()
    {
        if (!counted)
        {
            Count += 1;
            CounterText.text = "Count : " + Count;
            counted = true;
        }
    }
  

}
