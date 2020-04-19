using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarrotSpawn : MonoBehaviour
{
    int arrayPozX;
    int arrayPozY;
    int iterationOnDead;
    public int IterationOnDead
    {
        get {return this.iterationOnDead; }
        set 
        { 
            iterationOnDead = value;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = Spawner.Instance.originalMap[arrayPozX, arrayPozY];
        }
    }
    public int iterationToRespawn;
    public int IterationOnRespawn
    {
        get { return iterationOnDead + iterationToRespawn; }
    }

    private void Start()
    {
        iterationToRespawn = StartCarrot.Manager.iterationToRespawn;
        arrayPozX = MapHelper.TransormX_ToMapX(this.transform.position.x);
        arrayPozY = MapHelper.TransormZ_ToMapY(this.transform.position.z);
    }
    void FixedUpdate()
    {
        if(!this.gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            if (IterationOnRespawn <= Iteration.Global)
            {
                this.gameObject.transform.position = Spawner.Instance.GetLegalVector3();
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                arrayPozX = MapHelper.TransormX_ToMapX(transform.position.x);
                arrayPozY = MapHelper.TransormZ_ToMapY(transform.position.z);
                Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = StartCarrot.sign;
            }
        }
    }
}
