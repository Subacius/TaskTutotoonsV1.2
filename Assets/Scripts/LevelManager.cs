using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameCore gameCore;
    private GameObject timer;

    private void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer");
            if (timer == null)
            {
                Debug.LogError("Timer object not found in the scene!");
            }
    }

    public void NextLevel()
    {
        gameCore.levelNumber++;

        if (gameCore.levelNumber >= gameCore.levelData.levels.Length)
        {
            SceneManager.LoadScene("GameOver");
            timer.GetComponent<Timer>().isTimerRunning = false;
            timer.GetComponent<Timer>().SaveTimerText();
        }
        else
        {
            gameCore.level = gameCore.levelData.levels[gameCore.levelNumber];
            gameCore.GenerateLevel();
            gameCore.UpdateCurrentNumberToClick(1);
            gameCore.InitRope();
            gameCore.Win.SetActive(false);
            timer.GetComponent<Timer>().isTimerRunning = true;
        }
    }
}
