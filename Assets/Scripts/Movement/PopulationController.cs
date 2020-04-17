using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction
{
    up = 1,
    right = 2,
    down = 3,
    left = 4,
    none = 5
}
public class PopulationController
{
    public static List<Movement> Creatures = new List<Movement>();
    public static bool IsReady()
    {
        return Creatures.All(l => l.ready == true);
    }
    public static bool IsNotReady()
    {
        return Creatures.All(l => l.ready == false);
    }
    public static void NumeringPopulation()
    {
        if (Creatures != null)
        {
            int idx = 1;
            foreach (var item in Creatures)
            {
                item.rabbitLife.rabbitID = idx;
                idx++;
            }
        }
    }

    public static void CollectingRabbitInfo() // sprawdź czy RII i rii nie odwołują się do siebie przez referencje
    {
        if (Generate.rabbitPopSum > 0)
        {
            int idx = 0;
            RabbitsIterationInfo rii = new RabbitsIterationInfo(Generate.rabbitPopSum, Iteration.Global);
            foreach (Movement c in Creatures)
            {
                c.RII.AssignInfo(c.IterationWithoutEat, c.iterationOfObject, c.rabbitLife.Eaten);
                rii.informations[idx] = c.RII.info; // tutaj np. czyli każde rabbit info(informations) jest takie samo.
                idx++;
            }
            InformationBase.AddRabbitsInfo(rii);
        }
    }

    public static void CollectionFamiliesInfo() // można dodać info o rabbitach z rodziny RabbitInfo[]
    {
        if (Generate.rabbitPopSum > 0)
        {
            FamiliesIterationInfo fii = new FamiliesIterationInfo(Generate.rabbitPopSum, Iteration.Global);
            for (int i = 0; i < Generate.sv.RabbitFamilyCount; i++)
            {    
                int memberCount = Creatures.Count(rabb => rabb.rabbitLife.FamilyRabbit.familyID == Generate.sv.familyRabbits[i].familyID);

                if (memberCount > 0)
                    FamilyIterationInfo.Instance.AssignInfo(i, memberCount);
                else if (FamilyIterationInfo.Instance.info[i].isAlive)
                    FamilyIterationInfo.Instance.AssignInfo(i);
            }
            fii.AssignInfo(FamilyIterationInfo.Instance.info);
            InformationBase.AddFamiliesInfo(fii);
        }
    }
}
