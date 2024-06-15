using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSound : MonoBehaviour
{
    public AudioClip train_Max_sound;
    public AudioClip train_basic_sound;
    public AudioClip train_Mibble_sound;

    AudioSource audio;

    public static TrainSound ts;

    void Start()
    {
        ts = this;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play_Max()
    {
        audio.clip = train_Max_sound;
        audio.Play();
    }
    public void Play_Basic()
    {
        audio.clip = train_basic_sound;
        audio.Play();
    }
    public void Play_Middle()
    {
        audio.clip = train_Mibble_sound;
        audio.Play();
    }
}
