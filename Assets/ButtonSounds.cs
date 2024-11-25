using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public AudioClip buttonSound;
    public AudioClip plauSound;
    public AudioSource source;


    public void PlayButtonSound()
    {
        source.clip = buttonSound;
        source.Play();
    }

    public void PlaySound()
    {
        source.clip = plauSound;
        source.Play();
    }

}
