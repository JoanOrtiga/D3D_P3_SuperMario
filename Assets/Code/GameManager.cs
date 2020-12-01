using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<IRestartGameElement> restartGameElements = new List<IRestartGameElement>();
    public void AddRestartGameElement(IRestartGameElement restartGameElement)
    {
        restartGameElements.Add(restartGameElement);
    }

    public void RestartGame()
    {
        foreach (IRestartGameElement item in restartGameElements)
        {
            item.Restart();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
}

public interface IRestartGameElement
{
    void Restart();
    void SetRestartPoint();
}
