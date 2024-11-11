using UnityEngine;
using UnityEngine.SceneManagement; // Đảm bảo thêm namespace này

public class SceneChanger : MonoBehaviour
{
    public static string previousScene;
    public string current_sceneName;
    private void Start()
    {
        if (previousScene == null)
        {
            previousScene = SceneManager.GetActiveScene().name;
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
        // Chuyển đến scene mới
        if (this.current_sceneName == null)
        {
            SceneManager.LoadScene(this.current_sceneName);
        }
    }

    public void GoBackToPreviousScene()
    {
        // Trở về scene trước đó
        SceneManager.LoadScene(previousScene);
    }
    public void set_SceneNeedToMove(string name)
    {
        this.current_sceneName = name;
    }
    public void GoToLevelSelectedScene()
    {
        LoadNewScene("LevelSelectScene");
    }
    public void GoToAgain()
    {
        current_sceneName = SceneManager.GetActiveScene().name;
        LoadNewScene(current_sceneName);
    }
    public void LoadToNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Đang là màn "+currentSceneName );
        if (currentSceneName.StartsWith("Lv"))
        {
            string levelNumberString = currentSceneName.Substring(2); // Remove the "lv" prefix
            int levelNumber;
            if (int.TryParse(levelNumberString, out levelNumber))
            {
                levelNumber++;
                string nextLevelSceneName = "Lv" + levelNumber.ToString();
                Debug.Log("Chuyển màn "+ nextLevelSceneName);
                SceneManager.LoadScene(nextLevelSceneName);
            }
        }
    }

}