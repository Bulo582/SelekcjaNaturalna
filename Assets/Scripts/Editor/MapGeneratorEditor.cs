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
            //ArrayToTxt.ReadMapArray2D(Spawner.Instance.GenerateMap);
            Debug.Log(ArrayModify.TypeField(Spawner.Instance.GenerateMap, 0, 4));
        }
    }
}
