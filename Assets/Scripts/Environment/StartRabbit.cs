using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRabbit : MonoBehaviour
{
    [Header("Rabbit")]

    [Range(0.1f,10f)]
    public float movementSpeed;
    public float rangeOfView;
    [Header("Hunger point to reproduction")]
    public int hunger;
    [Header("Iteration without meal to die")]
    public int iterationToDie = 25;
    private static StartRabbit manager = null;

    public static StartRabbit Manager
    {
        get
        {
            if (manager == null)
                manager = FindObjectOfType(typeof(StartRabbit)) as StartRabbit;
            return manager;
        }
    }

}


