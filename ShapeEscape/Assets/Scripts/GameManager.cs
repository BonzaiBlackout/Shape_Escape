using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameStarted = false;
    public GameObject startPanel;

    void Start()
    {
        // Initial game state = paused
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        gameStarted = true;
        startPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
