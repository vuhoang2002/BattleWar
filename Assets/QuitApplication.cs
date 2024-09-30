using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    void Update()
    {
        // Kiểm tra nếu nút Back được nhấn
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        // Thoát ứng dụng
        Application.Quit();

        // Nếu đang trong Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // Xử lý khi ứng dụng bị tạm dừng (có thể do vuốt hoặc nhấn nút)
            QuitGame();
        }
    }
}