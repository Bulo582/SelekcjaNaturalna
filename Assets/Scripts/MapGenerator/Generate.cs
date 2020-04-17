using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    static bool reSpawn = false;
    internal static StartValues sv;

    internal static int rabbitPopSum;
    internal static int freeFields = 0;
    internal static int generation = 0;
    void Awake()
    {
        Generating();
    }
    public static void Generating()
    {
        sv = GameObject.Find("Environment").GetComponent<StartValues>();
        rabbitPopSum = sv.FamilyPopSum();
        reSpawn = !sv.testCase;
        sv.MyUpdate();
        GameObject.Find("MapGenerator").GetComponent<MapGenerator>().DrawMapInEditor();
        if (reSpawn)
        {
            Spawner.InstanceCreator(MeshGenerator.HeightMap, MapGenerator.MapSize, MapGenerator.MapSize, MapGenerator.Regions);
            freeFields = MapHelper.MapFreeFieldCount();
            Spawner.Instance.SpawnRabbits(rabbitPopSum);
            //Spawner.Instance.SpawnFoxes(sv.foxesStartCount);
            Spawner.Instance.SpawnCarrots(sv.carrotSpawnCount);
            Spawner.Instance.SpawnTrees(sv.treeCount);
        }
        else
        {
            Spawner.InstanceCreator(MeshGenerator.HeightMap, MapGenerator.MapSize, MapGenerator.MapSize, MapGenerator.Regions);
            Spawner.Instance.TestSpawn();
        }
        GameManager.mapToTXTprinter.StaticReadMapArray2D(Spawner.Instance.GenerateMap);
    }

}
