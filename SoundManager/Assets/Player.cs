using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ISoundPlayable
{
    public AudioClip clip;

    public void PlayCound(string _key)
    {
        SoundManager.Instance.PlayBGM(_key);
    }

    void Start()
    {
        SoundManager.Instance.AddSoundClip(clip);
        PlayCound("BGM_01");
    }

    void Update()
    {

    }
}
