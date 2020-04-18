using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyIterationInfo : RabbitIteraionInfo
{
    private FamilyIterationInfo()
    {
        info = new FamiliyInfo[Generate.sv.RabbitFamilyCount];
        for (int i = 0; i < info.Length; i++)
        {
            info[i].family = Generate.sv.familyRabbits[i];
            info[i].isAlive = true;
            info[i].allLivenObject = Generate.sv.familyRabbits[i].startPop;
        }
    }

    private static FamilyIterationInfo instance;

    public static FamilyIterationInfo Instance
    {
        get
        {
            if (instance != null)
                return instance;
            else
            {
                instance = new FamilyIterationInfo();
                return instance;
            }
        }
        private set
        {
            instance = value;
        }
    }


    public new FamiliyInfo[] info;

    public void AssignInfo(int idx, int memberCount)
    {
        this.info[idx].memberCount = memberCount;
    }

    public void AssignInfo(int idx)
    {
        this.info[idx].isAlive = false;
        this.info[idx].memberCount = 0;
    }

    public void NewRabbit(int idxFamily)
    {
        info[idxFamily].allLivenObject++;
    }

    public void NextIterationOfObject(int idxFamily)
    {
        info[idxFamily].iterationSum++;
    }

    public void NextMealOfObject(int idxFamily)
    {
        info[idxFamily].eatenCarrotSum++;
    }

}
public struct FamiliyInfo
{
    public Spawner.FamilyRabbit family;
    public int memberCount;
    public int iterationSum;
    public int eatenCarrotSum;
    public int allLivenObject;
    public bool isAlive
    {
        get { return this._isAlive; }
        set 
        {
            _isAlive = value;
            if (!value)
                iterationOnDead = Iteration.Global;
        }
    }
    private int iterationOnDead;
    private bool _isAlive;
    //-------------------------
}

public struct FamiliesIterationInfo
{
    public FamiliesIterationInfo(int popSum, int globalIteration)
    {
        this.currentPopSum = popSum;
        this.currentGlobalIteration = globalIteration;
        informations = new FamiliyInfo[Generate.sv.RabbitFamilyCount];
    }

    public void AssignInfo(FamiliyInfo[] _informations)
    {
        for (int i = 0; i < _informations.Length; i++)
        {
            informations[i].allLivenObject = _informations[i].allLivenObject;
            informations[i].eatenCarrotSum = _informations[i].eatenCarrotSum;
            informations[i].iterationSum = _informations[i].iterationSum;
            informations[i].memberCount = _informations[i].memberCount;
            informations[i].isAlive = _informations[i].isAlive;
        }
    }

    public FamiliyInfo[] informations;
    public int currentGlobalIteration;
    public int currentPopSum;
}
