using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVal : MonoBehaviour
{
    public static StaticVal Instance;


    private int randomIndex;
    private int MaxSpeed = 20;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetRandomIndex(int index)
    {
        randomIndex = index;
    }

    public int GetRandomIndex()
    {
        return randomIndex;
    }

    public void SetMS(int s)
    {
        MaxSpeed = s;
    }

    public int GetMS()
    {
        return MaxSpeed;
    }
}
