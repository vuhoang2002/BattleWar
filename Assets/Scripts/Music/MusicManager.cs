using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance; // Đổi từ private sang public
    private AudioSource audioSource;
    private bool isMuted = false; // Biến để theo dõi trạng thái âm thanh

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject); // Giữ đối tượng này không bị hủy khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Nếu đã có, hủy đối tượng mới
        }
    }

    void Start()
    {
        audioSource.Play(); // Bắt đầu phát nhạc nền ban đầu
    }

    public void ChangeToGameplayMusic(AudioClip gameplayMusic)
    {
        audioSource.Stop(); // Dừng nhạc nền hiện tại
        audioSource.clip = gameplayMusic; // Đặt âm thanh mới
        audioSource.Play(); // Phát âm thanh mới
    }

    public void Mute()
    {
        isMuted = true; // Đặt trạng thái là tắt âm
        audioSource.mute = true; // Tắt âm thanh
    }

    public void Unmute()
    {
        isMuted = false; // Đặt trạng thái là bật âm
        audioSource.mute = false; // Mở âm thanh
    }

    public void ToggleMute()
    {
        if (isMuted)
            Unmute(); // Nếu đang tắt, gọi Unmute
        else
            Mute(); // Nếu đang bật, gọi Mute
    }
}