using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapGenerator;

public static class MeshGenerator
{
    public static float[,] HeightMap;
    static float[,] noiseMap; 
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail, TerrainType[] regions)
    {
        HeightMap = heightMap;
        noiseMap = heightMap;
        //noiseMap = ArrayToTxt.HeightRound(heightMap, regions);
        //ArrayToTxt.HeightToFile(noiseMap, "C", "F1");
        //noiseMap = ArrayToTxt.LoadHeightFromFile("floatArray2DC.txt");
        int wight = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (wight - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int meshSimplificationIncrement = (levelOfDetail == 0)? 1 : levelOfDetail * 2;
        int verticesPerline = (wight - 1) / meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(wight, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++ )
        {
            for (int x = 0; x < wight; x++ )
            {
                meshData.verticles[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(noiseMap[x,y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)wight, y / (float)height);

                if(x < wight - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerline + 1, vertexIndex + wight);
                    meshData.AddTriangle(vertexIndex + verticesPerline + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return meshData;
    }
}

public class MeshData
{
    public Vector3[] verticles;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        verticles = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth -1) * (meshHeight -1) * 6];
    }
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticles;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
