using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeToScene(string sceneName)
    {
        //("Load to scene" + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}