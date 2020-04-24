using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AlgoritmTime
{
    public bool canDo = true;
    private static AlgoritmTime _instance;
    public static AlgoritmTime Instance
    {
        get 
        {
            if(_instance != null)
                return _instance;
            else
            {
                _instance = new AlgoritmTime();
                return _instance;
            }
        }
        private set
        {
            _instance = value;
        }
    }

    bool running = false;
    private Stopwatch sw;
    float sum = 0;
    int count = 0;
    public float ave { get { return sum / count; } }
    public AlgoritmTime ()
    {
        sw = new Stopwatch();
    }

    public void Start()
    {
        if (!running && canDo)
        {
            count++;
            running = true;
            sw.Start();
        }
    }

    public void Stop(bool reset = true)
    {
        if (running || canDo)
        {
            running = false;
            sw.Stop();
            sum =+ sw.ElapsedMilliseconds;
            if(reset)
                sw.Reset();
        }
    }

    public void AveLog()
    {
        if (canDo)
        {
            UnityEngine.Debug.Log("Path found: " + AlgoritmTime.Instance.ave + " ms./interation");
            sum = 0;
            count = 0;
        }
    }

    public void NewTest()
    {
        if (!running)
        {
            sum = 0;
            count = 0;
            sw = new Stopwatch();
        }
        else
            UnityEngine.Debug.LogError("First finish active test");
    }
}
