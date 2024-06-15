using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntersectionController : MonoBehaviour
{
    public GameObject[] wpA;
    public GameObject[] wpB;

    public Slider slider;
    void Start()
    {
        EnableIntersection(0);
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EnableIntersection(int part)
    {
        Debug.Log(part);
        if(part == 0) 
        {
            foreach(GameObject point in wpA) 
            {
                point.SetActive(true);
            }
            foreach (GameObject point in wpB)
            {
                point.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject point in wpA)
            {
                point.SetActive(false);
            }
            foreach (GameObject point in wpB)
            {
                point.SetActive(true);
            }
        }
    }

    public void ValueChangeCheck()
    {
        if(slider.value == 0)
        {
            EnableIntersection(0);
        }
        else
            EnableIntersection(1);
    }
}
