using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    
    public AudioMixer audioMixer; // 오디오 믹서

    [SerializeField] private SoundComponent soundComponent_prefab;
    private Stack<SoundComponent> closed = new Stack<SoundComponent>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject);
        }
    }

    public void PlaySoundComponent(AudioClip clip, Vector3 position)
    {
        if (closed.Count > 0)
        {
            SoundComponent popped = closed.Pop();
            popped.PlaySound(clip, position);
        }
        else
        {
            SoundComponent newSoundComponent = Instantiate(soundComponent_prefab);
            newSoundComponent.transform.SetParent(this.transform);
            newSoundComponent.PlaySound(clip, position);
        }
    }

    public void ReturnSoundComponent(SoundComponent component)
    {
        component.gameObject.SetActive(false);
        closed.Push(component);
    }
}
