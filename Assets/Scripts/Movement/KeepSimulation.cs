using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSimulation : MonoBehaviour
{
    internal bool debugMode;
    // Var needed if no one rabbit exist.
    public int globalIteration = Iteration.Global;
    static internal float simulationMovementCooldown = 0;
    static internal float simulationTime = 0;
    float simulationCooldownTime = 2f;
    float simulationCooldownIncrease = 5f;
    private void FixedUpdate()
    {
        if (Generate.rabbitPopSum == 0)
        {
            simulationMovementCooldown += simulationCooldownIncrease * Time.deltaTime;
            if (simulationMovementCooldown >= simulationCooldownTime)
            {
                SmothMove.SimulationMove(ref simulationTime);
                if(debugMode)
                    globalIteration = Iteration.Global;
            }
        }
        else if(debugMode)
            globalIteration = Iteration.Global;
    }

    public void Start()
    {
        simulationCooldownIncrease = StartRabbit.Manager.movementSpeed;
    }
}
