using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testpath : MonoBehaviour
{


    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("游戏暂停!");
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("游戏开始!");
            Time.timeScale = 1;
        }
    }

 

}
