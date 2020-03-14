using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stats : MonoBehaviour
{
    float getOlderIncrease = 0.001f;

    public string Name;

    float maxHunger;
    [Header("Hunger")]
    public float currentHunger;
    void Start()
    {
        Name = this.gameObject.name;
        maxHunger = StartRabbit.Manager.hunger;
    }

    private void Update()
    {

    }

    public void Meal(float hangerValue)
    {
        currentHunger += hangerValue;
        if (currentHunger > maxHunger)
            currentHunger = maxHunger;
    }
}
