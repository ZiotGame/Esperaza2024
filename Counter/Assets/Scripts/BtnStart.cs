using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BtnStart : MonoBehaviour
{
    private Button button;
    public GameManager gamemanager;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(RestartGame);
        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    void RestartGame()
    {
        gamemanager.RestartGame();
    }
}
