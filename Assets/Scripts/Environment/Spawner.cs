using System.Collections;
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
    int halfWidthMap;
    int halfHeightMap;
    char[,] generateMap;
    TerrainType[] regions;
    int rabbitIndex = 0;
    int foxIndex = 0;

    bool[] rabbitsField;
    bool[] foxsField;
    bool[] treesField;
    bool[] plantsFields;

    public char[,] GenerateMap
    {
        get
        {
            return this.generateMap;
        }
    }

    private static Spawner _instance = null;

    public static void InstanceCreator(float[,] noiseMap, int heightMap, int widthMap, TerrainType[] regions)
    {
        if (_instance == null)
        {
            _instance = new Spawner(noiseMap, heightMap, widthMap, regions);
        }
    }

    public static Spawner Instance
    {
        get
        {
            return _instance;
        }
    }

    public int HalfWidthMap
    {
        get { return halfWidthMap; }
    }
    public int HalfHeightMap 
    { 
        get { return halfHeightMap; }
    }

    public Spawner(float[,] noiseMap, int heightMap, int widthMap, TerrainType[] regions)
    {
        this.halfWidthMap = (widthMap - 2) / -2;
        this.halfHeightMap = (heightMap - 2) / 2;
        this.regions = regions;
        generateMap = ArrayModify.GenerateArray(noiseMap,regions);
        
        //ArrayToTxt.CircleChechout(ref generateMap, 0, 0, 10, 'C');
        rabbitPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Rabbit.prefab", typeof(GameObject));
        carrotPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/CarrotSpawn.prefab", typeof(GameObject));
        foxPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Fox.prefab", typeof(GameObject));
        treePrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tree.prefab", typeof(GameObject));
    }
    #region Spawns
    public void TestSpawn()
    {
        GameObject rabbit = Instantiate(rabbitPrefab, new Vector3(halfWidthMap +1, 0.3f, 0 - halfHeightMap), Quaternion.identity) as GameObject;
        rabbit.name = "Rabbit" + rabbitIndex;
        rabbit.transform.parent = GameObject.Find("Rabbits").transform;
        generateMap[1, 0] = 'R';

        GameObject carriot = Instantiate(carrotPrefab, new Vector3(halfWidthMap + 2, 0.2f, 0 - halfHeightMap), Quaternion.identity) as GameObject;
        carriot.transform.parent = GameObject.Find("Carrots").transform;
        generateMap[2, 0] = 'C';
    }
    public void SpawnRabbits(int count)
    {
        DeleteRabbits();
        int range;
        int breakout = 0;
        int generated = 0;
    while(true)
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
                    if (int.TryParse(generateMap[i, j].ToString(), out int result) && result >= 2)
                    {
                        range = UnityEngine.Random.Range(0, 100);
                        if (range < 1)
                        {
                            rabbitIndex++;
                            GameObject rabbit = Instantiate(rabbitPrefab, new Vector3(halfWidthMap + i, 0.3f, j - halfHeightMap), Quaternion.identity) as GameObject;
                            rabbit.name = "Rabbit" + rabbitIndex;
                            rabbit.transform.parent = GameObject.Find("Rabbits").transform;
                            generateMap[i, j] = 'R';
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
            if (breakout > generateMap.Length)
                generated = breakout;
            for (int i = 0; i < generateMap.GetLength(0); i++)
            {
                for (int j = 0; j < generateMap.GetLength(1); j++)
                {
                    if (int.TryParse(generateMap[i, j].ToString(), out int result) && result >= 3)
                    {
                        range = UnityEngine.Random.Range(0, 100);
                        if (range < 1)
                        {
                            if (true)
                            {
                                GameObject carriot = Instantiate(carrotPrefab, new Vector3(halfWidthMap + i, 0.2f, j - halfHeightMap), Quaternion.identity) as GameObject;
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
                                GameObject tree = Instantiate(treePrefab, new Vector3(halfWidthMap + i, 0.3f, j - halfHeightMap), Quaternion.identity) as GameObject;
                                float scale = UnityEngine.Random.Range(1f, 4.5f);
                                tree.transform.localScale = new Vector3(scale, scale, scale);
                                tree.transform.parent = GameObject.Find("Trees").transform;
                                generateMap[i, j] = 'T';
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
}
