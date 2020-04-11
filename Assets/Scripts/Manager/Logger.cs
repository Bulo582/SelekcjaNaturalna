using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger
{
    internal bool canLog;
    public void PrintLog(string log)
    {
        if (canLog)
        {
            Debug.Log($"I = {Movement.globalIteration}\n" +
                $"{log}");
        }
    }

    public void PrintLog(string log, Transform obj)
    {
        if (canLog)
        {
            Debug.Log($"I = {Movement.globalIteration}\n" +
                $"{log}");
        }
    }
}
