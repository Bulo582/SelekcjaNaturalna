using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimal 
{
    short ReproductionRange { get; set; }

    short Age { get; set; }

    short LimitAge { get; set; }

    short ReproductionAge { get; set; }

    float MovementSpeed { get; set; }

    float RangeOfView { get; set; }

    float Hunger { get; set; }

    float Thrist { get; set; }

    float Strength { get; set; }

    Sex SetSex { get; set; }
}

public enum Sex
{
    male,
    female
}
