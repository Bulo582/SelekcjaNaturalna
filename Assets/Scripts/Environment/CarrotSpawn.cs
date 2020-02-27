using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarrotSpawn : MonoBehaviour
{
    GameObject carrotPrefab;
    public float carrotSpawnTime;
    public float carrotSpawnIncease;
    public float currentCarrotSpawnTime;

    private void Start()
    {
        currentCarrotSpawnTime = 0;
        carrotPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Carrot.prefab", typeof(GameObject));
    }
    void Update()
    {
        if(this.gameObject.transform.childCount < 1)
        {
            currentCarrotSpawnTime += carrotSpawnIncease * Time.deltaTime;
            if(currentCarrotSpawnTime >= carrotSpawnTime)
            {
                currentCarrotSpawnTime = carrotSpawnTime;
                GameObject carrot = Instantiate(carrotPrefab, new Vector3(this.gameObject.transform.position.x, 0.2f, this.gameObject.transform.position.z), Quaternion.identity) as GameObject;
                carrot.transform.SetParent(this.gameObject.transform);
            }
        }
    }
}
