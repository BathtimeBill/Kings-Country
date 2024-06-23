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
        musicAudioMixer.SetFloat("MusicVolume", sliderValue * 0.1f); 
    }

    public void SetVolumeSFX(float sliderValue)
    {
        SFXAudioMixer.SetFloat("SFXVolume", sliderValue * 0.1f);
    }
}
