using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider BgmSlider;                         // bgm 슬라이더 
    public Slider SfxSlider;
    
    // Update is called once per frame
    public void SetBgmVolume()
    {
        SoundManager.instance.audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
    }
    
    public void SetSFXVolume()
    {
        SoundManager.instance.audioMixer.SetFloat("SFX", Mathf.Log10(SfxSlider.value) * 20);
    }
    /*
    public void SFXPlay(string sfxName,AudioClip clip)
    {
        GameObject go = new GameObject(sfxName+"Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        
        Destroy(go, clip.length);
    }
    */
}
