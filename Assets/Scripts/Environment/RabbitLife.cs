using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RabbitLife : MonoBehaviour
{
    public RabbitLife Parent;
    public int rabbitID; // identify rabbit in Iteration Module ..// this variable getting value from method: NumeringPopulation()
    internal Spawner.FamilyRabbit FamilyRabbit; // membership of group...
    string Name; // name of current rabbit
    int reproductionPoint; // count carrot to division
    int Hunger; // counter that how eaten carrot before divison
    public int Eaten = 0; // sum carrots which the rabbit eaten in whole own life 
    // prefer add here IterationOfObject
    public int IterationToDie;

    int arrayPosX;
    int arrayPosY;
    public int GetNameNumber
    {
        get
        {
            int.TryParse(Name.Split('_')[1], out int result);
            return result;
        }
    }
    void Start()
    {
        Name = this.gameObject.name;
        this.gameObject.GetComponent<Renderer>().material.color = FamilyRabbit.color;
        reproductionPoint = StartRabbit.Manager.hunger;
        IterationToDie = StartRabbit.Manager.iterationToDie;
        if (Parent != null)
            IterationToDie = Parent.IterationToDie + 1;
    }
    public void Meal()
    {
        Eaten++;
        Hunger++;
        FamilyIterationInfo.Instance.NextMealOfObject(FamilyRabbit.familyID);

        if (Hunger >= reproductionPoint)
            Division(IterationToDie);
    }
    /// <summary>
    /// returns random, possible to acces way
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="way"></param>
    public void TakeFreeWay(int x, int y, out Way way)
    {
        way = new Way();
        char[,] accesArea = ArrayModify.CircleOut(Spawner.Instance.GenerateMap, x, y, 1);
        Way[] aceptedPos = new Way[4];

        if (Array.Exists(Movement.blockableChar, p => p == accesArea[1, 0]))
        {
            aceptedPos[0].isOpen = false;
            aceptedPos[0].idx = 0;
            aceptedPos[0].dir = Direction.left;
        }
        else
        {
            aceptedPos[0].isOpen = true;
            aceptedPos[0].idx = 0;
            aceptedPos[0].dir = Direction.left;
        }

        if (Array.Exists(Movement.blockableChar, p => p == accesArea[0, 1]))
        {
            aceptedPos[1].isOpen = false;
            aceptedPos[1].idx = 1;
            aceptedPos[1].dir = Direction.up;
        }
        else
        {
            aceptedPos[1].isOpen = true;
            aceptedPos[1].idx = 1;
            aceptedPos[1].dir = Direction.up;
        }

        if (Array.Exists(Movement.blockableChar, p => p == accesArea[1, 2]))
        {
            aceptedPos[2].isOpen = false;
            aceptedPos[2].idx = 2;
            aceptedPos[2].dir = Direction.right;
        }
        else
        {
            aceptedPos[2].isOpen = true;
            aceptedPos[2].idx = 2;
            aceptedPos[2].dir = Direction.right;
        }

        if (Array.Exists(Movement.blockableChar, p => p == accesArea[2, 1]))
        {
            aceptedPos[3].isOpen = false;
            aceptedPos[3].idx = 3;
            aceptedPos[3].dir = Direction.down;
        }
        else
        {
            aceptedPos[3].isOpen = true;
            aceptedPos[3].idx = 3;
            aceptedPos[3].dir = Direction.down;
        }

        if (Array.Exists(aceptedPos, p => p.isOpen == true))
        {
            int rnd;
            int idx = 0;
            while (idx < 20)
            {
                idx++;
                rnd = UnityEngine.Random.Range(0, 4);
                if (aceptedPos[rnd].isOpen == true)
                {
                     way = aceptedPos[rnd];
                }
            }
        }
    }
    /// <summary>
    /// change the main position parameter through seleced way
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="way"></param>
    public void CoordinateChange(ref int x, ref int y, Way way)
    {
        if(way.dir == Direction.up)
        {
            x--;
        }
        else if(way.dir == Direction.down)
        {
            x++;
        }
        else if (way.dir == Direction.right)
        {
            y++;
        }
        else if (way.dir == Direction.left)
        {
            y--;
        }
    }
    /// <summary>
    /// Born new little sweet rabbit. TakeFreeWay and CoordinateChange must be call before. And increment iteration to die on the new generation.
    /// </summary>
    public void Division(int iterationToDie)
     {
        Hunger = 0;
        arrayPosX = MapHelper.TransormX_ToMapX(this.transform.position.x);
        arrayPosY = MapHelper.TransormZ_ToMapY(this.transform.position.z);
        TakeFreeWay(arrayPosX, arrayPosY, out Way way);
        CoordinateChange(ref arrayPosX, ref arrayPosY, way);
        StartCoroutine("WaitForLastPerson");
        
    }
    IEnumerator WaitForLastPerson()
    {
        yield return new WaitWhile(() => PopulationController.IsReady() == true);
        Spawner.Instance.SpawnChildOfRabbit(arrayPosX, arrayPosY, FamilyRabbit, this);
        FamilyIterationInfo.Instance.NewRabbit(FamilyRabbit.familyID);
        if (Generate.freeFields == Generate.rabbitPopSum - 1)
        {
            Time.timeScale = 0;
            Debug.Log("Finish - Map is full");
        }
    }
    public struct Way
    {
        public Direction dir;
        public int idx;
        public bool isOpen;
    }
}
