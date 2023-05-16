using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    public void PlaySound(AudioClip clip, Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();

        StartCoroutine(StopPlayCo());
    }

    IEnumerator StopPlayCo()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        SoundManager.instance.ReturnSoundComponent(this);
    }
}
