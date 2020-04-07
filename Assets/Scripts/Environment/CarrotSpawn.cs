using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarrotSpawn : MonoBehaviour
{
    public int iterationOnDead;
    public int iterationToRespawn;
    public int IterationOnRespawn
    {
        get { return iterationOnDead + iterationToRespawn; }
    }

    private void Start()
    {
        iterationToRespawn = StartCarrot.Manager.iterationToRespawn;
    }
    void Update()
    {
        if(!this.gameObject.transform.GetChild(0).gameObject.activeSelf)
        {
            if (IterationOnRespawn == Movement.globalIteration)
            {
                this.gameObject.transform.position = Spawner.GetLegalVector3();
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
