using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarrotSpawn : MonoBehaviour
{
    public float carrotSpawnTime;
    public float carrotSpawnIncease;
    public float currentCarrotSpawnTime;

    private void Start()
    {
        currentCarrotSpawnTime = 0;
    }
    void Update()
    {
        if(!this.gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            currentCarrotSpawnTime += carrotSpawnIncease * Time.deltaTime;
            if(currentCarrotSpawnTime >= carrotSpawnTime)
            {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                currentCarrotSpawnTime = 0;
            }
        }
    }
}
