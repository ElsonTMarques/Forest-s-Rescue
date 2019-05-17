using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInputTest : MonoBehaviour
{
    void Update()
    {
        float horizontal = Input.GetAxis("FHorizontal");
        bool attack = Input.GetButton("FAttack");
        bool jump = Input.GetButton("FJump");



        Debug.Log($"Move [{horizontal}] - Attack [{(attack ? 1 : 0)}] - Jump [{(jump ? 1 : 0)}]");
    }
}
