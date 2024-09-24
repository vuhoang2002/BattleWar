using UnityEngine;
using UnityEngine.SceneManagement; // Đảm bảo thêm namespace này

public class SceneChanger : MonoBehaviour
{
    public static string previousScene;
    public string current_sceneName;
    private void Start()
    {
        if(previousScene==null){
            previousScene=SceneManager.GetActiveScene().name;
        }
    }
    
     public void LoadNewScene(string sceneName)
    {
        // Lưu tên scene hiện tại trước khi chuyển
        previousScene = SceneManager.GetActiveScene().name;
        
        // Chuyển đến scene mới
        
           
        SceneManager.LoadScene(sceneName);
        
    }
    public void LoadNewScene_Type2()
    {
        // Lưu tên scene hiện tại trước khi chuyển
        previousScene = SceneManager.GetActiveScene().name;
        Debug.Log("Click");
        // Chuyển đến scene mới
        if(this.current_sceneName==null){
        SceneManager.LoadScene(this.current_sceneName);
        }
    }
    
     public void GoBackToPreviousScene()
    {
        // Trở về scene trước đó
        SceneManager.LoadScene(previousScene);
    }
    public void set_SceneNeedToMove(string name){
        this.current_sceneName=name;
    }
}