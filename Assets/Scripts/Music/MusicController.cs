using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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

    public void ToggleMute()
    {
        MusicManager.instance.ToggleMute();
    }
}
