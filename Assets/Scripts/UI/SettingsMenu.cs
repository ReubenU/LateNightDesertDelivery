using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Toggle TradSteerToggle;
    public GameObject TradControlPanel;
    public GameObject HaloControlPanel;

    public Slider gamepadSensSlider;
    public Text gamepadSliderVal;

    public Slider mouseSensSlider;
    public Text mouseSliderVal;

    public string gamepadSensKey = "GamePadSens";
    public string mouseSensKey = "MouseSens";
    public string tradSteerKey = "TradSteer";

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

        if (!PlayerPrefs.HasKey(tradSteerKey))
        {
            PlayerPrefs.SetInt(tradSteerKey, 0);
        }

        Settings.gamepadSensitivity = PlayerPrefs.GetInt(gamepadSensKey);
        Settings.mouseSensitivity = PlayerPrefs.GetFloat(mouseSensKey);
        Settings.useTraditionalControls = (PlayerPrefs.GetInt(tradSteerKey) == 1) ? true : false;

        TradSteerToggle.isOn = Settings.useTraditionalControls;

        gamepadSensSlider.value = Settings.gamepadSensitivity;
        gamepadSliderVal.text = string.Format("{0}", Settings.gamepadSensitivity);

        mouseSensSlider.value = Settings.mouseSensitivity;
        mouseSliderVal.text = string.Format("{0}", Settings.mouseSensitivity);

        // Change control format here
        ShowTradControls();
    }

    public void GamepadSensChanged()
    {
        PlayerPrefs.SetInt(gamepadSensKey, (int)gamepadSensSlider.value);
        Settings.gamepadSensitivity = (int)gamepadSensSlider.value;

        gamepadSliderVal.text = string.Format("{0}", (int)gamepadSensSlider.value);
    }

    public void MouseSensChanged()
    {
        PlayerPrefs.SetFloat(mouseSensKey, mouseSensSlider.value);
        Settings.mouseSensitivity = mouseSensSlider.value;

        mouseSliderVal.text = string.Format("{0}", mouseSensSlider.value);
    }

    public void TradSteerChanged()
    {
        int isTrad = (TradSteerToggle.isOn) ? 1 : 0;
        PlayerPrefs.SetInt(tradSteerKey, isTrad);

        Settings.useTraditionalControls = TradSteerToggle.isOn;

        ShowTradControls();
    }

    void ShowTradControls()
    {
        TradControlPanel.SetActive(TradSteerToggle.isOn);
        HaloControlPanel.SetActive(!TradSteerToggle.isOn);
    }
}
