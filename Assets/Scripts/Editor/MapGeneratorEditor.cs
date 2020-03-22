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
            try
            {
                if (Spawner.Instance.GenerateMap != null)
                {
                    ArrayToTxt.StaticReadMapArray2D(Spawner.Instance.GenerateMap);
                }
            }
            catch (NullReferenceException)
            {
                Debug.LogError("Generate population first!");
            }
        }
    }
}
