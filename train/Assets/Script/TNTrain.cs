using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//슬라이더를 이용해 기차의 속도를 조절하는 스크립트
public class TNTrain : MonoBehaviour
{

    public Slider slider;
    public static float TNTrainspeed = 0;
    public int MaxSpeed;

    public static TNTrain Instance;
    public float increment = 0.01f;

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
    void Start()
    {
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Update is called once per frame
    void Update()
    {
        MaxSpeed = StaticVal.Instance.GetMS();

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
        slider.maxValue = MaxSpeed;
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }
}
