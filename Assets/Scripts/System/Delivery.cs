using UnityEngine;

public class Delivery : MonoBehaviour
{
    public int points = 50;
    public int addTime = 30;

    public Scoring scoreboard;

    public bool isActive = false;

    public bool isCompleted = false;

    public GameObject amazonPackage;
    public GameObject bouncyBallPackage;
    public GameObject tetraPackage;

    public GameObject packageLight;

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && !isCompleted)
        {
            scoreboard.currentScore += points;
            isActive = false;
            isCompleted = true;
            SpawnPackages();
        }
    }

    void SpawnPackages()
    {
        GameObject amzn = Instantiate(amazonPackage, transform.position + Vector3.up, transform.rotation);
        GameObject tetra = Instantiate(tetraPackage, transform.position + Vector3.up*2, transform.rotation);
        GameObject bounce = Instantiate(bouncyBallPackage, transform.position + Vector3.up*3, transform.rotation);
        GameObject light = Instantiate(packageLight, transform.position + Vector3.up * 4, transform.rotation);


        Destroy(amzn, 10);
        Destroy(tetra, 10);
        Destroy(bounce, 10);
        Destroy(light, 10);
    }
}
