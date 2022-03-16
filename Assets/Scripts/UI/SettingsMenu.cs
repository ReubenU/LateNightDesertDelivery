using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider gamepadSensSlider;
    public Text gamepadSliderVal;

    public Slider mouseSensSlider;
    public Text mouseSliderVal;

    public string gamepadSensKey = "GamePadSens";
    public string mouseSensKey = "MouseSens";

    private int gamepadSens = 30;
    private float mouseSens = 3;

    private void Awake()
    {
        if (PlayerPrefs.GetInt(gamepadSensKey) == 0)
        {
            PlayerPrefs.SetInt(gamepadSensKey, gamepadSens);
        }
        
        if (Mathf.RoundToInt(PlayerPrefs.GetFloat(mouseSensKey)) == 0)
        {
            PlayerPrefs.SetFloat(mouseSensKey, mouseSens);
        }

        gamepadSens = PlayerPrefs.GetInt(gamepadSensKey);
        mouseSens = PlayerPrefs.GetFloat(mouseSensKey);


        if (gamepadSens != 0)
        {
            gamepadSensSlider.value = gamepadSens;
            gamepadSliderVal.text = string.Format("{0}", gamepadSens);
        }

        if (Mathf.RoundToInt(mouseSens) != 0)
        {
            mouseSensSlider.value = mouseSens;
            mouseSliderVal.text = string.Format("{0}", mouseSens);
        }
    }

    public void GamepadSensChanged()
    {
        PlayerPrefs.SetInt(gamepadSensKey, (int)gamepadSensSlider.value);
        gamepadSliderVal.text = string.Format("{0}", (int)gamepadSensSlider.value);
    }

    public void MouseSensChanged()
    {
        PlayerPrefs.SetFloat(mouseSensKey, mouseSensSlider.value);
        mouseSliderVal.text = string.Format("{0}", mouseSensSlider.value);
    }
}
