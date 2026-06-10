using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int score;

    public List<int> highScores = new List<int>() { 0, 0, 0 };
    private float timer = 60;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        score = 0;
        timer = 60;
        Time.timeScale = 1;
        UIManager.Instance.UpdateScore(score);
        UIManager.Instance.UpdateTimer(timer);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (Time.timeScale > 0)
            UIManager.Instance.UpdateTimer(timer);

        if (timer < 0 && Time.timeScale > 0)
            HighScore();
        if (score < 0 && Time.timeScale > 0)
            GameOver();
    }

    public void AddScore(int toAdd)
    {
        score += toAdd;
        UIManager.Instance.UpdateScore(score);
    }

    public void GameOver()
    {
        UIManager.Instance.GameOverUI();
        Time.timeScale = 0;
    }

    public void HighScore()
    {
        //update scores
        highScores.Add(score);
        highScores.Sort();
        highScores.Reverse();

        UIManager.Instance.HighScoreUI();
        Time.timeScale = 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Start();
    }

    public void Close()
    { 
        Application.Quit();
    }
}
