using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMusicAndSound : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnMuteMusic()
    {
        MusicManager.instance.ToggleMute(); // Gọi phương thức để tắt/mở âm thanh
    }
}
