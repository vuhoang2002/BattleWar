using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager musicTheme; // Đổi từ private sang public
    public static MusicManager soundTheme;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public static bool isMuted_Music = false; // Biến để theo dõi trạng thái âm thanh
    public static bool isMuted_Sound = false;
    public enum MusicAndSound
    {
        Music,
        Sound
    }
    public MusicAndSound musicAndSound;

    void Awake()
    {
        if (musicAndSound == MusicAndSound.Music)
        {
            if (musicTheme == null)
            {
                musicTheme = this;
                musicTheme.audioSource = GetComponent<AudioSource>();
                DontDestroyOnLoad(gameObject); // Giữ đối tượng này không bị hủy khi chuyển scene
            }
            else
            {
                Destroy(gameObject); // Nếu đã có, hủy đối tượng mới
            }
        }
        if (musicAndSound == MusicAndSound.Sound)
        {
            if (soundTheme == null)
            {
                soundTheme = this;
                soundTheme.audioSource = GetComponent<AudioSource>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject); // Nếu đã có, hủy đối tượng mới
            }
        }

    }

    void Start()
    {
        //audioSource.Play(); // Bắt đầu phát nhạc nền ban đầu
        if (musicAndSound == MusicAndSound.Music)
        {
            ChangeToGameplayMusic(audioClip);
        }
    }

    public void ChangeToGameplayMusic(AudioClip gameplayMusic)
    {
        audioSource.Stop(); // Dừng nhạc nền hiện tại
        audioSource.clip = gameplayMusic; // Đặt âm thanh mới
        audioSource.Play(); // Phát âm thanh mới
    }
    public void ChangeAudioClip(AudioClip gameplayMusic)
    {
        audioSource.clip = gameplayMusic; // Đặt âm thanh mới

    }

    public void Mute_Music()
    {
        isMuted_Music = true; // Đặt trạng thái là tắt âm
        //audioSource.mute = true; // Tắt âm thanh
        audioSource.Stop();
    }

    public void Unmute_Music()
    {
        isMuted_Music = false; // Đặt trạng thái là bật âm
        audioSource.mute = false; // Mở âm thanh
        audioSource.Play();

    }

    public void ToggleMute()
    {
        if (isMuted_Music)
            Unmute_Music(); // Nếu đang tắt, gọi Unmute
        else
            Mute_Music(); // Nếu đang bật, gọi Mute
    }
    public void Mute_Sound()
    {
        isMuted_Sound = true; // Đặt trạng thái là tắt âm
        soundTheme.audioSource.mute = true; // Tắt âm thanh
                                            // audioSource.Stop();
        Debug.Log("Mute");
    }

    public void Unmute_Sound()
    {
        Debug.Log("un mute");
        isMuted_Sound = false; // Đặt trạng thái là bật âm
        soundTheme.audioSource.mute = false; // Mở âm thanh
                                             //  audioSource.Play();
    }

    public void ToggleMute_Sound()
    {
        Debug.Log("toggle mute" + isMuted_Sound);
        if (isMuted_Sound)
        {
            Unmute_Sound(); // Nếu đang tắt, gọi Unmu
        }
        else
        {
            Mute_Sound(); // Nếu đang bật, gọi Mute
        }
    }
    public void PlaySound(AudioClip gameplaySound)
    {
        //soundTheme.audioSource.Stop(); // Dừng nhạc nền hiện tại
        soundTheme.audioSource.clip = gameplaySound; // Đặt âm thanh mới
        soundTheme.audioSource.Play(); // Phát âm thanh mới
    }
    public void SlideMusic(float targetVolume, float duration)
    {
        StartCoroutine(SlideVolume(targetVolume, duration));
    }

    private IEnumerator SlideVolume(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / duration);
            yield return null;
        }

        audioSource.volume = targetVolume; // Đảm bảo âm lượng cuối cùng là chính xác
    }
    public void SlideSoundAll(float targetVolume, float duration)
    {
        StartCoroutine(SlideSound(targetVolume, duration));
    }

    private IEnumerator SlideSound(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / duration);
            yield return null;
        }

        audioSource.volume = targetVolume; // Đảm bảo âm lượng cuối cùng là chính xác
    }
}