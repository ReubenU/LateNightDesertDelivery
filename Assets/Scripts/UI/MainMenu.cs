using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainScreen;

    public GameObject controlsScreen;
    public GameObject creditsScreen;

    public EventSystem uiEvents;


    public string gamepadSensKey = "GamePadSens";
    public string mouseSensKey = "MouseSens";

    private int defaultGamepadSens = 30;
    private float defaultMouseSens = 3;


    private void Awake()
    {
        if (!PlayerPrefs.HasKey(gamepadSensKey))
        {
            PlayerPrefs.SetInt(gamepadSensKey, defaultGamepadSens);
        }

        if (!PlayerPrefs.HasKey(mouseSensKey))
        {
            PlayerPrefs.SetFloat(mouseSensKey, defaultMouseSens);
        }

        Settings.gamepadSensitivity = PlayerPrefs.GetInt(gamepadSensKey);
        Settings.mouseSensitivity = PlayerPrefs.GetFloat(mouseSensKey);
    }


        public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void CreditsToMenu()
    {
        creditsScreen.SetActive(false);
        mainScreen.SetActive(true);

        uiEvents.SetSelectedGameObject(FirstButton(mainScreen));
    }

    public void Controls2Menu()
    {
        controlsScreen.SetActive(false);
        mainScreen.SetActive(true);

        uiEvents.SetSelectedGameObject(FirstButton(mainScreen));
    }

    public void Menu2Credits()
    {
        creditsScreen.SetActive(true);
        mainScreen.SetActive(false);

        uiEvents.SetSelectedGameObject(FirstButton(creditsScreen));
    }

    public void Menu2Controls()
    {
        controlsScreen.SetActive(true);
        mainScreen.SetActive(false);

        uiEvents.SetSelectedGameObject(FirstButton(controlsScreen));
    }

    GameObject FirstButton(GameObject parentPanel)
    {
        Button[] buttons = parentPanel.GetComponentsInChildren<Button>();

        if (buttons.Length > 0)
        {
            return buttons[0].gameObject;
        }

        return uiEvents.firstSelectedGameObject;
    }
}
