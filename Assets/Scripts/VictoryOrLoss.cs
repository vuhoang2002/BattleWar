using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory_Or_Loss : MonoBehaviour
{
    public void Get_Victory()
    {
        ShowUI("VictoryUI");
    }

    public void Get_Loss()
    {
        ShowUI("LoseUI");
    }

    private void ShowUI(string uiName)
    {
        GameObject battleCanva = GameObject.Find("BattleCanva");
        if (battleCanva == null)
        {
            Debug.LogError("BattleCanva not found!");
            return; // Dừng lại nếu không tìm thấy BattleCanva
        }

        GameObject ui = battleCanva.transform.Find(uiName)?.gameObject;
        if (ui == null)
        {
            Debug.LogError(uiName + " not found in BattleCanva!");
            return; // Dừng lại nếu không tìm thấy UI
        }
        Debug.Log("UI is" + ui);

        ShowCanva(ui);
        // StartCoroutine(WaitBeforeShowCanva(ui));
        // Dừng thời gian
    }

    IEnumerator WaitBeforeShowCanva(GameObject ui)
    {
        yield return new WaitForSeconds(2f);
        ui.SetActive(true); // Hiển thị UI sau khi chờ
        Time.timeScale = 0f;
    }
    void ShowCanva(GameObject ui)
    {
        ui.SetActive(true); // Hiển thị UI sau khi chờ
        Time.timeScale = 0f;
    }
}