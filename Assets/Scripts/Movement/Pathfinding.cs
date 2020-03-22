using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static List<Node> GetWalkableAdjacentSquares(Node node, char[,] map)
    {
        List<Node> locations = new List<Node>();

        if (node.Y - 1 >= 0)
            locations.Add(new Node(node.X, node.Y - 1));
        if (node.X - 1 >= 0)
            locations.Add(new Node(node.X - 1, node.Y));
        if (node.Y + 1 < map.GetLength(1))
            locations.Add(new Node(node.X, node.Y + 1));
        if (node.X + 1 < map.GetLength(0))
            locations.Add(new Node(node.X + 1, node.Y));
        return locations.Where(n => int.TryParse(map[n.X, n.Y].ToString(), out int res) || map[n.X, n.Y].ToString() == "C").ToList();
    }

    public List<Node> FindPath(Transform target)
    {
        List<Node> path = new List<Node>();
        Node nodeSeeker = new Node(TransormX_ToMapX(transform.position.x), TransormZ_ToMapY(transform.position.z));
        Node nodeTarget = new Node(TransormX_ToMapX(target.position.x), TransormZ_ToMapY(target.position.z));
        Node current = null;
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        int g = 0;
        openList.Add(nodeSeeker);
        while (openList.Count > 0)
        {
            int lowest = openList.Min(node => node.F);
            current = openList.First(node => node.F == lowest);

            closedList.Add(current);
            openList.Remove(current);

            if (closedList.FirstOrDefault(node => node.X == nodeTarget.X && node.Y == nodeTarget.Y) != null)
                break;
            List<Node> neighbors = GetWalkableAdjacentSquares(current, Spawner.Instance.GenerateMap);
            g++;
            foreach (Node node in neighbors)
            {
                if (closedList.FirstOrDefault(n => n.X == node.X && n.Y == node.Y) != null)
                    continue;
                if (openList.FirstOrDefault(n => n.X == node.X && n.Y == node.Y) == null)
                {
                    node.G = g;
                    node.H = Node.CountHScore(node, nodeTarget.X, nodeTarget.Y);
                    node.F = node.G + node.H;
                    node.Partent = current;
                    openList.Insert(0, node);
                }
                else
                {
                    if (g + node.H < node.F)
                    {
                        node.G = g;
                        node.F = node.H + node.G;
                        node.Partent = current;
                    }
                }
            }
        }
        while (current != null)
        {
            if (current.IsTheSamePosition(nodeSeeker) == false && current.IsTheSamePosition(nodeTarget) == false)
            {
                path.Add(current);
                current = current.Partent;
            }
            else
                current = current.Partent;
        }
        path.Reverse();
        return path;
    }

    //----------------------- for optimalize method. New Class 
    public int TransormX_ToMapX(float XPos)
    {
        return (Convert.ToInt16(XPos)) + Spawner.Instance.HalfHeightMap;
    }

    public int TransormZ_ToMapY(float ZPos)
    {
        return (Convert.ToInt16(ZPos)) - Spawner.Instance.HalfWidthMap;
    }
}
