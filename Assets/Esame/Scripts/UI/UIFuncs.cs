using System.Collections;
using UnityEngine;

public class UIFuncs : MonoBehaviour
{
    [SerializeField] GameObject victoryScreen;

    private void Start()
    {
        victoryScreen.SetActive(false);
    }

    public void ResetBoard()
    {
        if (TouchManager.Instance.GetAllowTouch())
            GameManagerEsame.Instance.ResetGame();
    }

    public void Undo()
    {
        if (TouchManager.Instance.GetAllowTouch())
        {
            TouchManager.Instance.UndoPreviousMove();
        }
    }

    public void Nextlevel()
    {
        if (TouchManager.Instance.GetAllowTouch())
        {
            GameManagerEsame.Instance.GenerateLevel();
        }
    }

    public void CloseGame()
    {
        GameManagerEsame.Instance.CloseGame();
    }

    public void DisplayVictoryScreen()
    {
        TouchManager.Instance.SetAllowTouch(false);
        victoryScreen.SetActive(true);
        StartCoroutine(VictoryPause(2));
    }

    private IEnumerator VictoryPause(float pauseSeconds)
    {
        yield return new WaitForSeconds(pauseSeconds/2);
        GameManagerEsame.Instance.GenerateLevel();
        yield return new WaitForSeconds(pauseSeconds/2);
        victoryScreen.SetActive(false);
        TouchManager.Instance.SetAllowTouch(true);
    }
}
