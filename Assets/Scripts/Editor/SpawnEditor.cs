using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StartValues))]
public class SpawnEditor : Editor
{
   
    public override void OnInspectorGUI()
    {
        StartValues sv = (StartValues)target;

        if(DrawDefaultInspector())
        {
            sv.MyUpdate();
        }

        if (GUILayout.Button("Generate"))
        {
            ArrayToTxt.Do = true;
            if (MeshGenerator.HeightMap == null)
            {
                var generator = GameObject.Find("MapGenerator");
                generator.GetComponent<MapGenerator>().DrawMapInEditor();
            }
            Generate.Generating();
        }
        if (GUILayout.Button("Clear"))
        {
            try
            {
                Spawner.Instance.DeleteCarrotsSpawn();
                Spawner.Instance.DeleteCarrots();
                Spawner.Instance.DeleteFoxes();
                Spawner.Instance.DeleteRabbits();
                Spawner.Instance.DeleteTree();
            }
            catch(NullReferenceException)
            {
                ArrayToTxt.Do = false;
                if (MeshGenerator.HeightMap == null)
                {
                    var generator = GameObject.Find("MapGenerator");
                    generator.GetComponent<MapGenerator>().DrawMapInEditor();
                }
                Spawner.InstanceCreator(MeshGenerator.HeightMap, MapGenerator.MapSize, MapGenerator.MapSize, MapGenerator.Regions);
                Spawner.Instance.DeleteCarrotsSpawn();
                Spawner.Instance.DeleteCarrots();
                Spawner.Instance.DeleteFoxes();
                Spawner.Instance.DeleteRabbits();
                Spawner.Instance.DeleteTree();
            }
        }
    }
}
