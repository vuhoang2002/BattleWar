using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider; // Tham chiếu đến Slider
    public bool isMusic;

    void Start()
    {
        // Đặt giá trị của slider theo âm lượng hiện tại
        if (isMusic)
        {
            volumeSlider.value = MusicManager.musicTheme.audioSource.volume;
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            volumeSlider.value = MusicManager.soundTheme.audioSource.volume;
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged_Sound);
        }
    }


    private void OnSliderValueChanged(float value)
    {
        // Đặt âm lượng ngay lập tức
        MusicManager.musicTheme.audioSource.volume = value;
    }
    private void OnSliderValueChanged_Sound(float value)
    {
        // Đặt âm lượng ngay lập tức
        MusicManager.soundTheme.audioSource.volume = value;
    }

}