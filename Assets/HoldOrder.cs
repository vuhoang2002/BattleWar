using UnityEngine;
using System.Collections.Generic;

public class HoldOrder : MonoBehaviour
{
    public GameObject def_Child;
    public GameObject fallBack_Child; // retreatOrder
    public GameObject attack_Child;
    public bool isHold_Active;


    void Start()
    {
        // Tìm đối tượng con bằng tên
        // def_Child = transform.parent.Find("Def_Btn");
    }

    public void HandleButtonClick()
    {
        isHold_Active = true;
        def_Child.GetComponent<DefenseOrder>().isDef_Active = false;
        fallBack_Child.GetComponent<FallBackOrder>().isFallBack_Active = false;
        attack_Child.GetComponent<AttackOrder>().isAtk_Active = false;

        // Tìm tất cả các PlayerController
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();

        // Cập nhật isHold_Order thành true cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {

            SetHoldActive(playerController);
            // ("Hold Order!!!");
        }
    }

    public void OrderOneUnitType()
    {
        // Chỉ điều khiển cho tất cả đơn vị đội quan cùng loại
        GameObject chosenPlayer = PlayerController.chosenPlayer;

        // Tìm instance của UnitListManager
        UnitListManager unitListManager = FindObjectOfType<UnitListManager>();
        if (unitListManager == null)
        {
            Debug.LogError("Không tìm thấy UnitListManager trong scene.");
            return;
        }

        // Tìm list
        string unitNameToFind = chosenPlayer.name; // Sử dụng tên của chosenPlayer
        List<UnitListOrder> units = unitListManager.FindUnitsByName(unitNameToFind);

        foreach (UnitListOrder unit in units)
        {
            // So sánh với prefab của unit
            if (unit.prefab == chosenPlayer)
            {
                //  continue; // Bỏ qua chosenPlayer
            }

            // Lấy PlayerController từ prefab
            PlayerController pl = unit.prefab.GetComponent<PlayerController>();
            if (pl != null)
            {
                SetHoldActive(pl);
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy PlayerController trên prefab '{unit.prefab.name}'");
            }
        }
        ShowOffThisCanva();

    }
    public void CancelChosen()
    {
        CancelChosen cancelChosen = new CancelChosen();
        cancelChosen.HandleButtonClick();
    }

    public void OrderThisUnit()
    {
        // Chỉ điều khiển cho tất cả đơn vị đội quan cùng loại
        GameObject chosenPlayer = PlayerController.chosenPlayer;

        // Tìm instance của UnitListManager
        UnitListManager unitListManager = FindObjectOfType<UnitListManager>();
        if (unitListManager == null)
        {
            return;
        }

        // Tìm list
        string unitNameToFind = chosenPlayer.name; // Sử dụng tên của chosenPlayer
        List<UnitListOrder> units = unitListManager.FindUnitsByName(unitNameToFind);

        foreach (UnitListOrder unit in units)
        {
            // So sánh với prefab của unit
            if (unit.prefab == chosenPlayer)
            {
                PlayerController pl = unit.prefab.GetComponent<PlayerController>();
                if (pl != null)
                {
                    SetHoldActive(pl); // Kích hoạt giữ
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy PlayerController trên prefab '{unit.prefab.name}'");
                } //  continue; // Bỏ qua chosenPlayer
            }
        }
        ShowOffThisCanva();
        CancelChosen();

    }

    public void SetHoldActive(PlayerController playerController)
    {
        playerController.isHold_Order = true;  // Kích hoạt giữ
        playerController.isAtk_Order = false;   // Tắt tấn công
        playerController.isDef_Order = false;   // Tắt phòng thủ
        playerController.isFallBack_Order = false; // Tắt rút lui

        // Lấy tọa độ của đối tượng cha và gán vào hold_Position
        if (playerController.transform.parent != null)
        {
            playerController.hold_Position = playerController.transform.position;
        }
        else
        {
            // Nếu không có cha, có thể gán hold_Position bằng Vector3.zero hoặc một giá trị khác
            playerController.hold_Position = Vector3.zero;
        }
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