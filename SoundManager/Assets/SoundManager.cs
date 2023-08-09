using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance {get; set;} = null;

    public AudioSource audioSource;
    public Dictionary<string, AudioClip> soundDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = new SoundManager();
            DontDestroyOnLoad(this.gameObject);
        }    
        else
            Destroy(this.gameObject);

        audioSource = GetComponent<AudioSource>();
        soundDictionary = new Dictionary<string, AudioClip>();
    }

    public void PlaySound(string _key)
    {

    }

    public void PlayBGM(string _key) 
    {
        if (!soundDictionary.ContainsKey(_key))
        {
            Debug.Log("BGM 없음...");
        }

        audioSource.clip= soundDictionary[_key];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void AddSoundClip(AudioClip _clip)
    {
        if (soundDictionary.ContainsKey(_clip.name))
        {
            Debug.Log("이미 있음");
        }
        else
            soundDictionary.Add(_clip.name, _clip);
    }
}
