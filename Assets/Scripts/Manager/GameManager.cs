using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public static Logger logger = new Logger();
    public static MapToTxt mapToTXTprinter = new MapToTxt("Map");
    public static void StopGame()
    {
        Time.timeScale = 0;
    }
     
    public static void ResumeGame()
    {
        Time.timeScale = GameObject.Find("Manager").GetComponent<GameSettings>().timeSpeed;
    }
}
