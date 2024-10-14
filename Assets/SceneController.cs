using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public AudioClip gameplayMusic; // Âm thanh cho màn chơi

    public void StartGameplay()
    {
        // Chuyển nhạc nền sang âm thanh mới
        
        
        // Chuyển sang màn chơi
      //  SceneManager.LoadScene("GameplayScene"); // Thay "GameplayScene" bằng tên scene của bạn
    }
  
    void Start()
    {
        MusicManager.instance.ChangeToGameplayMusic(gameplayMusic);
    }
}