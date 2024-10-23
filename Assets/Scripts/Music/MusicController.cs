using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject unActiveImagine;
    public bool isMusic;
    void Start()
    {
        if (isMusic)
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
        else
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


    }

    public void ToggleMute_Music()
    {
        MusicManager.musicTheme.ToggleMute();
        SetUIActive_Music();
    }
    private void SetUIActive_Music()
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
    private void SetUIActive_Sound()
    {
        Debug.Log(MusicManager.isMuted_Sound + "Is mute sound");
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
        SetUIActive_Sound();
    }
}
