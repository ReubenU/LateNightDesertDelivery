using UnityEngine;

public class Delivery : MonoBehaviour
{
    public int points = 50;
    public int addTime = 30;

    public Scoring scoreboard;

    public bool isActive = false;

    public bool isCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && !isCompleted)
        {
            scoreboard.currentScore += points;
            isActive = false;
            isCompleted = true;
        }
    }
}
