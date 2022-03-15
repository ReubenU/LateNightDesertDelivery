using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class IngameUI : MonoBehaviour
{
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public GameObject IngameScreen;

    public QuadScript atvBike;

    private QuadControls quadControls;
    private InputAction pauseGame;

    // UI Navigation stuff...
    public EventSystem uiEventSystem;

    public GameObject selectedCrashButton;
    public GameObject selectedPauseButton;

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
    bool activeOnce = false;
    private void Update()
    {
        // Crash bike
        if (atvBike.state == QuadScript.QuadStates.Crash && !activeOnce)
        {
            GameOverScreen.SetActive(true);
            IngameScreen.SetActive(false);
            uiEventSystem.SetSelectedGameObject(selectedCrashButton);
            activeOnce = true;
        }

        // Pause game
        if (pauseGame.triggered)
        {
            isPaused = !isPaused;
            uiEventSystem.SetSelectedGameObject(selectedPauseButton);
        }
        
        if (isPaused && !activeOnce)
        {
            PauseScreen.SetActive(true);
            IngameScreen.SetActive(false);
            Time.timeScale = 0f;
            activeOnce = !activeOnce;
        }

        // Unpause game
        if (isPaused == false && GameOverScreen.activeSelf == false)
        {
            PauseScreen.SetActive(false);
            IngameScreen.SetActive(true);
            Time.timeScale = 1f;
            activeOnce = false;
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
