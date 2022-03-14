using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IngameUI : MonoBehaviour
{
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public GameObject IngameScreen;

    public QuadScript atvBike;

    private QuadControls quadControls;
    private InputAction pauseGame;


    private void Awake()
    {
        quadControls = new QuadControls();
        pauseGame = quadControls.UI.PauseGame;
    }

    private void OnEnable()
    {
        quadControls.Enable();
        pauseGame.Enable();
    }

    private void OnDisable()
    {
        quadControls.Disable();
        pauseGame.Disable();
    }

    bool isPaused = false;
    private void Update()
    {
        // Crash bike
        if (atvBike.state == QuadScript.QuadStates.Crash)
        {
            GameOverScreen.SetActive(true);
            IngameScreen.SetActive(false);
        }

        // Pause game
        if (pauseGame.triggered)
        {
            isPaused = !isPaused;
        }
        
        if (isPaused)
        {
            PauseScreen.SetActive(true);
            IngameScreen.SetActive(false);
            Time.timeScale = 0f;
        }

        // Unpause game
        if (isPaused == false && GameOverScreen.activeSelf == false)
        {
            PauseScreen.SetActive(false);
            IngameScreen.SetActive(true);
            Time.timeScale = 1f;
        }
    }


    public void ResumeGame()
    {
        PauseScreen.SetActive(false);
        IngameScreen.SetActive(true);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit2MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
