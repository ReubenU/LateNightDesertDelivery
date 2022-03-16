using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    [HideInInspector]
    public int currentScore = 0;

    public Text scoreText;
    public Text highScore;
    public Text timeLeft;

    int highestScore;

    public float time2ride = 60f;

    private void Awake()
    {
        highestScore = PlayerPrefs.GetInt("HighScore");

        highScore.text = string.Format("Highest Score\n{0}", highestScore);
    }

    private void Update()
    {
        scoreText.text = string.Format("Current Score\n{0}", currentScore);

        if (currentScore > highestScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }

        Timer();
    }

    void Timer()
    {
        time2ride -= Time.deltaTime;
        time2ride = Mathf.Clamp(time2ride, 0, Mathf.Infinity);
        timeLeft.text = string.Format("{0}", Mathf.RoundToInt(time2ride));
    }
}
