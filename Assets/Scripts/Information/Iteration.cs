using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Iteration
{
    private static int _global = 1;

    public static int Global
    {
        get { return _global; }
    }

    internal static void NextIteration()
    {
        PopulationController.CollectingRabbitInfo();
        PopulationController.CollectionFamiliesInfo();
        _global++;
    }
}

