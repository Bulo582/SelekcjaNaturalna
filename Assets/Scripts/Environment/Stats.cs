using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stats : MonoBehaviour
{
    float getOlderIncrease = 0.001f;

    public string Name;
    public int getNameNumber;

    float maxHunger;
    [Header("Hunger")]
    public float currentHunger;
    void Start()
    {
        Name = this.gameObject.name;
        maxHunger = StartRabbit.Manager.hunger;
        int.TryParse(Name.Split('_')[1], out int result);
        getNameNumber = result;
    }

    public void Meal(float hangerValue)
    {
        currentHunger += hangerValue;
        if (currentHunger > maxHunger)
            currentHunger = maxHunger;
    }
}
