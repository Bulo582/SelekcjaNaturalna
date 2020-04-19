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
            MapToTxt.Do = false;
            mapGen.DrawMapInEditor();
        }
        if(GUILayout.Button("TXT"))
        {
            MapToTxt.Do = true;
            if (MeshGenerator.HeightMap == null)
            {
                var generator = GameObject.Find("MapGenerator");
                generator.GetComponent<MapGenerator>().DrawMapInEditor();
            }

            if (Spawner.Instance.GenerateMap != null)
            {
                GameManager.mapToTXTprinter.StaticReadMapArray2D(Spawner.Instance.GenerateMap);
                //ArrayToTxt.HeightToFile(MeshGenerator.HeightMap, "Before2", "F3",false);
                //ArrayToTxt.HeightToFile(MeshGenerator.HeightMap, "AfterOperation", "F3",false);
            }

        }
    }
}
