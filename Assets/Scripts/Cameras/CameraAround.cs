using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAround : MonoBehaviour
{
    private float speed = 3;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
