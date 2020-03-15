using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    int iteration = 0;
    bool animationWork = true;
    void Update()
    {
        if(animationWork)
            StartCoroutine("MoveTime", 2f); ;
    }

    public IEnumerator MoveTime()
    {
        animationWork = false;
        iteration++;
        Direction dir = Direction.none;
        int id = Random.Range(1, 5);
        if (id == (int)Direction.down)
            dir = Direction.down;
        else if (id == (int)Direction.up)
            dir = Direction.up;
        if (id == (int)Direction.left)
            dir = Direction.left;
        if (id == (int)Direction.right)
            dir = Direction.right;
        Debug.Log($"{iteration} - id{id} = {dir.ToString()}");
        this.gameObject.transform.Translate(Movement.DirToVect3(dir));
        yield return new WaitForSeconds(1.0f);
        animationWork = true;
    }
}
