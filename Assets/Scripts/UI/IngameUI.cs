using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class IngameUI : MonoBehaviour
{
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public GameObject SettingsScreen;
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

        Cursor.lockState = CursorLockMode.Locked;
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
            uiEventSystem.SetSelectedGameObject(FirstButton(GameOverScreen));
            activeOnce = true;

            Cursor.lockState = CursorLockMode.None;
        }

        // Pause game
        if (pauseGame.triggered)
        {
            isPaused = !isPaused;
        }
        
        if (isPaused && !activeOnce && !GameOverScreen.activeSelf)
        {
            PauseScreen.SetActive(true);
            IngameScreen.SetActive(false);
            SettingsScreen.SetActive(false);
            Time.timeScale = 0f;
            activeOnce = !activeOnce;

            Cursor.lockState = CursorLockMode.None;

            uiEventSystem.SetSelectedGameObject(FirstButton(PauseScreen));
        }

        // Unpause game
        if (isPaused == false && GameOverScreen.activeSelf == false)
        {
            PauseScreen.SetActive(false);
            SettingsScreen.SetActive(false);
            IngameScreen.SetActive(true);
            Time.timeScale = 1f;
            activeOnce = false;

            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    public void ResumeGame()
    {
        PauseScreen.SetActive(false);
        IngameScreen.SetActive(true);
        isPaused = false;
        Time.timeScale = 1f;

        atvBike.state = QuadScript.QuadStates.Active;
    }

    // Settings function
    public void OpenSettings()
    {
        PauseScreen.SetActive(false);
        SettingsScreen.SetActive(true);
        uiEventSystem.SetSelectedGameObject(FirstButton(SettingsScreen));
    }

    public void CloseSettings()
    {
        PauseScreen.SetActive(true);
        SettingsScreen.SetActive(false);
        uiEventSystem.SetSelectedGameObject(FirstButton(PauseScreen));
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit2MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    GameObject FirstButton(GameObject parentPanel)
    {
        Button[] buttons = parentPanel.GetComponentsInChildren<Button>();

        if (buttons.Length > 0)
        {
            return buttons[0].gameObject;
        }

        return uiEventSystem.firstSelectedGameObject;
    }
}
