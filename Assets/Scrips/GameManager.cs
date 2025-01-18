using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum GameState
{
    Intro,
    Playing,
    Dead,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState = GameState.Intro;
    
    public int lives = 3;
    
    [Header("References")] 
    public GameObject introUI;
    public GameObject deadUI;
    
    public GameObject enemySpawner;
    public GameObject foodSpawner;
    public GameObject goldenSpawner;

    public TMP_Text scoreText;
    
    public Player player;
    
    [FormerlySerializedAs("playerStartTime")] public float playStartTime;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        introUI.SetActive(true);
    }

    private float CalculateScore()
    {
        return Time.time - playStartTime;
    }

    private void SaveHighScore()
    {
        var score = Mathf.FloorToInt(CalculateScore());
        var curHighScore = PlayerPrefs.GetInt("highScore", 0);
        if (score > curHighScore)
        {
            PlayerPrefs.SetInt("highScore", score);
        }
    }

    private int GetHighScore()
    {
        return PlayerPrefs.GetInt("highScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState.Equals(GameState.Playing))
        {
            scoreText.text = "Score: " + Mathf.FloorToInt(CalculateScore());
        }
        else if (gameState.Equals(GameState.Dead))
        {
            scoreText.text = "High Score: " + GetHighScore();
        }
        
        if (gameState.Equals(GameState.Intro) && Input.GetKeyDown(KeyCode.Space))
        {
            gameState = GameState.Playing;
            introUI.SetActive(false);
            enemySpawner.SetActive(true);
            foodSpawner.SetActive(true);
            goldenSpawner.SetActive(true);

            playStartTime = Time.time;
        }

        if (gameState.Equals(GameState.Playing) && lives.Equals(0))
        {
            player.KillPlayer();
            deadUI.SetActive(true);
            enemySpawner.SetActive(false);
            foodSpawner.SetActive(false);
            goldenSpawner.SetActive(false);
            gameState = GameState.Dead;
            SaveHighScore();
        }

        if (gameState.Equals(GameState.Dead) && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("main");
        }
    }

    public float CalculateGameSpeed()
    {
        if (!gameState.Equals(GameState.Playing))
        {
            return 5f;
        }

        var speed = 5f + (0.5f * Mathf.FloorToInt(CalculateScore() / 10f));
        return Mathf.Min(speed, 30f);
    }
}
