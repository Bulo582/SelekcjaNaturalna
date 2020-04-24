using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AlgoritmTime
{
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
        if (!running)
        {
            count++;
            running = true;
            sw.Start();
        }
        else
            UnityEngine.Debug.LogError("First stop watcher");
    }

    public void Stop(bool reset = true)
    {
        if (running)
        {
            running = false;
            sw.Stop();
            sum =+ sw.ElapsedMilliseconds;
            if(reset)
                sw.Reset();
        }
        else
            UnityEngine.Debug.LogError("First start watcher");
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
