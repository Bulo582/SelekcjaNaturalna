using System;
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

public class Movement : MonoBehaviour
{
    static int numberOfPerson = 1;
    
    public bool agro = false;
    public int iteration = 0;

    float animationTime = 2f;
    float globalMovementIncrease = 0.5f;
    public readonly float movementSpeed = 1.2f;
    public float movementCooldown = 0;

    public Vector3 actualPosition;

    bool animationWork;

    public int arrayPozX;
    public int arrayPozZ;
    public char[,] accesArea;

    int halfWidthMap;
    int halfHeightMap;

    DirMove dirMove;
    FieldOfView fow;
    Stats thisStats;
    ArrayToTxt logWritter;
    private char[] blockableChar = { 'X', 'T', 'C', 'R', 'O' };
    void Start()
    {
        logWritter = new ArrayToTxt(this.gameObject.name);
        thisStats = GetComponent<Stats>();
        actualPosition = this.gameObject.transform.position;
        animationWork = false;

        halfHeightMap = Spawner.Instance.HalfHeightMap;
        halfWidthMap = Spawner.Instance.HalfWidthMap;

        arrayPozX = Convert.ToInt16(actualPosition.x) + halfHeightMap;
        arrayPozZ = Convert.ToInt16(actualPosition.z) - halfWidthMap;


        accesArea = ArrayModify.CircleOut(Spawner.Instance.GenerateMap, arrayPozZ, arrayPozX, 1); //tip! reverse argument x/z

        dirMove = new DirMove(Direction.right);

        logWritter.ReadMapArray2D(Spawner.Instance.GenerateMap);
        logWritter.ThrowLogToFile(iteration.ToString(), OldLog());
    }

    public string OldLog()
    {
        string log = $"{transform.name}\n";
        for (int i = 0; i < accesArea.GetLength(0); i++)
        {
            for (int j = 0; j < accesArea.GetLength(0); j++)
            {
                log += accesArea[i, j].ToString();
            }
            log += "\n";
        }

        log += $"Dir = {dirMove.dir.ToString()}\n";
        log += $"X = {arrayPozX} Z = {arrayPozZ}\n";
        log += $"What is on postion = {ArrayModify.TypeField(Spawner.Instance.GenerateMap, arrayPozX, arrayPozZ)}\n";
        log += $"Real position = {transform.position.x}, {transform.position.z}";
        return log;
    }

    void Update()
    {

        if (!animationWork)
            if (movementCooldown <= movementSpeed)
                movementCooldown += globalMovementIncrease * Time.deltaTime;
            else
            {
                StartCoroutine("MoveTime", animationTime);
                numberOfPerson++;
            }
       
    }
    public IEnumerator MoveTime()
    {
        movementCooldown = 0;
        animationWork = true;
        iteration++;
        fow = GetComponent<FieldOfView>();
        if (fow.visibleTargets.Count == 0)
        {
            RandomMove();
            logWritter.ReadMapArray2D(Spawner.Instance.GenerateMap, iteration.ToString());
            logWritter.ThrowLogToFile(iteration.ToString(), OldLog());
            yield return new WaitForSeconds(animationTime);
            animationWork = false;
        }
        else if (fow.visibleTargets.Count > 0)
        {
            Transform target = fow.visibleTargets.First();
            MoveToTartget(target);
            logWritter.ReadMapArray2D(Spawner.Instance.GenerateMap);
            logWritter.ThrowLogToFile(iteration.ToString(), OldLog());
            yield return new WaitForSeconds(animationTime);
            animationWork = false;
        }
    }
    public void MoveToTartget(Transform target)
    {
        List<Node> path = GetComponent<Pathfinding>().FindPath(target);
        if (path.Count > 0)
        {
            Node step = path.First();
            dirMove.dir = step.dir;
            dirMove.move = DirToVect3(dirMove.dir);
            MoveObject(dirMove.dir);
            TurnObject(dirMove.dir);
            DirToArrayPoz();
        }
        else
        {
            fow.visibleTargets.Clear();
            target.gameObject.SetActive(false);
        }
    }
    public void RandomMove()
    {
        ChooseWay(accesArea);
        MoveObject(dirMove.dir);
        TurnObject(dirMove.dir);
        DirToArrayPoz();
        accesArea = ArrayModify.CircleOut(Spawner.Instance.GenerateMap, arrayPozZ, arrayPozX, 1);
    }
    /// <summary>
    /// Atrer take way 
    /// Change: arrayPozX, arrayPozZ, Spawner.Instance.GenerateMap[x,y]
    /// </summary>
    public void DirToArrayPoz()
    {
        if (dirMove.dir == Direction.up)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = Spawner.Instance.originalMap[arrayPozX, arrayPozZ];
            arrayPozX--;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = 'R';
        }
        else if (dirMove.dir == Direction.down)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = Spawner.Instance.originalMap[arrayPozX, arrayPozZ];
            arrayPozX++;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = 'R';
        }
        else if (dirMove.dir == Direction.right)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = Spawner.Instance.originalMap[arrayPozX, arrayPozZ];
            arrayPozZ++;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = 'R';
        }
        else if (dirMove.dir == Direction.left)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = Spawner.Instance.originalMap[arrayPozX, arrayPozZ];
            arrayPozZ--;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozZ] = 'R';
        }
    }
    public static Vector3 DirToVect3(Direction direction)
    {
        if (direction == Direction.up)
            return new Vector3(-1f, 0f, 0f);
        else if (direction == Direction.left)
            return new Vector3(0f, 0f, -1f);
        else if (direction == Direction.down)
            return new Vector3(1f, 0f, 0f);
        else if (direction == Direction.right)
            return new Vector3(0f, 0f, 1f);
        else return new Vector3(0f, 0f, 0f);
    }
    int WayIndex(bool[] pos)
    {
        if (pos.Length != 3)
            throw new Exception("tabela 'pos' musi być [3]");
        int rnd;
        int inx = 0;
        while (inx < 10)
        {
            rnd = UnityEngine.Random.Range(0, 3);
            if (pos[rnd] == true)
                return rnd;
            inx++;
        }
        return 3;
    }
    public void ChooseWay(char[,] accesArea)
    {
        // RIGHT 
        if (dirMove.dir == Direction.right)
        {
            if (Array.Exists(blockableChar, element => element == accesArea[2, 1]))
                dirMove.pos[0] = false;
            else
                dirMove.pos[0] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[1, 2]))
                dirMove.pos[1] = false;
            else
                dirMove.pos[1] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[0, 1]))
                dirMove.pos[2] = false;
            else
                dirMove.pos[2] = true;

            int inx = WayIndex(dirMove.pos);

            if (inx == 0)
            {
                dirMove.move = DirToVect3(Direction.down);
                dirMove.dir = Direction.down;
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
            }
            else
            {
                dirMove.move = DirToVect3(Direction.left);
                dirMove.dir = Direction.left;
            }

        }
        // DOWN 
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
            }
            else
            {
                dirMove.move = DirToVect3(Direction.up);
                dirMove.dir = Direction.up;
            }


        }
        //UP
        else if (dirMove.dir == Direction.up)
        {
            if (Array.Exists(blockableChar, x => x == accesArea[1, 0]))
                dirMove.pos[0] = false;
            else
                dirMove.pos[0] = true;

            if (Array.Exists(blockableChar, x => x == accesArea[0, 1]))
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
            }
            else
            {
                dirMove.move = DirToVect3(Direction.down);
                dirMove.dir = Direction.down;
            }

        }
        //LEFT
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

            if (Array.Exists(blockableChar, x => x == accesArea[0, 1]))
                dirMove.pos[2] = false;
            else
                dirMove.pos[2] = true;

            int inx = WayIndex(dirMove.pos);

            if (inx == 0)
            {
                dirMove.move = DirToVect3(Direction.down);
                dirMove.dir = Direction.down;
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
            }
            else
            {
                dirMove.move = DirToVect3(Direction.right);
                dirMove.dir = Direction.right;
            }
        }
    }
    public void TurnObject(Direction dir)
    {
        if (dir == Direction.right)
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir == Direction.up)
            this.gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        else if (dir == Direction.down)
            this.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (dir == Direction.left)
            this.gameObject.transform.rotation = Quaternion.Euler(0, -180, 0);
    }
    void MoveObject(Direction dir)
    {
        transform.position = transform.localPosition + DirToVect3(dir);
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


