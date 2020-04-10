using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmothMove : MonoBehaviour
{
    Movement movement;
    Vector3 target;
    Direction dir = Direction.none;

    float timeToReachTarget = 1f;
    float time;
    bool move = false;
    int numebrOfPerson;

    Vector2 simulationTarget = new Vector2(0,1);
    Vector2 simulationSeeker = new Vector2(0, 0);
    private void Start()
    {
        numebrOfPerson = GetComponent<RabbitLife>().getNameNumber;
        movement = GetComponent<Movement>();
    }
    void FixedUpdate()
    {
        if(move)
        {
            SmothDirectionShift();
        }
    }

    public void SmothDirectionShift()
    {
        
        if (dir != Direction.none)
        {
            float test = Vector3.Distance(transform.position, target); // delete
            if (Vector3.Distance(transform.position, target) > 0.1f)
            {
                time += Time.deltaTime / timeToReachTarget;
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.x, Spawner.rabbitY, target.z), time);
            }
            else
            {
                time = 0;
                transform.position = target;
                movement.ready = false;
                movement.canMakeWay = true;
                move = false;
                Logger.PrintLog($"{this.gameObject.name} - Smooth Dir.{dir.ToString()}");
                if (numebrOfPerson== Generate.RabbitPopSum)
                    Movement.globalIteration++;
            }
        }
        else
        {
            // simulation move for get the same time like on move 
            if (Vector2.Distance(simulationSeeker,simulationTarget) > 0.1f)
            {
                time += (Time.deltaTime / timeToReachTarget);
                simulationSeeker = Vector2.Lerp(simulationSeeker, simulationTarget, time);
                float check = Vector2.Distance(simulationSeeker, simulationTarget);
            }
            else
            {
                time = 0;
                simulationSeeker.Set(0, 0);
                transform.position = target;
                movement.ready = false;
                movement.canMakeWay = true;
                move = false;
                Logger.PrintLog($"{this.gameObject.name} - Smooth Dir.none");
                if (numebrOfPerson == Generate.RabbitPopSum)
                    Movement.globalIteration++;
            }
        }
    }

    public void SwitchOnMove(Direction dir)
    {
        target = transform.localPosition + DirToVect3(dir);
        move = true;
        this.dir = dir;
        
    }

    public  Vector3 DirToVect3(Direction direction)
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
}
