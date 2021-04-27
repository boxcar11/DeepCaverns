using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{

    public AudioMixer mixer;

    public void AdjustMasterVolume(float value)
    {
        mixer.SetFloat("Master", Mathf.Log(value) * 20);
    }

    public void AdjustMusicVolume(float value)
    {
        mixer.SetFloat("Music", Mathf.Log(value) * 20);
    }

    public void AdjustSFXVolume(float value)
    {
        mixer.SetFloat("SFX", Mathf.Log(value) * 20);
    }
}
