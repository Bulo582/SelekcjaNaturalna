using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookCamera : MonoBehaviour
{
    private static bool _isRun;
    public bool isRun
    {
        set
        {
            _isRun = value;
            if (value)
                SetFLCamera(value);
            else
                SetFLCamera(value);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void SetFLCamera(bool value)
    {

    }
}
