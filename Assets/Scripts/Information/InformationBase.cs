using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationBase 
{
    public static List<RabbitsIterationInfo> rabbitsIterationInfos = new List<RabbitsIterationInfo>();
    public static List<FamiliesIterationInfo> familiesIterationInfos = new List<FamiliesIterationInfo>();

    public static void AddRabbitsInfo(RabbitsIterationInfo informations)
    {
        InformationBase.rabbitsIterationInfos.Add(informations);
    }

    public static void AddFamiliesInfo(FamiliesIterationInfo informations)
    {
        InformationBase.familiesIterationInfos.Add(informations);
    }
}
