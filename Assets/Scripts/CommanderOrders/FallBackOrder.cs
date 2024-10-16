using UnityEngine;
using System.Collections.Generic;

public class FallBackOrder : MonoBehaviour
{
    public GameObject def_Child;
    public GameObject attack_Child;
    public GameObject hold_Child;
    public bool isFallBack_Active;

    void Start()
    {
        // Tìm đối tượng con bằng tên
    }

    public void HandleButtonClick()
    {
        isFallBack_Active = true;
        def_Child.GetComponent<DefenseOrder>().isDef_Active = false;
        attack_Child.GetComponent<AttackOrder>().isAtk_Active = false;
        hold_Child.GetComponent<HoldOrder>().isHold_Active = false;

        // Tìm tất cả các PlayerController
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();

        // Cập nhật isFallBack_Order thành true cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {
            SetFallBackActive(playerController);
        }
    }

    public void OrderOneUnitType()
    {
        GameObject chosenPlayer = PlayerController.chosenPlayer;

        // Tìm instance của UnitListManager
        UnitListManager unitListManager = FindObjectOfType<UnitListManager>();
        if (unitListManager == null)
        {
            Debug.LogError("Không tìm thấy UnitListManager trong scene.");
            return;
        }

        string unitNameToFind = chosenPlayer.name;
        List<UnitListOrder> units = unitListManager.FindUnitsByName(unitNameToFind);

        foreach (UnitListOrder unit in units)
        {
            if (unit.prefab == chosenPlayer)
            {
                // continue; // Bỏ qua chosenPlayer
            }

            PlayerController pl = unit.prefab.GetComponent<PlayerController>();
            if (pl != null)
            {
                SetFallBackActive(pl);
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
            Debug.LogError("Không tìm thấy UnitListManager trong scene.");
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
                    SetFallBackActive(pl);
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy PlayerController trên prefab '{unit.prefab.name}'");
                }
            }

            ShowOffThisCanva();
            CancelChosen();
        }
    }
    public void CancelChosen()
    {
        CancelChosen cancelChosen = new CancelChosen();
        cancelChosen.HandleButtonClick();
    }


    public void SetFallBackActive(PlayerController playerController)
    {
        playerController.isFallBack_Order = true;  // Kích hoạt rút lui
        playerController.isAtk_Order = false;       // Tắt tấn công
        playerController.isDef_Order = false;       // Tắt phòng thủ
        playerController.isHold_Order = false;      // Tắt giữ
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
}