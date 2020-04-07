using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCarrot : MonoBehaviour
{
    public int iterationToRespawn = 5;

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
