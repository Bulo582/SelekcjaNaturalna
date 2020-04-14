using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmothMove : MonoBehaviour
{
    Movement movement;
    Vector3 target;
    Direction dir = Direction.none;

    static readonly float timeToReachTarget = 1f;
    float time;
    bool move = false;
    int numebrOfPerson => movement.numberOfPerson;

    Vector2 simulatedTarget = new Vector2(0, 1);
    Vector2 simulatedSeeker = new Vector2(0, 0);

    static Vector2 globalSimulatedTarget = new Vector2(0, 1);
    static Vector2 globalSimulatedSeeker = new Vector2(0, 0);
    private void Start()
    {
        movement = GetComponent<Movement>();
    }
    void FixedUpdate()
    {
        if(move)
        {
            SmoothDirectionMove();
        }
    }

    void SmoothDirectionMove()
    {
        
        if (dir != Direction.none)
        {
            if (Vector3.Distance(transform.position, target) > 0.1f)
            {
                time += Time.deltaTime / timeToReachTarget;
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, Spawner.rabbitY, target.z), time);
            }
            else
            {
                time = 0;
                this.transform.position = target;
                movement.ready = false;
                movement.canMakeWay = true;
                move = false;
                GameManager.logger.PrintLog($"{this.gameObject.name} - Smooth Dir.{dir.ToString()}");
                if (numebrOfPerson == Generate.rabbitPopSum)
                {
                    Iteration.NextIteration();
                }
            }
        }
        else
        {
            // simulation move for get the same time like on move 
            if (Vector2.Distance(simulatedSeeker,simulatedTarget) > 0.1f)
            {
                time += (Time.deltaTime / timeToReachTarget);
                simulatedSeeker = Vector2.Lerp(simulatedSeeker, simulatedTarget, time);
            }
            else
            {
                time = 0;
                simulatedSeeker.Set(0, 0);
                movement.ready = false;
                movement.canMakeWay = true;
                move = false;
                GameManager.logger.PrintLog($"{this.gameObject.name} - Smooth Dir.none");
                if (numebrOfPerson == Generate.rabbitPopSum)
                {
                    Iteration.NextIteration();
                }
            }
        }
    }
    public void SwitchOnMove(Direction dir)
    {
        target = transform.localPosition + DirToVect3(dir);
        move = true;
        this.dir = dir;
        
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

    public static void SimulationMove(ref float time) // for FixedUpadate
    {
        // simulation move for get the same time like on move 
        if (Vector2.Distance(globalSimulatedSeeker, globalSimulatedTarget) > 0.1f)
        {
            time += (Time.deltaTime / timeToReachTarget);
            globalSimulatedSeeker = Vector2.Lerp(globalSimulatedSeeker, globalSimulatedTarget, time);
        }
        else
        {
            time = 0;
            KeepSimulation.simulationMovementCooldown = 0;
            globalSimulatedSeeker.Set(0, 0);
            Iteration.NextIteration();
        }
    }
}
