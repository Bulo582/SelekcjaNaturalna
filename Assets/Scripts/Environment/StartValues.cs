using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartValues : MonoBehaviour
{
    public int RabbitFamilyCount = 0;
    // int foxesStartCount = 0;
    public int carrotSpawnCound = 0;
    public int treeCount = 0;
    public bool testCase = false;
    public Spawner.FamilyRabbit[] familyRabbits;
    [Header("Rabbit")]

    private Color defaultColor;
    public void Awake()
    {
        ColorUtility.TryParseHtmlString("#823E23", out defaultColor);
    }

    public void MyUpdate()
    {
        int familyPopSum = 0;
        if(familyRabbits.Length != RabbitFamilyCount)
            familyRabbits = new Spawner.FamilyRabbit[RabbitFamilyCount];
        for (int i = 0; i < familyRabbits.Length; i++)
        {
            familyPopSum += familyRabbits[i].startPop;
            if (System.String.IsNullOrEmpty(familyRabbits[i].familyName))
            {
                familyRabbits[i].familyName = $"Family_{i+1}";
                familyRabbits[i].color = defaultColor;
                familyRabbits[i].startPop = 1;
            }
        }
        if(familyPopSum < RabbitFamilyCount)
        {
            int familyPopSumCopy = familyPopSum;
            int i = 0;
            while (familyPopSumCopy > 0)
            {
                familyRabbits[i].startPop++;
                familyPopSumCopy--;
                i++;
            }
        }
    }

    public int FamilyPopSum()
    {
        int familyPopSum = 0;
        foreach (var item in familyRabbits)
        {
            familyPopSum += item.startPop;
        }
        return familyPopSum;
    }

}
