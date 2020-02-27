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
    public float hungerIncrease = 0.5f;

    float maxThrist;
    [Header("Thrist")]
    public float currentThrist;
    public float thristIncrease = 2f;

    float maxReproductionCapacity;
    [Header("Reproduction")]
    public float currentReproCapacity = 0;
    public float reproductionCapacityIncrease = 1f;

    float maxStrength;
    [Header("Strength")]
    public float currentStrength;
    public float strengthIncrease = 2;

    int reproductionAge;
    int maxAge;
    [Header("Age")]
    public float currentAge;
    void Start()
    {
        Name = this.gameObject.name;
        maxHunger = StartRabbit.Manager.hunger;
        maxThrist = StartRabbit.Manager.thrist;
        maxReproductionCapacity = StartRabbit.Manager.reproductionCapacity;
        maxAge = StartRabbit.Manager.limitAge;
        currentAge = StartRabbit.Manager.age;
        reproductionAge = StartRabbit.Manager.reproductionAge;
        maxStrength = StartRabbit.Manager.strength;
        currentStrength = maxStrength;
        currentHunger = maxHunger;
        currentThrist = maxThrist;
    }


    private void Update()
    {
        if (currentHunger >= 0)
        {
            currentHunger -= (hungerIncrease * Time.deltaTime);
        }
        if (currentThrist >= 0)
        {
            currentThrist -= (thristIncrease * Time.deltaTime);
        }
        if (currentReproCapacity < maxReproductionCapacity && currentAge >= reproductionAge)
        {
            currentReproCapacity += (reproductionCapacityIncrease * Time.deltaTime);
        }
        if(currentStrength > 0 && (LowHunger() || LowThrist()))
        {
            currentStrength -= (strengthIncrease * Time.deltaTime);
            if(currentStrength <= 0)
                Destroy(this.gameObject);
        }

        if (currentAge < maxAge)
        {
            currentAge += (getOlderIncrease * Time.deltaTime);
        }
        else
            Destroy(this.gameObject);
    }

    public void Meal(float hangerValue)
    {
        currentHunger += hangerValue;
        if (currentHunger > maxHunger)
            currentHunger = maxHunger;
    }
    public void Drink(float thristValue)
    {
        currentThrist += thristValue;
        if (currentThrist > maxThrist)
            currentThrist = maxThrist;
    }

    public bool LowHunger()
    {
        if (((currentStrength * 100) / maxHunger) <= 20)
            return true;
        return false;
    }

    public bool LowThrist()
    {
        if (((currentThrist * 100) / maxHunger) <= 20)
            return true;
        return false;
    }
}
