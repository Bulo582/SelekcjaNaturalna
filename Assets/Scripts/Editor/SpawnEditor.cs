﻿using System;
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
                Spawner.Instance.ResetMapAray();
                Spawner.Instance.DeleteCarrotsSpawn();
                Spawner.Instance.DeleteCarrots();
                Spawner.Instance.DeleteFoxes();
                Spawner.Instance.DeleteRabbits();
                Spawner.Instance.DeleteTree();

            }
            catch(NullReferenceException)
            {
                MapToTxt.Do = false;
                if (MeshGenerator.HeightMap == null)
                {
                    var generator = GameObject.Find("MapGenerator");
                    generator.GetComponent<MapGenerator>().DrawMapInEditor();
                }
                Spawner.InstanceCreator(MeshGenerator.HeightMap, MapGenerator.MapSize, MapGenerator.MapSize, MapGenerator.Regions);
                Spawner.Instance.ResetMapAray();
                Spawner.Instance.DeleteCarrotsSpawn();
                Spawner.Instance.DeleteCarrots();
                Spawner.Instance.DeleteFoxes();
                Spawner.Instance.DeleteRabbits();
                Spawner.Instance.DeleteTree();
                Destroy(GameObject.Find("Environment").GetComponent<Spawner>());
            }
        }
    }
}
