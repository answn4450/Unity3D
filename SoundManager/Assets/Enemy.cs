using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ISoundPlayable
{
    public AudioClip clip;
    private string Key;

    public void PlayCound(string _key)
    {
        SoundManager.Instance.PlaySound(_key);
    }

    void Start()
    {
        SoundManager.Instance.AddSoundClip(clip);
        Key = clip.name;
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            PlayCound(Key);
    }
}
