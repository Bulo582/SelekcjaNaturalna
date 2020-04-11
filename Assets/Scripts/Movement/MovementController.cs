using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction
{
    up = 1,
    right = 2,
    down = 3,
    left = 4,
    none = 5
}
public class MovementController // Population Controller
{
    public static List<Movement> Creatures = new List<Movement>();
    public static bool IsReady()
    {
        return Creatures.All(l => l.ready == true);
    }
    public static bool IsNotReady()
    {
        return Creatures.All(l => l.ready == false);
    }
    public static void NumeringPopulation()
    {
        if (Creatures != null)
        {
            int idx = 1;
            foreach (var item in Creatures)
            {
                item.rabbitLife.rabbitID = idx;
                idx++;
            }
        }
    }
}
