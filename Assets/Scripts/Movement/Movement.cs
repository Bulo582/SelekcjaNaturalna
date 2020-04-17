using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour // Iteration Module
{
    // var for know where i am 
    static int currentNumberMove = 1;
    public int iterationOfObject = 0; // sent to rabbitLife
    public int numberOfPerson => rabbitLife.rabbitID;

    // var for know what doing now
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
    internal bool ready = false;
    internal bool populationReady = false;
    internal bool canMakeWay;
    DirMove dirMove;

    // Var needed for die system
    int dietIteration;
    public int IterationWithoutEat = 0;

    // Var needed if rabbits exist
    float movementTime = 2f;
    float movementCooldownIncrease = 5f;
    float currentMovementCooldown = 0;

    // Var needed for operation of (genetated map == array map)
    int arrayPozX;
    int arrayPozY;
    char[,] accesArea;
    internal static readonly char[] blockableChar = { 'X', 'T', 'C', 'R', 'O' };

    //Other modules 
    FieldOfView fow;
    public RabbitIteraionInfo RII;
    internal RabbitLife rabbitLife;
    MapToTxt MapToTXTprinter;
    void Start()
    {
        dietIteration = StartRabbit.Manager.iterationToDie;
        MapToTXTprinter = new MapToTxt(this.gameObject.name);
        fow = GetComponent<FieldOfView>();
        rabbitLife = GetComponent<RabbitLife>();
        RII = new RabbitIteraionInfo(rabbitLife.name, rabbitLife.FamilyRabbit);
        movementCooldownIncrease = StartRabbit.Manager.movementSpeed;
        Vector3 actualPosition = this.gameObject.transform.position;
        arrayPozX = MapHelper.TransormX_ToMapX(actualPosition.x);
        arrayPozY = MapHelper.TransormZ_ToMapY(actualPosition.z);
        dirMove = new DirMove(Direction.none);
        accesArea = ArrayModify.CircleOut(Spawner.Instance.GenerateMap, arrayPozX, arrayPozY, 1);
        PopulationController.Creatures.Add(this);
        canMakeWay = true;
        PrintTxtLogs();
    }
    void FixedUpdate()
    {
        // main of iteration module 

        if (currentNumberMove == this.numberOfPerson && ready == false)
        {
            DecideWhatToDo();
        }

        if (populationReady)
        {
            CooldownAndMove();
        }

        if (currentNumberMove > Generate.rabbitPopSum)
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
        bool ate = false;
        IterationWithoutEat++;
        iterationOfObject++;
        FamilyIterationInfo.Instance.NextIterationOfObject(rabbitLife.FamilyRabbit.familyID);

        if (IterationWithoutEat >= dietIteration)
        {
            Die();
        }
        else
        {
            Transform target = fow.visibleTargets.First();
            List<Node> path = GetComponent<Pathfinding>().FindPath(target);
            if (path.Count > 0)
            {
                Node step = path.First();
                dirMove.dir = step.dir;
                DirToArrayPoz();
                PrintTxtLogs();
            }
            else
            {
                dirMove.dir = Direction.none;
                DirToArrayPoz();
                PrintTxtLogs();
                fow.visibleTargets.Clear();
                target.gameObject.GetComponentInParent<CarrotSpawn>().IterationOnDead = Iteration.Global;
                this.gameObject.transform.LookAt(target);
                target.gameObject.SetActive(false);
                rabbitLife.Meal();
                IterationWithoutEat = 0;
                ate = true;
            }
            ready = true;
            canMakeWay = false;
            currentNumberMove++;
            GameManager.logger.PrintLog($"{this.gameObject.name} - TargetMove meal = {ate}");
            StartCoroutine("CoordinatePopulation");
        }
    }
    public void RandomMove()
    {
        IterationWithoutEat++;
        iterationOfObject++;
        FamilyIterationInfo.Instance.NextIterationOfObject(rabbitLife.FamilyRabbit.familyID);

        if (IterationWithoutEat >= dietIteration)
        {
            Die();
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
            GameManager.logger.PrintLog($"{this.gameObject.name} - RandomMove");
            StartCoroutine("CoordinatePopulation");
        }
    }
    public void Die()
    {
        Generate.rabbitPopSum--;
        
        PopulationController.Creatures.Remove(this);
        GameManager.logger.PrintLog($"{this.gameObject.name} - Die");
        Destroy(this.gameObject);
        Spawner.Instance.GenerateMap[arrayPozX, arrayPozY] = Spawner.Instance.originalMap[arrayPozX, arrayPozY];
        PopulationController.NumeringPopulation();
    }
    private void Move()
    {
        GetComponent<SmothMove>().SwitchOnMove(dirMove.dir);
        TurnObject(dirMove.dir);
        currentMovementCooldown = 0;
        populationReady = false;      
    }
    public IEnumerator CoordinatePopulation()
    {
        yield return new WaitWhile(() => populationReady == true);
        populationReady = PopulationController.IsReady();
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
        { }
    }

    public Direction DirectionDetect(char signToDetect ) // Use to chooseWay
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
    public void ChooseWay(char[,] accesArea) // dirMove można usunąć
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
                dirMove.dir = Direction.down;
           
            else if (inx == 1)
                dirMove.dir = Direction.right;
          
            else if (inx == 2)
                dirMove.dir = Direction.up;
          
            else if (inx == 3)
                dirMove.dir = Direction.left;
         
            else
                dirMove.dir = Direction.none;
           
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
                dirMove.dir = Direction.left;
            
            else if (inx == 1)
                dirMove.dir = Direction.down;
           
            else if (inx == 2)
                dirMove.dir = Direction.right;

            else if (inx == 3)
                dirMove.dir = Direction.up;
          
            else
                dirMove.dir = Direction.none;

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
                dirMove.dir = Direction.left;
           
            else if (inx == 1)
                dirMove.dir = Direction.up;
           
            else if (inx == 2)
                dirMove.dir = Direction.right;
          
            else if(inx == 3)
                dirMove.dir = Direction.down;
            
            else
                dirMove.dir = Direction.none;
            
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
                dirMove.dir = Direction.down;
            
            else if (inx == 1)
                dirMove.dir = Direction.left;
           
            else if (inx == 2)
                dirMove.dir = Direction.up;

            else if(inx == 3)
                dirMove.dir = Direction.right;

            else
                dirMove.dir = Direction.none;
          
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

        public DirMove(Direction dir)
        {
            this.dir = dir;
            this.pos = new bool[4];
        }
    }

    // -------------------- No mather things
    public string Log()
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
        log += $"ArrayX = {arrayPozX}, ArrayY = {arrayPozY}\n";
        log += $"object iteration = {iterationOfObject}\n";
        log += $"RealX {transform.position.x}, RealZ = {transform.position.z}";
        return log;
    }
    public void PrintTxtLogs()
    {
        MapToTXTprinter.ReadMapArray2D(Spawner.Instance.GenerateMap, Iteration.Global.ToString());
        MapToTXTprinter.ThrowLogToFile(Iteration.Global.ToString(), Log());
    }
}
