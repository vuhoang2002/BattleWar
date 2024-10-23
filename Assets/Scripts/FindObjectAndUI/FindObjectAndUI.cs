using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectAndUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject Find_PlayerList()
    {
        GameObject playerList = null;
        playerList = GameObject.Find("PlayerList(Clone)");
        if (playerList != null)
        {
            return playerList;
        }
        Debug.Log("Không tìm thấy Object_ PlayerList");
        return null;
    }
    public GameObject Find_OrderPanelFor_OneUnitType()
    {
        GameObject BattleCanvas = GameObject.Find("BattleCanva");
        Transform findCanva = BattleCanvas.transform.Find("OrderCanva");
        findCanva = findCanva.transform.Find("PanelOrder_UnitType");
        // findCanva.gameObject.SetActive(false);
        return findCanva.gameObject;
    }
    public GameObject Find_OrderPanel_OneUnit()
    {
        GameObject BattleCanvas = GameObject.Find("BattleCanva");
        Transform findCanva = BattleCanvas.transform.Find("OrderCanva");
        findCanva = findCanva.transform.Find("PanelOrder_OneUnit");
        // findCanva.gameObject.SetActive(false);
        return findCanva.gameObject;
    }
    public GameObject Find_OrderSelectUnit_Buton()
    {
        GameObject BattleCanvas = GameObject.Find("BattleCanva");
        Transform findCanva = BattleCanvas.transform.Find("OrderCanva");
        findCanva = findCanva.transform.Find("SelectUnit_Btn");
        // findCanva.gameObject.SetActive(false);
        return findCanva.gameObject;
    }
}
