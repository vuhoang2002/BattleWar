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
    public EnemyManager Find_EnemyManager()
    {
        GameObject eUnitList = GameObject.Find("EUnit_List");
        return eUnitList.GetComponent<EnemyManager>();
    }
    public void Show_FunctionButton(AbilityCount abilityCount)
    {
        GameObject functionCanva = GameObject.Find("BattleCanva");
        functionCanva = functionCanva.transform.Find("FunctionCanva").gameObject;
        // Transform transformChild;
        switch (abilityCount)
        {
            case AbilityCount.Three:
                Support_For_FunctionCanva("Abl3_Btn", true, functionCanva);
                Support_For_FunctionCanva("Abl2_Btn", true, functionCanva);
                Support_For_FunctionCanva("Abl1_Btn", true, functionCanva);
                break;
                break;
            case AbilityCount.Two:
                Support_For_FunctionCanva("Abl2_Btn", true, functionCanva);
                Support_For_FunctionCanva("Abl1_Btn", true, functionCanva);
                break;
            case AbilityCount.One:
                Support_For_FunctionCanva("Abl1_Btn", true, functionCanva);
                break;
            case AbilityCount.Zero:
                Support_For_FunctionCanva("Atck_Btn", true, functionCanva);
                break;
        }
        Support_For_FunctionCanva("Atck_Btn", true, functionCanva);
    }
    private void Support_For_FunctionCanva(string nameObject, bool active, GameObject functionCanva)
    {
        Transform transformChild = functionCanva.transform.Find(nameObject);
        transformChild.gameObject.SetActive(active);
    }
    public void Off_FunctionButton()
    {
        GameObject functionCanva = GameObject.Find("FunctionCanva");
        Support_For_FunctionCanva("Abl3_Btn", false, functionCanva);
        Support_For_FunctionCanva("Abl2_Btn", false, functionCanva);
        Support_For_FunctionCanva("Abl1_Btn", false, functionCanva);
        Support_For_FunctionCanva("Atck_Btn", false, functionCanva);
    }
}
