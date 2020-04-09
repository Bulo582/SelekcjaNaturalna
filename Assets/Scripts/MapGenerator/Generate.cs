using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    static bool reSpawn = false;
    static GameObject generator;
    public static StartValues sv;
    public static int RabbitPopSum;
    public static int freeFields;
     public static int generation = 0;
    void Awake()
    {
        Generating();
    }
    
    public static void Generating()
    {
        sv = GameObject.Find("Environment").GetComponent<StartValues>();
        RabbitPopSum = sv.FamilyPopSum();
        reSpawn = !sv.testCase;
        generator = GameObject.Find("MapGenerator");
        generator.GetComponent<MapGenerator>().DrawMapInEditor();
        if (reSpawn)
        {
            Spawner.InstanceCreator(MeshGenerator.HeightMap, MapGenerator.MapSize, MapGenerator.MapSize, MapGenerator.Regions);
            freeFields = MapHelper.MapFreeFieldCount();
            Spawner.Instance.SpawnRabbits(RabbitPopSum);
            //Spawner.Instance.SpawnFoxes(sv.foxesStartCount);
            Spawner.Instance.SpawnCarrots(sv.carrotSpawnCound);
            Spawner.Instance.SpawnTrees(sv.treeCount);
        }
        else
        {
            Spawner.InstanceCreator(MeshGenerator.HeightMap, MapGenerator.MapSize, MapGenerator.MapSize, MapGenerator.Regions);
            Spawner.Instance.TestSpawn();
        }
        //Debug.Log($"What is on postion = {ArrayModify.TypeField(Spawner.Instance.GenerateMap, 0, 4)}");
        ArrayToTxt.StaticReadMapArray2D(Spawner.Instance.GenerateMap);
        sv.MyUpdate();
    }

}
