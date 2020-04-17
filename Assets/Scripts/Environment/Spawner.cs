﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using static MapGenerator;

public class Spawner : MonoBehaviour
{
    GameObject rabbitPrefab;
    GameObject carrotPrefab;
    GameObject foxPrefab;
    GameObject treePrefab;
    FamilyRabbit[] familyRabbit; 

    TerrainType[] regions;
    int halfWidthMap; // variable needed to convert transform pos to array pos
    int halfHeightMap; // variable needed to convert transform pos to array pos
    public int HalfWidthMap
    {
        get { return halfWidthMap; }
    }
    public int HalfHeightMap
    {
        get { return halfHeightMap; }
    }

    int rabbitIndex = 0;
    int foxIndex = 0;

    int maxRange; // surface area of map
    internal static readonly float rabbitY = 0.3f;
    internal static readonly float carrotY = 0.2f;
    internal static readonly float treeY = -0.01f;
    internal char[,] originalMap; // map after generated terrain 
    char[,] generateMap; // updatable map for movement of object

    private static Spawner _instance = null;
    public char[,] GenerateMap
    {
        get
        {
            return this.generateMap;
        }
        set
        {
            this.generateMap = value;
        }
    }
    public static void InstanceCreator(float[,] noiseMap, int heightMap, int widthMap, TerrainType[] regions)
    {
        if (_instance == null)
        {
            // _instance = new Spawner(noiseMap, heightMap, widthMap, regions);
            _instance = GameObject.Find("Environment").AddComponent<Spawner>();
            _instance.Initialize(noiseMap, heightMap, widthMap, regions);
        }
        else
            { Debug.Log("Warning: multiple Spawner instace in scene!"); }
    }
    public static Spawner Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    private void Initialize(float[,] noiseMap, int heightMap, int widthMap, TerrainType[] regions)
    {
        familyRabbit = new FamilyRabbit[Generate.sv.familyRabbits.Length];
        for (int i = 0; i < Generate.sv.familyRabbits.Length; i++)
        {
            familyRabbit[i].startPop = Generate.sv.familyRabbits[i].startPop;
            familyRabbit[i].familyID = Generate.sv.familyRabbits[i].familyID;
            familyRabbit[i].familyName = Generate.sv.familyRabbits[i].familyName;
            familyRabbit[i].color = Generate.sv.familyRabbits[i].color;
        }
        this.halfWidthMap = (widthMap - 2) / -2;
        this.halfHeightMap = (heightMap - 2) / 2;
        this.regions = regions;
        maxRange = MapGenerator.MapSize * MapGenerator.MapSize;
        generateMap = ArrayModify.GenerateArray(noiseMap, regions);
        originalMap = ArrayModify.GenerateArray(noiseMap, regions);
        rabbitPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rabbit.prefab", typeof(GameObject));
        carrotPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/CarrotSpawn.prefab", typeof(GameObject));
        foxPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Fox.prefab", typeof(GameObject));
        treePrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tree.prefab", typeof(GameObject));
    }
    public static Vector3 GetLegalVector3(float y = 0.2f)
    {
        int range;
        while(true)
        {
            for (int i = 0; i < Instance.GenerateMap.GetLength(0); i++)
            {
                for (int j = 0; j < Instance.GenerateMap.GetLength(1); j++)
                {
                    if (int.TryParse(Instance.GenerateMap[i, j].ToString(), out int result) && result >= 2)
                    {
                        range = UnityEngine.Random.Range(0, 1000);
                        if (range < 5)
                        {
                            return  new Vector3(Instance.HalfWidthMap + i, y, j - Instance.halfHeightMap);
                        }
                    }
                }
            }
        }
    }

    #region Spawns
    public void TestSpawn() // testCase for specific seed of map
    {
        int rabbitX = 1;
        int rabbitY = 2;
        GameObject rabbit = Instantiate(rabbitPrefab, new Vector3(halfWidthMap + rabbitX, rabbitY, rabbitY - halfHeightMap), Quaternion.identity) as GameObject;
        rabbit.name = "Rabbit" + rabbitIndex;
        rabbit.transform.parent = GameObject.Find("Rabbits").transform;
        generateMap[rabbitX, rabbitY] = 'R';

        int carrotX = 7;
        int carrotY = 3;
        GameObject carriot = Instantiate(carrotPrefab, new Vector3(halfWidthMap + carrotX, rabbitY, carrotY - halfHeightMap), Quaternion.identity) as GameObject;
        carriot.transform.parent = GameObject.Find("Carrots").transform;
        generateMap[carrotX, carrotY] = 'C';
    }
    public void SpawnChildOfRabbit(int x, int y, FamilyRabbit familyRabbit)
    {
        GameObject rabbit = Instantiate(rabbitPrefab, new Vector3(halfWidthMap + x, rabbitY, y - halfHeightMap), Quaternion.identity) as GameObject;
        rabbit.GetComponent<RabbitLife>().FamilyRabbit = familyRabbit;
        rabbit.GetComponent<RabbitLife>().rabbitID = ++Generate.rabbitPopSum;
        rabbit.name = $"Rabbit_{++Generate.generation}";
        rabbit.transform.parent = GameObject.Find("Rabbits").transform;
        rabbit.GetComponent<Movement>().ready = true;
        rabbit.GetComponent<Movement>().populationReady = true;
        generateMap[x, y] = 'R';
        GameManager.logger.PrintLog($"{rabbit.name} Born -- populationReady {PopulationController.IsReady()}");
    }
    public void SpawnRabbits(int count)
    {
        DeleteRabbits();
        int range;
        int indexName = 0;
        int breakout = 0;
        int generated = 0;
        while(true)
        {
            if (count >= generateMap.Length)
                break;
            breakout++;
            if (breakout > maxRange)
                generated = breakout;

            for (int i = 0; i < generateMap.GetLength(0); i++)
            {
                for (int j = 0; j < generateMap.GetLength(1); j++)
                {
                    if (int.TryParse(generateMap[i, j].ToString(), out int result) && result >= 2)
                    {
                        range = UnityEngine.Random.Range(0, maxRange);
                        if (range == 0)
                        {
                            if (familyRabbit[rabbitIndex].startPop > 0)
                            {
                                Generate.generation++;
                                indexName++;
                                GameObject rabbit = Instantiate(rabbitPrefab, new Vector3(halfWidthMap + i, rabbitY, j - halfHeightMap), Quaternion.identity) as GameObject;
                                familyRabbit[rabbitIndex].startPop--;
                                rabbit.GetComponent<RabbitLife>().FamilyRabbit = familyRabbit[rabbitIndex];
                                rabbit.name = $"Rabbit_{indexName}";
                                rabbit.GetComponent<RabbitLife>().rabbitID = indexName;
                                rabbit.transform.parent = GameObject.Find("Rabbits").transform;
                                generateMap[i, j] = 'R';
                                generated++;
                            }
                            else
                                rabbitIndex++;
                        }
                    }
                    if (generated >= count)
                        break;
                }
                if (generated >= count)
                    break;
            }
            if (generated >= count)
                break;
        }
        //Debug
        //ArrayToTxt.ReadMapArray2D(generateMap);
    }
    public void SpawnCarrots(int count)
    {
        DeleteCarrotsSpawn();
        DeleteCarrots();
        int range;
        int generated = 0;
        int breakout = 0;
        while (true)
        {
            if (count >= generateMap.Length)
                break;
            breakout++;
            if (breakout > Mathf.Abs(generateMap.Length))
                generated = breakout;
            for (int i = 0; i < generateMap.GetLength(0); i++)
            {
                for (int j = 0; j < generateMap.GetLength(1); j++)
                {
                    if (int.TryParse(generateMap[i, j].ToString(), out int result) && result >= 3)
                    {
                        range = UnityEngine.Random.Range(0, maxRange);
                        if (range < 2)
                        {
                            if (true)
                            {
                                GameObject carriot = Instantiate(carrotPrefab, new Vector3(halfWidthMap + i, carrotY, j - halfHeightMap), Quaternion.identity) as GameObject;
                                carriot.transform.parent = GameObject.Find("Carrots").transform;
                                generateMap[i, j] = 'C';
                                generated++;
                            }

                        }
                    }
                    if (generated >= count)
                        break;
                }
                if (generated >= count)
                    break;
            }
            if (generated >= count)
                break;
        }
    }
    public void SpawnTrees(int count)
    {
        DeleteTree();
        int range;
        int generated = 0;
        int breakout = 0;
        while (true)
        {
            if (count >= generateMap.Length)
                break;
            breakout++;
            if (breakout > generateMap.Length * 100)
                generated = breakout;
            for (int i = 0; i < generateMap.GetLength(0); i++)
            {
                for (int j = 0; j < generateMap.GetLength(1); j++)
                {
                    if (int.TryParse(generateMap[i, j].ToString(), out int result) && result >= 4)
                    {
                        range = UnityEngine.Random.Range(0, 100);
                        if (range < 1)
                        {
                            GameObject tree = Instantiate(treePrefab, new Vector3(halfWidthMap + i, treeY, j - halfHeightMap), Quaternion.identity) as GameObject;
                            float scale = UnityEngine.Random.Range(1f, 4.5f);
                            tree.transform.localScale = new Vector3(scale, scale, scale);
                            tree.transform.parent = GameObject.Find("Trees").transform;
                            generateMap[i, j] = 'T';
                            generated++;
                        }
                    }
                    if (generated >= count)
                        break;
                }
                if (generated >= count)
                    break;
            }
            if (generated >= count)
                break;
        }
    }
    public void SpawnFoxes(int count)
    {
        DeleteFoxes();
        int range;
        int generated = 0;
        int breakout = 0;
        while (true)
        {
            if (count >= generateMap.Length)
                break;
            breakout++;
            if (breakout > generateMap.Length)
                generated = breakout;
            for (int i = 0; i < generateMap.GetLength(0); i++)
            {
                for (int j = 0; j < generateMap.GetLength(1); j++)
                {
                    if (int.TryParse(generateMap[i, j].ToString(), out int result) && result >= 4)
                    {
                        range = UnityEngine.Random.Range(0, 100);
                        if (range < 1)
                        {
                            if (true)
                            {
                                foxIndex++;
                                GameObject fox = Instantiate(foxPrefab, new Vector3(halfWidthMap + i, 0.3f, j - halfHeightMap), Quaternion.identity) as GameObject;
                                fox.name = "Fox" + foxIndex;
                                fox.transform.parent = GameObject.Find("Foxes").transform;
                                generateMap[i, j] = 'F';
                                generated++;
                            }

                        }
                    }
                    if (generated >= count)
                        break;
                }
                if (generated >= count)
                    break;
            }
            if (generated >= count)
                break;
        }
    }
    #endregion
    #region Deletes

    public void DeleteFoxes()
    {
        //Debug.Log("DeleteFoxes");
        var gameObjects = GameObject.FindGameObjectsWithTag("Fox");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            DestroyImmediate(gameObjects[i]);
        }
    }
    public void DeleteRabbits()
    {
        //Debug.Log("DeleteRabbits");
        var gameObjects = GameObject.FindGameObjectsWithTag("Rabbit");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            DestroyImmediate(gameObjects[i]);
        }
    }
    public void DeleteTree()
    {
        //Debug.Log("DeleteTrees");
        var gameObjects = GameObject.FindGameObjectsWithTag("Tree");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            DestroyImmediate(gameObjects[i]);
        }
    }
    public void DeleteCarrots()
    {
        //Debug.Log("DeleteCarrots");
        var gameObjects = GameObject.FindGameObjectsWithTag("Carrot");
        for (var i = 0; i < gameObjects.Length; i++)
        {
            DestroyImmediate(gameObjects[i]);
        }
    }
    public void DeleteCarrotsSpawn()
    {
        //Debug.Log("DeleteCarrotsSpawn");
        var gameObjects = GameObject.FindGameObjectsWithTag("CarrotSpawn");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            DestroyImmediate(gameObjects[i]);
        }
    }

    #endregion

    [System.Serializable]
    public struct FamilyRabbit
    {
        public int familyID;
        public string familyName;
        public Color color;
        public int startPop;
    }
}
