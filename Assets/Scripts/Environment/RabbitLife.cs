using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RabbitLife : MonoBehaviour
{
    int eaten = 0;
    public string Name;
    public int getNameNumber;
    public Spawner.FamilyRabbit FamilyRabbit;
    private int reproductionPoint;
    int x;
    int y;
    public int Hunger;


    void Start()
    {
        Name = this.gameObject.name;
        getNameNumber = GetNameNumber;
        this.gameObject.GetComponent<Renderer>().material.color = FamilyRabbit.color;
        reproductionPoint = StartRabbit.Manager.hunger;
    }

    public int GetNameNumber
    {
        get
        {
            int.TryParse(Name.Split('_')[1], out int result);
            return result;
        }
    }

    public void Meal()
    {
        Hunger++;

        if (Hunger >= reproductionPoint)
            Division();
    }

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

    public void Division()
     {
        eaten += Hunger;
        Hunger = 0;
        x = MapHelper.TransormX_ToMapX(this.transform.position.x);
        y = MapHelper.TransormZ_ToMapY(this.transform.position.z);
        TakeFreeWay(x, y, out Way way);
        CoordinateChange(ref x, ref y, way);
        StartCoroutine("WaitForLastPerson");
    }

    IEnumerator WaitForLastPerson()
    {
        yield return new WaitWhile(() => MovementController.IsReady() == true);
        Spawner.Instance.SpawnChildOfRabbit(x, y, FamilyRabbit);
        if (Generate.freeFields == Generate.RabbitPopSum - 1)
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
