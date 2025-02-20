using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSound : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] sounds;

    private void Update()
    {
        PlaySoundWhenKeyPressed(0);
        PlaySoundWhenKeyPressed(1);
        PlaySoundWhenKeyPressed(2);
    }

    private void PlaySoundWhenKeyPressed(int indexSound)
    {
        if (Input.GetKeyDown(KeyCode.Alpha0 + indexSound) && sounds.Length > indexSound)
            PlaySound(indexSound);
    }

    public void PlaySound(int index)
    {
        source.clip = sounds[index];
        source.Play();
    }
}
