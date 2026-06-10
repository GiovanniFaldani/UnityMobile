using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {  get; private set; }

    [SerializeField] GameObject hudCanvas;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject highScoreCanvas;
    [SerializeField] TMP_Text highScore1;
    [SerializeField] TMP_Text highScore2;
    [SerializeField] TMP_Text highScore3;

    private void Awake()
    {
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
    }

    public void UpdateTimer(float timer)
    {
        timerText.text = "Time: " + timer.ToString("00");
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOverUI()
    {
        hudCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
    }

    public void HighScoreUI()
    {
        UpdateHighScores();
        hudCanvas.SetActive(false);
        highScoreCanvas.SetActive(true);
    }

    public void UpdateHighScores()
    {
        highScore1.text = GameManager.Instance.highScores[0].ToString();
        highScore2.text = GameManager.Instance.highScores[1].ToString();
        highScore3.text = GameManager.Instance.highScores[2].ToString();
    }

    public void CallRestart()
    {
        GameManager.Instance.Restart();
    }

    public void CallClose()
    {
        GameManager.Instance.Close();
    }
}
