using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RabbitLife : MonoBehaviour
{

    public float movementSpeed;
    public float rangeOfView;
    public float hunger;


    private void Start()
    {
        FieldOfView fow = GetComponent<FieldOfView>();
        movementSpeed = StartRabbit.Manager.movementSpeed;
        rangeOfView = StartRabbit.Manager.rangeOfView;
        hunger = StartRabbit.Manager.hunger;
    }

    public RabbitLife(float movementSpeed, float rangeOfView, float hunger)
    {
        this.movementSpeed = movementSpeed;
        this.rangeOfView = rangeOfView;
        this.hunger = hunger;
    }

    public bool TosACoin()
    {
        int result = Random.Range(0, 100);
        if (result > 50)
            return true;
        else
            return false;
    }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float RangeOfView { get => rangeOfView; set => rangeOfView = value; }
    public float Hunger { get => hunger; set => hunger = value; }

}
