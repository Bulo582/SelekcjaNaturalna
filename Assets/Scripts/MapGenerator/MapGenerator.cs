using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static int MapSize;
    public static TerrainType[] Regions;
    public enum DrawMode
    {
        NoiseMap, ColourMap, Mesh
    }
    public DrawMode drawMode;

    public int mapSize;

    public int mapWidth { get => mapSize; }
    public int mapHeight { get => mapSize; }

    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplayer;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void DrawMapInEditor()
    {
        if (mapSize % 2 != 0)
            mapSize--;
        MapSize = mapSize;
        MapData mapData = GenerateMapData();
        Regions = regions;
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        else if (drawMode == DrawMode.ColourMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.colorMap, mapWidth, mapWidth));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplayer, meshHeightCurve, levelOfDetail, regions), TextureGenerator.TextureFromColorMap(mapData.colorMap, mapWidth, mapWidth));
    }

    public MapData GenerateMapData()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        noiseMap = ArrayModify.RewriteLastXYOneBefore(noiseMap);
        Color[] colourMap = new Color[mapHeight * mapWidth];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colourMap [y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        return new MapData(noiseMap, colourMap);
    }

    private void OnValidate()
    {
        if(lacunarity < 1) {
            lacunarity = 1;
        }
        if(octaves < 0) { 
            octaves = 0;
        }
        if(meshHeightMultiplayer <= 0)
        {
            meshHeightMultiplayer = 0.1f;
        }
    }
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color colour;
    }
    public struct MapData
    {
        public float[,] heightMap;
        public Color[] colorMap;

        public MapData(float[,] heightMap, Color[] colorMap)
        {
            this.heightMap = heightMap;
            this.colorMap = colorMap;
        }
    }
}
