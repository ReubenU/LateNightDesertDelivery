using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destinations : MonoBehaviour
{
    public Delivery[] dest;
    public Transform player;
    public Transform highlight;

    Delivery currentDest;

    private void Awake()
    {
        currentDest = ChooseRandomDest();
        currentDest.isActive = true;
        highlight.position = currentDest.transform.position;
    }

    private void Update()
    {
        ChooseNextDest();
    }

    Delivery ChooseRandomDest()
    {
        int lenDests = dest.Length;
        int randNum = Mathf.RoundToInt(Random.value * (lenDests-1));


        return dest[randNum];
    }

    void ChooseNextDest()
    {
        if (currentDest.isCompleted)
        {
            currentDest.isCompleted = false;
            currentDest = ChooseRandomDest();
            currentDest.isActive = true;
            highlight.position = currentDest.transform.position;
        }
    }
}
