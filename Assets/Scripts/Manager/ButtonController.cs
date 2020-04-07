using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public bool pauseMode = false;
    void Update()
    {
        // -Space
          if(Input.GetKeyDown(KeyCode.Space) && !pauseMode)
        {
            pauseMode = true;
            GameManager.StopGame();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && pauseMode)
        {
            pauseMode = false;
            GameManager.ResumeGame();
        }
        //
    }
}
