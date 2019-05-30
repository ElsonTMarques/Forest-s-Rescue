using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Congratulations : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Invoke("GoToMenu", 3f);
        }
    }

    void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
