using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject carPrefab;
    public GameObject[] opponentPrefab;
    public GameObject[] collectablePrefab;
    public GameObject endGame;
    private Collectable collectable;

    private float randomX = 45;
    private float randomZ = 30;
    private float edgeZ = 45;
    public bool isGameActive;

    private float time;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    private int score;
    public Button start;
    public Button restart;
    public Button reset;
    public int maxOpponents;

    // Start is called before the first frame update
    void Start()
    {
        start = GameObject.Find("Btn").GetComponent<Button>();

        start.onClick.AddListener(StartGame);
        restart.onClick.AddListener(RestartGame);
        reset.onClick.AddListener(RestartGame);
        time = 100;

    }

    void Update()
    {
        Timer();

    }
    // pick only 2 threshold values
    public float ThresholdZ()
    {
        var randy = Random.Range(-edgeZ, edgeZ);
        float randomZ = edgeZ;
        if (randy > 0)
        {
            randomZ = edgeZ;
        }
        else
        {
            randomZ = -edgeZ;
        }
        return randomZ;

    }

    // Car spawn. Direction according to the position of the spawn
    void CarSpawn()
    {

        if (ThresholdZ() < 0 && isGameActive == true)
        {
            Instantiate(carPrefab, GenerateSpawnCarPosition(), Quaternion.Euler(0, 0, 0));

        }
        if ((ThresholdZ() > 0 && isGameActive == true))
        {
            Instantiate(carPrefab, GenerateSpawnCarPosition(), Quaternion.Euler(0, 180, 0));
        }

    }

    // Generarte random position in X and Z for the car
    Vector3 GenerateSpawnCarPosition()
    {
        float xPos = Random.Range(-randomX, randomX);
        float zPos = ThresholdZ();
        return new Vector3(xPos, 1, zPos);

    }

    // Collectable spawn
    void CollectableSpawn()
    {
        if (isGameActive == true)
        {
            int collectableIndex = Random.Range(0, collectablePrefab.Length);
            Instantiate(collectablePrefab[collectableIndex], GenerateCollectablePosition(), gameObject.transform.rotation);
        }
    }
    void OpponentSpawn()
    {
        if (isGameActive == true)
        {
            int opponentIndex = Random.Range(0, opponentPrefab.Length);
            Instantiate(opponentPrefab[opponentIndex], GenerateCollectablePosition(), gameObject.transform.rotation);
        }
    }

    // Generate random position in X and Z for the collectable
    Vector3 GenerateCollectablePosition()
    {
        float xPos = Random.Range(-randomX, randomX);
        float zPos = Random.Range(-randomZ, randomZ);
        return new Vector3(xPos, 6, zPos);

    }

    // All the stuff to make a timer!
    public void Timer()
    {
        if (isGameActive == true)
        {
            time -= Time.deltaTime;
            double b = System.Math.Round(time, 0);
            timerText.text = b.ToString();

        }
        if (time < 0)
        {
            isGameActive = false;
            GameOver();
        }

    }
    public void StartGame()
    {
        isGameActive = true;
        start.gameObject.SetActive(false);
        InvokeRepeating("CarSpawn", 0, 2);
        InvokeRepeating("CollectableSpawn", 2, 3);
        score = 0;
        UpdateScore(0);
        for (int i = 0; i < maxOpponents; i++)
        {
            OpponentSpawn();
        }
    }

    // Update score with value from collectable collided
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "" + score;
    }
    public void GameOver()
    {

        endGame.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
