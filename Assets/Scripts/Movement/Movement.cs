using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour
{
    bool HaveTarget
    {
        get
        {
            if (fow.visibleTargets.Count > 0)
                return true;
            else
                return false;
        }
    }
    int numberOfPerson;
    public bool ready = false;
    private bool populationReady = false;
    private bool waitForRest = false;
    public int iteration = 0;

    public bool waitForCoolDown = true;
    float animationTime = 2f;
    float globalMovementIncrease = 0.5f;
    public readonly float movementSpeed = 1.2f;
    public float movementCooldown = 0;

    public Vector3 actualPosition;

    bool animationWork;

    public int arrayPozX;
    public int arrayPozZ;
    public char[,] accesArea;
    private char[] blockableChar = { 'X', 'T', 'C', 'R', 'O' };

    DirMove dirMove;
    FieldOfView fow;
    Stats stats;
    ArrayToTxt logWritter;

    void Start()
    {
        logWritter = new ArrayToTxt(this.gameObject.name);
        fow = GetComponent<FieldOfView>();
        stats = GetComponent<Stats>();

        numberOfPerson = stats.getNameNumber;
        actualPosition = this.gameObject.transform.position;
        arrayPozX = Convert.ToInt16(actualPosition.x) + Spawner.Instance.HalfHeightMap;
        arrayPozZ = Convert.ToInt16(actualPosition.z) - Spawner.Instance.HalfWidthMap;
        dirMove = new DirMove(Direction.right);

        MovementController.Creatures.Add(this);
    }

    void Update()
    {
        if (!waitForRest)
        {
            if (fow.visibleTargets.Count > 0)
                ToTargetMove();
            else
                RandomMove();
            if (populationReady)
            {
                waitForRest = true;
                StartCoroutine("Move", animationTime);
            }
        }
    }

    public void ToTargetMove()
    {
        iteration++;
        Transform target = fow.visibleTargets.First();
        List<Node> path = GetComponent<Pathfinding>().FindPath(target);
        if (path.Count > 0)
        {
            Node step = path.First();
            dirMove.dir = step.dir;
            dirMove.move = DirToVect3(dirMove.dir);
            DirToArrayPoz();
        }
        PrintTxtLogs();
        ready = true;
        StartCoroutine("CoordinatePopulation", animationTime);
    }
    public void RandomMove()
    {
        accesArea = ArrayModify.CircleOut(Spawner.Instance.GenerateMap, arrayPozZ, arrayPozX, 1);
        ChooseWay(accesArea);
        DirToArrayPoz();
        PrintTxtLogs();
        ready = true;
        StartCoroutine("CoordinatePopulation", animationTime);
    }

    public IEnumerator Move(float time)
    {
        yield return new WaitForSeconds(1);
        MoveObject(dirMove.dir);
        TurnObject(dirMove.dir);
        movementCooldown = 0;
        ready = false;
        waitForRest = false;
        populationReady = false;
        
    }
    public IEnumerator CoordinatePopulation()
    {
        yield return new WaitWhile(() => MovementController.IsReady() == false);
        populationReady = true;
    }


    #region Array/Prepare to Move

    /// <summary>
    /// Change map array
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

    /// <summary>
    /// Choose random way among acces area fileds
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Change MoveDir struct
    /// </summary>
    /// <param name="accesArea"></param>
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

    #endregion

    #region Move
    void MoveObject(Direction dir)
    {
        transform.position = transform.localPosition + DirToVect3(dir);
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
    /// <summary>
    /// Move method.
    /// </summary>
    /// <param name="dir"></param>
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
    #endregion

    struct DirMove
    {
        public Direction dir;
        public bool[] pos;
        public Vector3 move;

        public DirMove(Direction dir)
        {
            this.dir = dir;
            this.pos = new bool[3];
            move = DirToVect3(Direction.none);
        }
    }

    // -------------------- No mather things
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
    public void PrintTxtLogs()
    {
        logWritter.ReadMapArray2D(Spawner.Instance.GenerateMap, iteration.ToString());
        logWritter.ThrowLogToFile(iteration.ToString(), OldLog());
    }
}
