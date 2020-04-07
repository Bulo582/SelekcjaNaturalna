using System.Collections;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{ 
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.DrawMapInEditor();
            }
        }
        if(GUILayout.Button("Generate"))
        {
            ArrayToTxt.Do = false;
            mapGen.DrawMapInEditor();
        }
        if(GUILayout.Button("TXT"))
        {
            ArrayToTxt.Do = true;
            if (MeshGenerator.HeightMap == null)
            {
                var generator = GameObject.Find("MapGenerator");
                generator.GetComponent<MapGenerator>().DrawMapInEditor();
                
            }
            Generate.Generating();
            Spawner.InstanceCreator(MeshGenerator.HeightMap, MapGenerator.MapSize, MapGenerator.MapSize, MapGenerator.Regions);
            Spawner.Instance.DeleteCarrotsSpawn();
            Spawner.Instance.DeleteCarrots();
            Spawner.Instance.DeleteFoxes();
            Spawner.Instance.DeleteRabbits();
            Spawner.Instance.DeleteTree();
            if (Spawner.Instance.GenerateMap != null)
            {
                ArrayToTxt.StaticReadMapArray2D(Spawner.Instance.GenerateMap);
                //ArrayToTxt.HeightToFile(MeshGenerator.HeightMap, "Before2", "F3",false);
                //ArrayToTxt.HeightToFile(MeshGenerator.HeightMap, "AfterOperation", "F3",false);
            }

        }
    }
}
