using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour, IComparable<Node>
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

    public int CompareTo(Node other)
    {
        throw new NotImplementedException();
    }

    public List<Node> FindPath(Transform target)
    {
        AlgoritmTime.Instance.Start();
        List<Node> path = new List<Node>();
        Node nodeSeeker = new Node(MapHelper.TransormX_ToMapX(transform.position.x), MapHelper.TransormZ_ToMapY(transform.position.z));
        Node nodeTarget = new Node(MapHelper.TransormX_ToMapX(target.position.x), MapHelper.TransormZ_ToMapY(target.position.z));
        Node current = null;
        
        List<Node> openList = new List<Node>(MapGenerator.SurfaceArea);
        List<Node> closedList = new List<Node>(MapGenerator.SurfaceArea);

        int g = 0;
        openList.Add(nodeSeeker);
        while (openList.Count > 0)
        {
            int lowest = openList.Min(node => node.F);
            current = openList.First(node => node.F == lowest);

            closedList.Add(current);
            openList.Remove(current);

            if (closedList.Find(node => node.X == nodeTarget.X && node.Y == nodeTarget.Y) != null) 
                break;
            List<Node> neighbors = GetWalkableAdjacentSquares(current, Spawner.Instance.GenerateMap);
            g++;
            foreach (Node node in neighbors)
            {
                if (closedList.Find(n => n.X == node.X && n.Y == node.Y) != null)
                    continue;
                if (openList.Find(n => n.X == node.X && n.Y == node.Y) == null)
                {
                    node.G = g;
                    node.H = Node.CountHScore(node, nodeTarget.X, nodeTarget.Y);
                    node.Partent = current;
                    openList.Insert(0, node);
                }
                else
                {
                    if (g + node.H < node.F)
                    {
                        node.G = g;
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
        AlgoritmTime.Instance.Stop();
        path.Reverse();
        return path;
    }
}
