using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCarrot : MonoBehaviour
{
    public int iterationToRespawn = 5;
    public static readonly char sign = 'C';
    private static StartCarrot manager = null;

    public static StartCarrot Manager
    {
        get
        {
            if (manager == null)
                manager = FindObjectOfType(typeof(StartCarrot)) as StartCarrot;
            return manager;
        }
    }
}
