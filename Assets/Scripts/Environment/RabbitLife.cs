using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RabbitLife : MonoBehaviour, IAnimal
{

    [Range(0, 10)]
    public short reproductionRange;
    public short reproductionAge;
    [Range(0, 10)]
    public short age;
    public short limitAge;
    public float movementSpeed;
    public float rangeOfView;
    public float hunger;
    public float thrist;
    public float strength;
    public Sex setSex;

    private void Start()
    {

       
        FieldOfView fow = GetComponent<FieldOfView>();
        reproductionRange = StartRabbit.Manager.reproductionCapacity;
        reproductionAge = StartRabbit.Manager.reproductionAge;
        age = StartRabbit.Manager.age;
        limitAge = StartRabbit.Manager.limitAge;
        movementSpeed = StartRabbit.Manager.movementSpeed;
        rangeOfView = StartRabbit.Manager.rangeOfView;
        thrist = StartRabbit.Manager.thrist;
        hunger = StartRabbit.Manager.hunger;
        strength = StartRabbit.Manager.strength;
        setSex = (TosACoin()) ? Sex.male : Sex.female;
        if (setSex == Sex.female)
            this.gameObject.GetComponent<Renderer>().material.color = StartRabbit.Manager.femaleColor;
        fow.viewRadious = rangeOfView;
    }

    public RabbitLife(short reproductionRange, short reproductionAge, short age, short limitAge, float movementSpeed, float rangeOfView, float hunger, float desire, float strength)
    {
        this.reproductionRange = reproductionRange;
        this.reproductionAge = reproductionAge;
        this.age = age;
        this.limitAge = limitAge;
        this.movementSpeed = movementSpeed;
        this.rangeOfView = rangeOfView;
        this.hunger = hunger;
        this.strength = strength;
        setSex = (TosACoin()) ? Sex.male : Sex.female;
    }

    public bool TosACoin()
    {
        int result = Random.Range(0, 100);
        if (result > 50)
            return true;
        else
            return false;
    }

    public short ReproductionRange { get => reproductionRange; set => reproductionRange = value; }
    public short Age { get => age; set => age = value; }
    public short LimitAge { get => limitAge; set => limitAge = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public float RangeOfView { get => rangeOfView; set => rangeOfView = value; }
    public float Hunger { get => hunger; set => hunger = value; }
    public float Thrist { get => thrist; set => thrist = value; }
    public float Strength { get => strength; set => strength = value; }
    public Sex SetSex { get => setSex; set => setSex = value; }
    public short ReproductionAge { get => reproductionAge; set => reproductionAge = value; }
}
