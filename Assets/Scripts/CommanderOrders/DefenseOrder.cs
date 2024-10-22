using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DefenseOrder : MonoBehaviour
{
    public GameObject atk_Child;
    public GameObject fallBack_Child;
    public GameObject hold_Child;
    public bool isDef_Active;
    public GameObject players;
    Player_ListObject player_ListObject = new Player_ListObject();
    List<PlayerController> playerControllers;
    public GameObject iconOrderActive;
    public void Show_IconOrderWhenActive(PlayerController playerController)
    {
        GameObject player = playerController.gameObject;
        GameObject icon = Instantiate(iconOrderActive, player.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        icon.GetComponent<IconOrder>().SetIconOrder(playerController);
    }
    void Start()
    {
        players = GameObject.Find("PlayerList(Clone)");

        StartCoroutine(WaitForPlayerList());
    }




    private IEnumerator WaitForPlayerList()
    {
        while (players == null)
        {
            players = GameObject.Find("PlayerList(Clone)");
            yield return null; // Đợi một frame
        }

        // Khi tìm thấy GameObject
        player_ListObject = new Player_ListObject();
        playerControllers = player_ListObject.FindAllPlayerInList(players);
        Debug.Log("Đã tìm thấy PlayerList(Clone) và khởi tạo player_ListObject.");
    }

    public void HandleButtonClick()
    {
        isDef_Active = true;
        atk_Child.GetComponent<AttackOrder>().isAtk_Active = false;
        fallBack_Child.GetComponent<FallBackOrder>().isFallBack_Active = false;
        hold_Child.GetComponent<HoldOrder>().isHold_Active = false;

        // Tìm tất cả các PlayerController
        playerControllers = player_ListObject.FindAllPlayerInList(players);
        // Cập nhật isDef_Order thành true cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {
            SetDefActive(playerController);
            Show_IconOrderWhenActive(playerController);

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
                Show_IconOrderWhenActive(pl);
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
                    Show_IconOrderWhenActive(pl);
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