using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public int X;
    public int Y;
    public int F;
    public int G;
    public int H;
    public Node Partent;
    public Direction dir
    {
        get
        {
            if (Partent != null)
            {
                if (Partent.X < X)
                    return Direction.down;
                else if (Partent.X > X)
                    return Direction.up;
                else if (Partent.Y > Y)
                    return Direction.left;
                else if (Partent.Y < Y)
                    return Direction.right;
                else
                    return Direction.none;
            }
            else return Direction.none;
        }
    }
    public Node(int x, int y)
    {
        X = x;
        Y = y;
    }
    public static int CountHScore(Node node, int targetX, int targetY)
    {
        return Mathf.Abs(targetX - node.X) + Mathf.Abs(targetY - node.Y);
    }
    public bool IsTheSamePosition(Node target)
    {
        if (this.X == target.X && this.Y == target.Y)
            return true;
        else return false;
    }
}
