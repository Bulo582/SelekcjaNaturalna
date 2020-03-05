using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up,
    right,
    down,
    left,
    none
}

public class Movement : MonoBehaviour
{
    float animationTime = 2f;
    float globalMovementIncrease = 0.5f;
    public float movementSpeed;
    public float movementCooldown = 0;
    public Vector3 actualPosition;
    bool animationWork;

    public int arrayPozX;
    public int arrayPozZ;
    public char[,] accesArea;

    int halfWidthMap;
    int halfHeightMap;

    DirMove dirMove;

    private char[] blockableChar = { 'X', 'T', 'C', 'R'};
    void Start()
    {
        actualPosition = this.gameObject.transform.position;
        animationWork = false;
        movementSpeed = StartRabbit.Manager.movementSpeed;

        halfHeightMap = Spawner.Instance.HalfHeightMap;
        halfWidthMap = Spawner.Instance.HalfWidthMap;
         
        arrayPozX = Convert.ToInt16(actualPosition.x ) + halfHeightMap;
        arrayPozZ = Convert.ToInt16(actualPosition.z) - halfWidthMap;

        accesArea = ArrayModify.CircleOut(Spawner.Instance.GenerateMap, arrayPozZ, arrayPozX, 2); //tip! reverse argument x/z

        dirMove = new DirMove(Direction.right);
    }

    public void OldLog()
    {
        foreach (var item in accesArea)
        {
            Debug.Log(item.ToString());
        }
        Debug.Log($"X = {arrayPozX} Z = {arrayPozZ}");
        Debug.Log($"What is on postion = {ArrayModify.TypeField(Spawner.Instance.GenerateMap, arrayPozX, arrayPozZ)}");
        StartCoroutine("MoveTime", animationTime);
    }

    void Update()
    {
        if(!animationWork)
        if (movementCooldown <= movementSpeed)
            movementCooldown += globalMovementIncrease * Time.deltaTime;
        else
        {
            StartCoroutine("MoveTime", animationTime);
        }
    }
    public IEnumerator MoveTime()
    {
        movementCooldown = 0;
        animationWork = true;
        RandomMove();
        yield return new WaitForSeconds(animationTime);
        Debug.Log("Jump");
        animationWork = false;
    }
    public void RandomMove()
    {
        ChooseWay(accesArea);
        DirToArrayPoz();
    }
    /// <summary>
    /// Atrer take way 
    /// Change: arrayPozX, arrayPozZ, Spawner.Instance.GenerateMap[x,y]
    /// </summary>
    public void DirToArrayPoz()
    {
        if (dirMove.dir == Direction.up)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = '4';
            arrayPozX++;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = 'R';
        }
        else if (dirMove.dir == Direction.down)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = '4';
            arrayPozX--;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = 'R';
        }
        else if (dirMove.dir == Direction.right)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = '4';
            arrayPozZ++;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = 'R';
        }
        else if (dirMove.dir == Direction.left)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = '4';
            arrayPozZ--;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = 'R';
        }
    }
    public static Vector3 DirToVect3(Direction direction)
    {
        if (direction == Direction.up)
            return new Vector3(-1, 0, 0);
        else if (direction == Direction.left)
            return new Vector3(0, 0, -1);
        else if (direction == Direction.down)
            return new Vector3(1, 0, 0);
        else if (direction == Direction.right)
            return new Vector3(0, 0, 1);
        else return new Vector3(0, 0, 0);
    }
    int WayIndex(bool[] pos)
    {
        if (pos.Length != 3)
            throw new Exception("tabela 'pos' musi być [3]");
        int rnd;
        int inx = 0;
        while (inx < 10)
        {
            rnd = UnityEngine.Random.Range(0, 2);
            if (pos[rnd] == true)
                return rnd;
            inx++;
        }
        return 3;
    }
    public void ChooseWay(char[,] accesArea)
    {
        if (dirMove.dir == Direction.right)
        {
            if (Array.Exists(blockableChar, x => x == accesArea[0, 1]))
                dirMove.pos[0] = false;
            else
                dirMove.pos[0] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[1, 2]))
                dirMove.pos[1] = false;
            else
                dirMove.pos[1] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[2, 1]))
                dirMove.pos[2] = false;
            else
                dirMove.pos[2] = true;

            int inx = WayIndex(dirMove.pos);

            if (inx == 0)
            {
                dirMove.move = DirToVect3(Direction.down);
                dirMove.dir = Direction.down;
                TurnObject(dirMove.dir);
            }
            else if (inx == 1)
            {
                dirMove.move = DirToVect3(Direction.right);
                dirMove.dir = Direction.right;
            }
            else if (inx == 2)
            {
                dirMove.move = DirToVect3(Direction.up);
                dirMove.dir = Direction.up;
                TurnObject(dirMove.dir);
            }
            else
            {
                dirMove.move = DirToVect3(Direction.left);
                dirMove.dir = Direction.left;
                TurnObject(dirMove.dir);
            }

        }
        else if (dirMove.dir == Direction.down)
        {
            if (Array.Exists(blockableChar, x => x == accesArea[1, 0]))
                dirMove.pos[0] = false;
            else
                dirMove.pos[0] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[2, 1]))
                dirMove.pos[1] = false;
            else
                dirMove.pos[1] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[1, 2]))
                dirMove.pos[2] = false;
            else
                dirMove.pos[2] = true;

            int inx = WayIndex(dirMove.pos);

            if (inx == 0)
            {
                dirMove.move = DirToVect3(Direction.left);
                dirMove.dir = Direction.left;
                TurnObject(dirMove.dir);
            }
            else if (inx == 1)
            {
                dirMove.move = DirToVect3(Direction.down);
                dirMove.dir = Direction.down;
            }
            else if (inx == 2)
            {
                dirMove.move = DirToVect3(Direction.right);
                dirMove.dir = Direction.right;
                TurnObject(dirMove.dir);
            }
            else
            {
                dirMove.move = DirToVect3(Direction.up);
                dirMove.dir = Direction.up;
                TurnObject(dirMove.dir);
            }


        }
        else if (dirMove.dir == Direction.up)
        {
            if (Array.Exists(blockableChar, x => x == accesArea[1, 0]))
                dirMove.pos[0] = false;
            else
                dirMove.pos[0] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[1, 1]))
                dirMove.pos[1] = false;
            else
                dirMove.pos[1] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[1, 2]))
                dirMove.pos[2] = false;
            else
                dirMove.pos[2] = true;

            int inx = WayIndex(dirMove.pos);

            if (inx == 0)
            {
                dirMove.move = DirToVect3(Direction.left);
                dirMove.dir = Direction.left;
                TurnObject(dirMove.dir);
            }
            else if (inx == 1)
            {
                dirMove.move = DirToVect3(Direction.up);
                dirMove.dir = Direction.up;
            }
            else if (inx == 2)
            {
                dirMove.move = DirToVect3(Direction.right);
                dirMove.dir = Direction.right;
                TurnObject(dirMove.dir);
            }
            else
            {
                dirMove.move = DirToVect3(Direction.down);
                dirMove.dir = Direction.down;
                TurnObject(dirMove.dir);
            }

        }
        else if (dirMove.dir == Direction.left)
        {
            if (Array.Exists(blockableChar, x => x == accesArea[2, 1]))
                dirMove.pos[0] = false;
            else
                dirMove.pos[0] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[1, 0]))
                dirMove.pos[1] = false;
            else
                dirMove.pos[1] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[1, 1]))
                dirMove.pos[2] = false;
            else
                dirMove.pos[2] = true;

            int inx = WayIndex(dirMove.pos);

            if (inx == 0)
            {
                dirMove.move = DirToVect3(Direction.down);
                dirMove.dir = Direction.down;
                TurnObject(dirMove.dir);
            }
            else if (inx == 1)
            {
                dirMove.move = DirToVect3(Direction.left);
                dirMove.dir = Direction.left;
            }
            else if (inx == 2)
            {
                dirMove.move = DirToVect3(Direction.up);
                dirMove.dir = Direction.up;
                TurnObject(dirMove.dir);
            }
            else
            {
                dirMove.move = DirToVect3(Direction.right);
                dirMove.dir = Direction.right;
                TurnObject(dirMove.dir);
            }

        }
    }
    public void TurnObject(Direction dir)
    {
        if (dir == Direction.right)
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir == Direction.up)
            this.gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        else if(dir == Direction.down)
            this.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (dir == Direction.left)
            this.gameObject.transform.rotation = Quaternion.Euler(0, -180, 0);
    }
    public struct DirMove
    {
        public Direction dir;
        public bool[] pos;
        public Vector3 move;

        public DirMove(Direction dir)
        {
            this.dir = dir;
            this.pos = new bool[3];
            move = Movement.DirToVect3(Direction.none);
        }
    }
}


