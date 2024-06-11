using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TNTrain : MonoBehaviour
{

    public Slider slider;
    public static float TNTrainspeed = 0;

    public float increment = 0.01f;
    void Start()
    {
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (TNTrainspeed != slider.value)
        {
            if (TNTrainspeed < slider.value)
                TNTrainspeed += increment;
            else
                TNTrainspeed -= increment;
        }
    }
    public void ValueChangeCheck()
    {
        TNTrainspeed = slider.value;
    }
}
