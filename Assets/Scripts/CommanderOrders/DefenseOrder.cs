using UnityEngine;
using System.Collections.Generic;

public class DefenseOrder : MonoBehaviour
{
    public GameObject atk_Child;
    public GameObject fallBack_Child;
    public GameObject hold_Child;
    public bool isDef_Active;

    void Start()
    {
        // Tìm đối tượng con bằng tên
    }

    public void HandleButtonClick()
    {
        isDef_Active = true;
        atk_Child.GetComponent<AttackOrder>().isAtk_Active = false;
        fallBack_Child.GetComponent<FallBackOrder>().isFallBack_Active = false;
        hold_Child.GetComponent<HoldOrder>().isHold_Active = false;

        // Tìm tất cả các PlayerController
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();

        // Cập nhật isDef_Order thành true cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {
            SetDefActive(playerController);
        }
    }

    public void OrderOneUnitType()
    {
        GameObject chosenPlayer = PlayerController.chosenPlayer;

        // Tìm instance của UnitListManager
        UnitListManager unitListManager = FindObjectOfType<UnitListManager>();
        if (unitListManager == null)
        {
            return;
        }

        string unitNameToFind = chosenPlayer.name;
        List<UnitListOrder> units = unitListManager.FindUnitsByName(unitNameToFind);

        foreach (UnitListOrder unit in units)
        {
            if (unit.prefab == chosenPlayer)
            {
                //  continue; // Bỏ qua chosenPlayer
            }

            PlayerController pl = unit.prefab.GetComponent<PlayerController>();
            if (pl != null)
            {
                SetDefActive(pl);
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy PlayerController trên prefab '{unit.prefab.name}'");
            }
        }
        ShowOffThisCanva();
    }
    public void OrderThisUnit()
    {
        GameObject chosenPlayer = PlayerController.chosenPlayer;

        // Tìm instance của UnitListManager
        UnitListManager unitListManager = FindObjectOfType<UnitListManager>();
        if (unitListManager == null)
        {
            return;
        }

        string unitNameToFind = chosenPlayer.name;
        List<UnitListOrder> units = unitListManager.FindUnitsByName(unitNameToFind);

        foreach (UnitListOrder unit in units)
        {
            if (unit.prefab == chosenPlayer)
            {
                PlayerController pl = unit.prefab.GetComponent<PlayerController>();
                if (pl != null)
                {
                    SetDefActive(pl);
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy PlayerController trên prefab '{unit.prefab.name}'");
                }
            }
        }
        ShowOffThisCanva();
        CancelChosen();
    }



    public void SetDefActive(PlayerController playerController)
    {
        playerController.isDef_Order = true;  // Kích hoạt phòng thủ
        playerController.isAtk_Order = false;  // Tắt tấn công
        playerController.isHold_Order = false;  // Tắt giữ
        playerController.isFallBack_Order = false; // Tắt rút lui
    }
    public void ShowOffThisCanva()
    {
        GameObject BattleCanva = GameObject.Find("BattleCanva");
        Transform orderCanva = BattleCanva.transform.Find("OrderCanva");
        Transform childCanva = orderCanva.transform.Find("PanelOrder_UnitType");
        childCanva.gameObject.SetActive(false);
        childCanva = orderCanva.transform.Find("PanelOrder_OneUnit");
        childCanva.gameObject.SetActive(false);
        childCanva = orderCanva.transform.Find("SelectUnit_Btn");
        childCanva.gameObject.SetActive(false);

    }
    public void CancelChosen()
    {
        CancelChosen cancelChosen = new CancelChosen();
        cancelChosen.HandleButtonClick();
    }

}