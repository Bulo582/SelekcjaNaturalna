using System.Collections;
using System.Collections.Generic;
using System.IO;

using System;
using UnityEngine;
using static MapGenerator;

public class ArrayToTxt 
{
    static string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    static string path = Path.Combine(desktop, "TestLand");
    public static bool Do = true;
    string folderName;

    public ArrayToTxt(string folderName)
    {
        this.folderName = folderName;
        if (folderName == "")
            folderName = "Unknow";
        FileHelper.CreateFolder(path, folderName);
    }

    public  void ThrowLogToFile(string alias, string log)
    {
        if (Do)
        {
            string filePath;
            if (alias != "")
                filePath = Path.Combine(path, folderName, $"Log{alias}.txt");
            else
                filePath = Path.Combine(path, $"Log{alias}.txt");
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(log);
            }
        }
    }

    public static float[,] LoadHeightFromFile(string filename)
    {
        float[,] array = null;
        string filePath = Path.Combine(path, filename);
        string line;
        using (StreamReader sr = new StreamReader(filePath))
        {
            int X = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (X == 0)
                {
                    int x = int.Parse(line.Substring(1, 2));
                    int y = int.Parse(line.Substring(4, 2));
                    array = new float[x, y];
                    line = sr.ReadLine();
                }
                int Y = 0;
                foreach (var item in line.Split('-'))
                {
                    try
                    {
                        array[X, Y] = float.Parse(item);
                        Y++;
                    }
                    catch (FormatException)
                    {
                        continue;
                    }
                }
                X++;
            }
        }
        return array;
    }
    public static void HeightToFile(float[,] heightMap, string alias = "",string format = "F4", bool MapMode = false)
    {
        heightMap = ArrayModify.RewriteLastXYOneBefore(heightMap);
        if(MapMode)
        {
            heightMap = ArrayModify.ArrayReverseY(heightMap);
            //heightMap = ArrayModify.ArrayCutXY(heightMap);
            heightMap = ArrayModify.ArrayReverseX(heightMap);
            heightMap = ArrayModify.ArrayReverseY(heightMap);
        }
        if(Do)
        {
            string filePath = Path.Combine(path, $"floatArray2D{alias}.txt");
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"[{heightMap.GetLength(0)},{heightMap.GetLength(1)}]");
                    for (int x = 0; x < heightMap.GetLength(0); x++)
                    {
                        for (int y = 0; y < heightMap.GetLength(1); y++)
                        {
                            sw.Write(heightMap[x, y].ToString(format) + "-");
                        }
                        sw.WriteLine();
                    }
                }
                UnityEngine.Debug.Log("FileCreated - float[.]");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
            }
        }
    }
    public static void VectorsToFile(Vector3[] array)
    {
        if (Do)
        {
            string filePath = Path.Combine(path, "Vectors.txt");
            List<float> yExist = new List<float>();
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"[] {DateTime.Now}");
                    for (int x = 0; x < array.GetLength(0); x++)
                    {
                        if(!yExist.Contains(array[x].y))
                        {
                            yExist.Add(array[x].y);
                            sw.WriteLine(array[x].y);
                        }
                    }
                }
                UnityEngine.Debug.Log("FileCreated - Vectors");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
            }
        }
    }
    public static void HeightToBool(float[,] heightMap, float min, float max)
    {
        if (Do)
        {
            string filePath = Path.Combine(path, "HeightBool.txt");
            heightMap = ArrayModify.ArrayReverseY(heightMap);
            heightMap = ArrayModify.ArrayCutXY(heightMap);
            heightMap = ArrayModify.ArrayReverseX(heightMap);
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"[{min};{max}] {DateTime.Now}");
                    for (int x = 0; x < heightMap.GetLength(0); x++)
                    {
                        if (x == 0)
                        {
                            for (int k = 0; k < heightMap.GetLength(1); k++)
                            {
                                if(k < 9)
                                    sw.Write((k + 1) + " ");
                                else
                                    sw.Write((k + 1));
                            }
                            sw.WriteLine();
                        }

                        for (int y = 0; y < heightMap.GetLength(1); y++)
                        {
                            if (heightMap[x, y] > min && heightMap[x, y] <= max)
                            {
                                sw.Write($"1-");
                                
                            }
                            else
                            {
                                sw.Write("0-");
                            }

                            if (y == heightMap.GetLength(1) - 1)
                                sw.Write(" | "+ (x+1));
                        }
                        sw.WriteLine();
                        
                    }
                }
                UnityEngine.Debug.Log("FileCreated - Vectors");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
            }
        }
    }
    public static void GenerateRabbitFields(float[,] array , float min, float max, int population)
    {
        if (Do)
        {
            
            int Ax = array.GetLength(0);
            int Ay = array.GetLength(1);
            int fields = Ax * Ay;
            string filePath = Path.Combine(path, "Rabbits.txt");
            if (population > fields)
                throw new Exception("population is too large");
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"[{min};{max}]  {DateTime.Now}");
                    for (int x = 0; x < array.GetLength(0); x++)
                    {
                        for (int y = 0; y < array.GetLength(1); y++)
                        {
                            if (array[x, y] > min && array[x, y] <= max)
                            {

                            }
                                
                            else
                                sw.Write("0-");
                        }
                        sw.WriteLine();
                    }
                }
                UnityEngine.Debug.Log("FileCreated - height[01]");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
            }
        }
    }
    public static void ReadFloatArray2D(float[,] array)
    {
        if (Do)
        {
            string filePath = Path.Combine(path, "Reader.txt");
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {

                        sw.Write($"{array[i, j]} -");

                    }

                }
                sw.WriteLine();
            }
        }
    }
    public void ReadMapArray2D(char[,] array, string alias = "")
    {
        string filePath;
        if (alias != "")
            filePath = Path.Combine(path, folderName,$"Map{alias}.txt");
        else
            filePath = Path.Combine(path, $"Map{alias}.txt");

        if (Do)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"{array.GetLength(0)}-{array.GetLength(1)}");
                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        for (int j = 0; j < array.GetLength(1); j++)
                        {
                            sw.Write($"{array[i, j]} -");
                        }
                        sw.WriteLine();
                    }
                    
                }
                UnityEngine.Debug.Log("FileCreated - Map");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
            }
        }
    }
    public static void StaticReadMapArray2D(char[,] array, string alias = "")
    {
        string filePath;
        filePath = Path.Combine(path, $"Map{alias}.txt");

        if (Do)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"{array.GetLength(0)}-{array.GetLength(1)}");
                    for (int i = 0; i < array.GetLength(0); i++)
                    {
                        for (int j = 0; j < array.GetLength(1); j++)
                        {
                            sw.Write($"{array[i, j]} -");
                        }
                        sw.WriteLine();
                    }

                }
                UnityEngine.Debug.Log("FileCreated - Map");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
            }
        }
    }

    public static string TestFolder
    {
        get { return path; }
    }
}
