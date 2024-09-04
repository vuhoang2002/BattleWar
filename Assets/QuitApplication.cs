using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void QuitApplication(bool confindExit)
    {
        Debug.Log("Thoat");
        Application.Quit();
    }
}