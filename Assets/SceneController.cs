using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public AudioClip gameplayMusic; // Âm thanh cho màn chơi

    public void StartGameplay()
    {
        //  SceneManager.LoadScene("GameplayScene"); // Thay "GameplayScene" bằng tên scene của bạn
    }

    void Start()
    {
        MusicManager.instance.ChangeAudioClip(gameplayMusic);
        if (!MusicManager.isMuted)
        {
            MusicManager.instance.ChangeToGameplayMusic(gameplayMusic);
        }
    }
}