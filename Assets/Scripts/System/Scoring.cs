using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    [HideInInspector]
    public int currentScore = 0;

    public Text scoreText;
    public Text highScore;

    int highestScore;

    QuadScript quad;

    private void Awake()
    {
        highestScore = PlayerPrefs.GetInt("HighScore");

        quad = GetComponent<QuadScript>();

        highScore.text = string.Format("Highest Score\n{0}", highestScore);
    }

    private void Update()
    {
        scoreText.text = string.Format("Current Score\n{0}", currentScore);

        if (currentScore > highestScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }
}
