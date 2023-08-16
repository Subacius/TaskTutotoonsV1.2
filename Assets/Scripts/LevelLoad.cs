using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour 
{
    [SerializeField] int timeToWait = 4;
    int currentSceneIndex;
    private void Start() {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitForTime());
        }
    }
    IEnumerator WaitForTime() {
        yield return new WaitForSeconds(timeToWait);
        LoadNextScene();
    }
    public void LoadNextScene() {
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void QuitGame() {
        Application.Quit();
    }
}
