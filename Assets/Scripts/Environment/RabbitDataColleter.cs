using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitDataColleter
{
    public RabbitDataColleter(Spawner.FamilyRabbit family)
    {
        represent = family;
    }

    Spawner.FamilyRabbit represent;
    int globalIteration => Movement.globalIteration;
    int popSum => Generate.rabbitPopSum;
    Dictionary<RabbitLife, IterationRabbitInfo> BookOfKnowledge = new Dictionary<RabbitLife, IterationRabbitInfo>();

    public void CollectingInfo()
    {

    }
    public void AddNewRabbit(RabbitLife rabbit)
    {
        BookOfKnowledge.Add(rabbit, new IterationRabbitInfo());
    }
    public void RemoveDeadRabbit(RabbitLife rabbit)
    {
        BookOfKnowledge.Remove(rabbit);
    }
}

struct IterationRabbitInfo
{
    int iterationOfLife;
    int globalIteration;
    // -----------------
    int totalyEatenCarrot;
    int iterationWithOutEat;
    float distanceToTheClosestCarrot;
}
