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
public class MovementController
{
    public static List<Movement> Creatures = new List<Movement>();
    public static int iteration = 0;
    public static bool IsReady()
    {
        return Creatures.All(l => l.ready == true);
    }


}
