using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    static bool canLog = false;
    public static void PrintLog(string log)
    {
        if (canLog)
        {
            Debug.Log($"I = {Movement.globalIteration}\n" +
                $"{log}");
        }
    }

    public static void PrintLog(string log, Transform obj)
    {
        if (canLog)
        {
            Debug.Log($"I = {Movement.globalIteration}\n" +
                $"{log}");
        }
    }
}
