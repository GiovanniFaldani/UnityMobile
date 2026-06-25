using UnityEngine;

public class UIFuncs : MonoBehaviour
{
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
}
