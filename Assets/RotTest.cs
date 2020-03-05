using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        this.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
    }



}
