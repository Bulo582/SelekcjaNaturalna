using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitIteraionInfo 
{
    public RabbitIteraionInfo() { }
    public RabbitIteraionInfo(string name, Spawner.FamilyRabbit family) 
    {
        info = new RabbitInfo();
        info.name = name;
        info.family = family;
    }

    public RabbitInfo info;

    public void AssignInfo(int iterationWithoutEat, int iterationOfObject, int eatenCarrots)
    {
        info.iterationOfObject = iterationOfObject;
        info.iterationWithoutEat = iterationWithoutEat;
        info.eatenCarrots = eatenCarrots;
    }
}
public struct RabbitInfo
{

    public Spawner.FamilyRabbit family;
    public string name;
    public int iterationWithoutEat;
    public int iterationOfObject;
    public int eatenCarrots;
    //float distanceToTheClosestCarrot;
}

public struct RabbitsIterationInfo
{
    public RabbitsIterationInfo(int popSum, int globalIteration)
    {
        this.currentPopSum = popSum;
        this.currentGlobalIteration = globalIteration;
        this.informations = new RabbitInfo[popSum];
    }

    public RabbitInfo[] informations;
    public int currentGlobalIteration;
    public int currentPopSum;
}


