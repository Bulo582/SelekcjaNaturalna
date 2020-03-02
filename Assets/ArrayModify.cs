using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapGenerator;

public class ArrayModify 
{

    public static string TypeField(char[,] array, int x, int z)
    {
        return array[x, z].ToString();
    }

    public static void CircleChechout(ref char[,] array, int mX, int mY, int r, char region)
    {
        if (array is null)
            throw new Exception("CircleCheckout NullReference Exception");
        if ((mX > array.GetLength(1) && mX < 0) || (mY > array.GetLength(0) || mY < 0))
            throw new Exception("Mid point has coordinate incorrect");

        int arrayX = array.GetLength(1);
        int arrayY = array.GetLength(0);

        int dX = 0 - r + mX;
        int dY = 0 - r + mY;

        int x;
        int y;

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                x = j - r;
                y = i - r;
                if ((x * x + y * y) <= r * r + 1)
                {
                    try
                    {
                        if (array[i + dY, j + dX] != 'X')
                            array[i + dY, j + dX] = region;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }
                }
            }
        }
    }
    public static float[,] ArrayReverseY(float[,] oldArray)
    {
        float[,] newArray = new float[oldArray.GetLength(0), oldArray.GetLength(1)];
        int x = 0;
        for (int i = 0; i < newArray.GetLength(0); i++)
        {
            x = newArray.GetLength(0) - 1 - i;
            for (int j = 0; j < newArray.GetLength(1); j++)
            {
                newArray[i, j] = oldArray[x, j];
            }
        }
        return newArray;

    }
    public static float[,] ArrayReverseX(float[,] oldArray)
    {
        float[,] newArray = new float[oldArray.GetLength(0), oldArray.GetLength(1)];
        int y = 0;
        for (int i = 0; i < newArray.GetLength(0); i++)
        {
            for (int j = 0; j < newArray.GetLength(1); j++)
            {
                y = newArray.GetLength(1) - 1 - j;
                newArray[i, j] = oldArray[i, y];
            }
        }
        return newArray;

    }
    public static float[,] ArrayCutXY(float[,] oldArray)
    {
        float[,] newArray = new float[oldArray.GetLength(0) - 1, oldArray.GetLength(1) - 1];
        for (int i = 0; i < newArray.GetLength(0); i++)
        {
            for (int j = 0; j < newArray.GetLength(1); j++)
            {
                newArray[i, j] = oldArray[i + 1, (j != newArray.GetLength(1) - 1) ? j : j + 1];
            }
        }
        return newArray;
    }
    public static bool[,] AbleToGenerateArray(float[,] heightMap, float min, float max)
    {
        bool[,] boolMap = new bool[heightMap.GetLength(0), heightMap.GetLength(1)];
        heightMap = ArrayReverseY(heightMap);
        heightMap = ArrayCutXY(heightMap);
        heightMap = ArrayReverseX(heightMap);
        heightMap = ArrayReverseY(heightMap);
        string log = "";
        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                if (heightMap[x, y] > min && heightMap[x, y] <= max)
                {
                    log += "1-";
                    boolMap[x, y] = true;
                }
                else
                {
                    boolMap[x, y] = false;
                    log += "0-";
                }
            }
            log = "";
        }
        return boolMap;
    }
    public static char[,] GenerateArray(float[,] heightMap, TerrainType[] regions)
    {

        char[,] generateMap = new char[heightMap.GetLength(0), heightMap.GetLength(1)];
        heightMap = ArrayReverseY(heightMap);
        heightMap = ArrayCutXY(heightMap);
        heightMap = ArrayReverseX(heightMap);
        heightMap = ArrayReverseY(heightMap);
        string log = "";
        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    if ((heightMap[x, y] <= regions[i].height))
                    {

                        if (regions[i].name == "DeepWather" || regions[i].name == "Wather")
                        {
                            generateMap[x, y] = 'X';
                            log += generateMap[x, y] + "-";
                            break;
                        }
                        generateMap[x, y] = Convert.ToChar(i.ToString());
                        log += generateMap[x, y] + "-";
                        break;
                    }

                }
            }
            //Debug.Log(log);
            log = "";
        }
        return generateMap;
    }
    public static float[,] HeightRound(float[,] heightMap, TerrainType[] regions)
    {
        string log = "";
        float[,] array = new float[heightMap.GetLength(0), heightMap.GetLength(1)];
        for (int x = 0; x < heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < heightMap.GetLength(1); y++)
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    if ((heightMap[x, y] <= regions[i].height))
                    {
                        log += heightMap[x, y] + " ";
                        if (regions[i].name == "DeepWather" || regions[i].name == "Wather")
                        {
                            array[x, y] = 0.1f;
                            log += "Sea";
                            break;
                        }
                        log += "Ground";
                        array[x, y] = 0.2f;
                        break;
                    }

                }
                //Debug.Log(log);
                log = "";
            }
        }
        return array;
    }
}
