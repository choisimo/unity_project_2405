using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneSwitcher : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene("Scene2");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Scene2");
        }
    }
}
