using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject unActiveImagine;
    void Start()
    {
        if (MusicManager.isMuted_Music)
        {
            unActiveImagine.SetActive(true);
        }
        else
        {
            unActiveImagine.SetActive(false);
        }
    }
    // Update is called once per frame
    // public void Mute()
    // {
    //     MusicManager.instance.Mute(); // Tắt âm thanh
    // }

    // public void Unmute()
    // {
    //     MusicManager.instance.Unmute();
    //     // audioSource.Play(); // Mở âm thanh
    // }

    public void ToggleMute_Music()
    {
        MusicManager.musicTheme.ToggleMute();
        SetUIActive_Music();
    }
    public void SetUIActive_Music()
    {
        if (MusicManager.isMuted_Music)
        {
            unActiveImagine.SetActive(true);
        }
        else
        {
            unActiveImagine.SetActive(false);
        }
    }
    public void SetUIActive_Sound()
    {
        if (MusicManager.isMuted_Sound)
        {
            unActiveImagine.SetActive(true);
        }
        else
        {
            unActiveImagine.SetActive(false);
        }
    }
    public void ToggleMute_Sound()
    {
        MusicManager.soundTheme.ToggleMute_Sound();
        SetUIActive_Music();
    }
}
