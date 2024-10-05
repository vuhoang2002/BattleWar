using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance; // Đổi từ private sang public
    private AudioSource audioSource;

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
}