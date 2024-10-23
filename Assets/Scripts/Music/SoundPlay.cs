using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip audioClip_Active;
    public AudioClip audioClip_unActive;
    public void PlayBtnSound(AudioClip audioClip)
    {
        // MusicManager.soundTheme.GetComponent<AudioSource>().PlayOneShot(attackSound);
        if (audioClip != null)
        {
            MusicManager.soundTheme.ChangeToGameplayMusic(audioClip);
        }
    }
    public void PlayBtnSound(bool active)
    {
        // MusicManager.soundTheme.GetComponent<AudioSource>().PlayOneShot(attackSound);
        if (active)
        {
            if (audioClip_Active != null)
            {
                MusicManager.soundTheme.ChangeToGameplayMusic(audioClip_Active);
            }
        }
        else
        {
            if (audioClip_unActive != null)
            {
                MusicManager.soundTheme.ChangeToGameplayMusic(audioClip_unActive);
            }
        }
    }
}

