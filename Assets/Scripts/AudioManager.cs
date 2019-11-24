using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioPlayer;
    public AudioClip swordToSword, swordToBody, defend;
    // Start is called before the first frame update
    void Awake()
    {
        audioPlayer = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound)
    {
        audioPlayer.clip = sound;
        audioPlayer.loop = false;
        audioPlayer.volume = 1.0f;
        audioPlayer.Play();
    }
}
