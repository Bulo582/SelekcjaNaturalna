using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    GameObject generator;
    void Start()
    {
        generator = GameObject.Find("MapGenerator");
        generator.GetComponent<MapGenerator>().DrawMapInEditor();
    }
}
