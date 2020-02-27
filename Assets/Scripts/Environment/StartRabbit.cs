using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRabbit : MonoBehaviour
{
    [Header("Rabbit")]
    public short reproductionCapacity;
    public short reproductionAge;
    public short age;
    public short limitAge;
    public float movementSpeed;
    public float rangeOfView;
    public float hunger;
    public float thrist;
    public float strength;
    public Color femaleColor;
    public Color maleColour;

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


