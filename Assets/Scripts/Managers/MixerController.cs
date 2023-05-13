using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer musicAudioMixer;
    [SerializeField] private AudioMixer SFXAudioMixer;

    public void SetVolume(float sliderValue)
    {
        musicAudioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20); 
    }

    public void SetVolumeSFX(float sliderValue)
    {
        SFXAudioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }
}
