using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    GameObject generator;
    void Start()
    {
        StartValues sv = GameObject.Find("Environment").GetComponent<StartValues>();
        generator = GameObject.Find("MapGenerator");
        generator.GetComponent<MapGenerator>().DrawMapInEditor();
        Spawner.InstanceCreator(MeshGenerator.HeightMap, MapGenerator.MapHeight, MapGenerator.MapWidth, MapGenerator.Regions);
        Spawner.Instance.SpawnRabbits(sv.rabbitStartCount);
        Spawner.Instance.SpawnFoxes(sv.foxesStartCount);
        Spawner.Instance.SpawnCarrots(sv.carrotSpawnCound);
        Spawner.Instance.SpawnTrees(sv.treeCount);
    }
}
