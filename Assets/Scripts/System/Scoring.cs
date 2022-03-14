using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    [HideInInspector]
    public int currentScore = 0;

    public Text scoreText;

    private void Update()
    {
        scoreText.text = string.Format("Current Score\n{0}", currentScore);
    }
}
