﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    bool reSpawn = false;
    GameObject generator;
    void Awake()
    {
        StartValues sv = GameObject.Find("Environment").GetComponent<StartValues>();
        reSpawn = !sv.testCase;
        generator = GameObject.Find("MapGenerator");
        generator.GetComponent<MapGenerator>().DrawMapInEditor();
        if (reSpawn)
        {
            Spawner.InstanceCreator(MeshGenerator.HeightMap, MapGenerator.MapHeight, MapGenerator.MapWidth, MapGenerator.Regions);
            Spawner.Instance.SpawnRabbits(sv.rabbitStartCount);
            Spawner.Instance.SpawnFoxes(sv.foxesStartCount);
            Spawner.Instance.SpawnCarrots(sv.carrotSpawnCound);
            Spawner.Instance.SpawnTrees(sv.treeCount);
        }
        else
        {
            Spawner.InstanceCreator(MeshGenerator.HeightMap, MapGenerator.MapHeight, MapGenerator.MapWidth, MapGenerator.Regions);
            Spawner.Instance.TestSpawn();
        }
        //Debug.Log($"What is on postion = {ArrayModify.TypeField(Spawner.Instance.GenerateMap, 0, 4)}");
        ArrayToTxt.ReadMapArray2D(Spawner.Instance.GenerateMap);
    }
}
