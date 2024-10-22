using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory_Or_Loss : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    public void Get_Victory()
    {
        GameObject battleCanva = GameObject.Find("BattleCanva");
        GameObject victoryUi = battleCanva.transform.Find("VictoryUI").gameObject;
        victoryUi.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Get_Loss()
    {
        GameObject battleCanva = GameObject.Find("BattleCanva");
        GameObject lossUI = battleCanva.transform.Find("LossUI").gameObject;
        lossUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
