using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private int antiBug;
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
    public static int currentNumberMove = 1;
    public int numberOfPerson 
    { 
        get 
        {
            return rabbitLife.getNameNumber;
        }
    }
    internal bool ready = false;
    private bool populationReady = false;
    private bool canMakeWay;
    public static int globalIteration = 0;
    public int iterationOfObject = 0;

    int dietIteration;
    public int NoEatIterationDie = 0;

    float movementTime = 2f;
    float movementCooldownIncrease = 5f;
    public float currentMovementCooldown = 0;

    public Vector3 actualPosition;

    public int arrayPozX;
    public int arrayPozY;
    public char[,] accesArea;
    public static readonly char[] blockableChar = { 'X', 'T', 'C', 'R', 'O' };

    DirMove dirMove;
    FieldOfView fow;
    public RabbitLife rabbitLife;
    ArrayToTxt logWritter;
    void Start()
    {
        dietIteration = StartRabbit.Manager.iterationToDie;
        logWritter = new ArrayToTxt(this.gameObject.name);
        fow = GetComponent<FieldOfView>();
        rabbitLife = GetComponent<RabbitLife>();
        movementCooldownIncrease = StartRabbit.Manager.movementSpeed;
        actualPosition = this.gameObject.transform.position;
        arrayPozX = MapHelper.TransormX_ToMapX(actualPosition.x);
        arrayPozY = MapHelper.TransormZ_ToMapY(actualPosition.z);
        dirMove = new DirMove(Direction.right);
        accesArea = ArrayModify.CircleOut(Spawner.Instance.GenerateMap, arrayPozX, arrayPozY, 1);
        MovementController.Creatures.Add(this);
        canMakeWay = true;
        PrintTxtLogs();
    }
    void FixedUpdate()
    {
        // Dlaczego obiekty się wieszają, chyba jak umierają

        if (currentNumberMove == this.numberOfPerson && ready == false)
        {
            DecideWhatToDo();
            
        }

        if (populationReady)
        {
            CooldownAndMove();
        }

        if (currentNumberMove > Generate.RabbitPopSum)
        {
            currentNumberMove = 1;
        }
    }

    public void CooldownAndMove()
    {
        currentMovementCooldown += movementCooldownIncrease * Time.deltaTime;
        if (currentMovementCooldown >= movementTime)
            Move();
    }
    public void DecideWhatToDo()
    {
        if (canMakeWay)
        {
            if (HaveTarget)
                ToTargetMove();
            else
                RandomMove();
        }
    }
    public void ToTargetMove() 
    {
        NoEatIterationDie++;
        iterationOfObject++;

        if (NoEatIterationDie >= dietIteration)
        {
            Generate.RabbitPopSum--;
            MovementController.Creatures.Remove(this);
            Destroy(this.gameObject);
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = Spawner.Instance.originalMap[arrayPozX, arrayPozY];
            MovementController.NumeringPopulation();
        }
        else
        {
            Transform target = fow.visibleTargets.First();
            List<Node> path = GetComponent<Pathfinding>().FindPath(target);
            if (path.Count > 0)
            {
                Node step = path.First();
                dirMove.dir = step.dir;
                dirMove.move = DirToVect3(dirMove.dir);
                DirToArrayPoz();
                PrintTxtLogs();
            }
            else
            {
                dirMove.dir = Direction.none;
                dirMove.move = DirToVect3(dirMove.dir);
                DirToArrayPoz();
                PrintTxtLogs();
                fow.visibleTargets.Clear();
                target.gameObject.GetComponentInParent<CarrotSpawn>().iterationOnDead = globalIteration;
                this.gameObject.transform.LookAt(target);
                target.gameObject.SetActive(false);
                rabbitLife.Meal();
                NoEatIterationDie = 0;
            }
            ready = true;
            canMakeWay = false;
            currentNumberMove++;
            StartCoroutine("CoordinatePopulation");
        }
    }
    public void RandomMove()
    {
        NoEatIterationDie++;
        iterationOfObject++;
        if (NoEatIterationDie >= dietIteration)
        {
            Generate.RabbitPopSum--;
            MovementController.Creatures.Remove(this);
            Destroy(this.gameObject);
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = Spawner.Instance.originalMap[arrayPozX, arrayPozY];
            MovementController.NumeringPopulation();
        }
        else
        {
            accesArea = ArrayModify.CircleOut(Spawner.Instance.GenerateMap, arrayPozX, arrayPozY, 1);
            ChooseWay(accesArea);
            DirToArrayPoz();
            PrintTxtLogs();
            ready = true;
            canMakeWay = false;
            currentNumberMove++;
            StartCoroutine("CoordinatePopulation");
        }
    }
    // dir jednek kierunek inny sprawdz przy jednem osobniku
    public void Move()
    {
        MoveObject(dirMove.dir);
        TurnObject(dirMove.dir);
        currentMovementCooldown = 0;
        ready = false;
        canMakeWay = true;
        populationReady = false;
        if (numberOfPerson == Generate.RabbitPopSum)
            globalIteration++;

    }
    public IEnumerator CoordinatePopulation()
    {
        yield return new WaitWhile(() => populationReady == true);
        populationReady = MovementController.IsReady();
    }

    #region Array/Prepare to Move

    /// <summary>
    /// Change map array
    /// </summary>
    public void DirToArrayPoz()
    {
        if (dirMove.dir == Direction.up)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = Spawner.Instance.originalMap[arrayPozX, arrayPozY];
            arrayPozX--;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = 'R';
        }
        else if (dirMove.dir == Direction.down)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = Spawner.Instance.originalMap[arrayPozX, arrayPozY];
            arrayPozX++;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = 'R';
        }
        else if (dirMove.dir == Direction.right)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = Spawner.Instance.originalMap[arrayPozX, arrayPozY];
            arrayPozY++;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = 'R';
        }
        else if (dirMove.dir == Direction.left)
        {
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = Spawner.Instance.originalMap[arrayPozX, arrayPozY];
            arrayPozY--;
            Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = 'R';
        }
        else
        {

        }
    }

    public Direction DirectionDetect(char signToDetect )
    {
        if (signToDetect == accesArea[1, 0])
        {
            return Direction.left;
        }
        else if (signToDetect == accesArea[0, 1])
        {

            return Direction.up;
        }
        else if (signToDetect == accesArea[1, 2])
        {

            return Direction.right;
        }
        else if (signToDetect == accesArea[2, 1])
        {

            return Direction.down;
        }
        else
            return Direction.none;
    }

    /// <summary>
    /// Choose random way among acces area fileds
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    int WayIndex(bool[] pos)
    {
        if (pos.Length != 4)
            throw new Exception("tabela 'pos' musi być [3]");
        else if (Array.TrueForAll(dirMove.pos, x => x == false))
            return 4;
        else if (!dirMove.pos[0] && !dirMove.pos[1] && !dirMove.pos[2] && dirMove.pos[3])
            return 3;
        else
        {
            int rnd;
            int inx = 0;
            while (true)
            {
                rnd = UnityEngine.Random.Range(0, 3);
                if (pos[rnd] == true)
                    return rnd;
                inx++;
            }
        }
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

            if(Array.Exists(blockableChar, x => x == accesArea[1, 0]))
                dirMove.pos[3] = false;
            else
                dirMove.pos[3] = true;

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
            else if (inx == 3)
            {
                dirMove.move = DirToVect3(Direction.left);
                dirMove.dir = Direction.left;
            }
            else
            {
                dirMove.move = DirToVect3(Direction.none);
                dirMove.dir = Direction.none;
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

            if (Array.Exists(blockableChar, x => x == accesArea[0, 1]))
                dirMove.pos[3] = false;
            else
                dirMove.pos[3] = true;

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
            else if (inx == 3)
            {
                dirMove.move = DirToVect3(Direction.up);
                dirMove.dir = Direction.up;
            }
            else
            {
                dirMove.move = DirToVect3(Direction.none);
                dirMove.dir = Direction.none;
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

            if (Array.Exists(blockableChar, x => x == accesArea[2, 1]))
                dirMove.pos[3] = false;
            else
                dirMove.pos[3] = true;

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
            else if(inx == 3)
            {
                dirMove.move = DirToVect3(Direction.down);
                dirMove.dir = Direction.down;
            }
            else
            {
                dirMove.move = DirToVect3(Direction.none);
                dirMove.dir = Direction.none;
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

            if (Array.Exists(blockableChar, x => x == accesArea[1, 2]))
                dirMove.pos[3] = false;
            else
                dirMove.pos[3] = true;

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
            else if(inx == 3)
            {
                dirMove.move = DirToVect3(Direction.right);
                dirMove.dir = Direction.right;
            }
            else
            {
                dirMove.move = DirToVect3(Direction.none);
                dirMove.dir = Direction.none;
            }
        }

        //NONE
        else if (dirMove.dir == Direction.none)
        {
            int rnd = UnityEngine.Random.Range(1, 4);
            dirMove.dir = (Direction)rnd;
            ChooseWay(accesArea); 
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
        else 
            return new Vector3(0f, 0f, 0f);
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
            this.pos = new bool[4];
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
        log += $"X = {arrayPozX} Z = {arrayPozY}\n";
        log += $"object iteration = {iterationOfObject}\n";
        log += $"Real position = {transform.position.x}, {transform.position.z}";
        return log;
    }
    public void PrintTxtLogs()
    {
        logWritter.ReadMapArray2D(Spawner.Instance.GenerateMap, globalIteration.ToString());
        logWritter.ThrowLogToFile(globalIteration.ToString(), OldLog());
    }
}
