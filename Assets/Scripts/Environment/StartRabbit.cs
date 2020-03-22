using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRabbit : MonoBehaviour
{
    [Header("Rabbit")]
     short reproductionCapacity;
     short reproductionAge;
     short age;
     short limitAge;
    public float movementSpeed;
    public float rangeOfView;
    public float hunger;
     float thrist;
     float strength;
     Color femaleColor;
     Color maleColour;

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#822358", out femaleColor);
        ColorUtility.TryParseHtmlString("#823E23", out maleColour);
    }

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


