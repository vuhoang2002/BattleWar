using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenu;
    void Update()
    {
        // Kiểm tra nếu người chơi nhấn phím Escape để tạm dừng hoặc tiếp tục game
        
    }

    public void TogglePause()
    {
        //isPaused = !isPaused;
        pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            // Có thể hiển thị menu pause ở đây
      
    }
    public void OnPause(){
        TogglePause();
    }
    public void OnResumeGame()
{
    Time.timeScale = 1f; // Tiếp tục thời gian
    pauseMenu.SetActive(false); // Ẩn menu pause
}
}