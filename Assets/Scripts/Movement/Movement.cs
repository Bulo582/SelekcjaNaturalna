using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    forward,
    right,
    down,
    left
}


public class Movement : MonoBehaviour
{
    Direction direction = Direction.right;
    float animationTime = 2f;
    float globalMovementIncrease = 0.5f;
    public float movementSpeed;
    public float movementCooldown = 0;
    public Vector3 actualPosition;
    bool animationWork;

    public int arrayPozX;
    public int arrayPozZ;
    public char[,] accesArea;

    int halfWidthMap;
    int halfHeightMap;
    void Start()
    {
        actualPosition = this.gameObject.transform.position;
        animationWork = false;
        movementSpeed = StartRabbit.Manager.movementSpeed;

        halfHeightMap = Spawner.Instance.HalfHeightMap;
        halfWidthMap = Spawner.Instance.HalfWidthMap;
         
        arrayPozX = Convert.ToInt16(actualPosition.x ) + halfHeightMap;
        arrayPozZ = Convert.ToInt16(actualPosition.z) - halfWidthMap;

        accesArea = ArrayModify.CircleOut(Spawner.Instance.GenerateMap, arrayPozZ, arrayPozX, 2); // reverse argument x/z

        foreach (var item in accesArea)
        {
           Debug.Log(item.ToString());
        }
        Debug.Log($"X = {arrayPozX} Z = {arrayPozZ}");
        Debug.Log($"What is on postion = {ArrayModify.TypeField(Spawner.Instance.GenerateMap, arrayPozX, arrayPozZ)}");
        StartCoroutine("MoveTime", animationTime);
    }

    void Update()
    {
        if(!animationWork)
        if (movementCooldown <= movementSpeed)
            movementCooldown += globalMovementIncrease * Time.deltaTime;
        else
        {
            StartCoroutine("MoveTime", animationTime);
        }
    }

    public IEnumerator MoveTime()
    {
        movementCooldown = 0;
        animationWork = true;
        //RandomMove();
        yield return new WaitForSeconds(animationTime);
        Debug.Log("Jump");
        animationWork = false;
    }

    public void RandomMove()
    {
        if(direction == Direction.right)
        {
            this.gameObject.transform.Translate(0, 0, 1);
        }
        else if(direction == Direction.down)
        {

        }
        else if(direction == Direction.forward)
        {

        }
        else if(direction == Direction. left)
        {

        }
    }
}
