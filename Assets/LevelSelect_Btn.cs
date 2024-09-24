using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Đảm bảo thêm namespace này

public class LevelSelect_Btn : MonoBehaviour
{
  
       
    private static string previousScene;
    public string scenceLevel="Level1";
    void Start()
    {
        //
        SceneChanger sceneChanger= GetComponent<SceneChanger>();

        sceneChanger.set_SceneNeedToMove(scenceLevel);
    if(previousScene==null){
            previousScene=SceneManager.GetActiveScene().name;
        }
    }
   public void setLvBtn(string name){
    scenceLevel=name;
    }

    // Update is called once per frame
    public void LoadLevelScene()
    {
        // Lưu tên scene hiện tại trước khi chuyển
        previousScene = SceneManager.GetActiveScene().name;
        
        // Chuyển đến scene mới
        SceneManager.LoadScene(scenceLevel);
        
    }

}
