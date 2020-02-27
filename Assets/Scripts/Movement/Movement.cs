﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float animationTime = 2f;
    float globalMovementIncrease = 0.5f;
    public float movementSpeed;
    public float movementCooldown = 0;
    public Vector3 actualPosition;
    bool animationWork;

    void Start()
    {
        actualPosition = this.gameObject.transform.position;
        animationWork = false;
        movementSpeed = StartRabbit.Manager.movementSpeed;
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
        yield return new WaitForSeconds(animationTime);
        Debug.Log("Jump");
        animationWork = false;
    }

    public void RandomMove()
    {

    }
}
