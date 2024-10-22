using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class AttackOrder : MonoBehaviour
{
    //public GameObject Def_Btn;  // Sử dụng GameObject thay vì Object
    public GameObject def_Child;
    public GameObject fallBack_Child;// retreatOrder
    public GameObject hold_Child;
    public bool isAtk_Active;
    public List<UnitListOrder> units;
    public GameObject players;
    Player_ListObject player_ListObject;
    List<PlayerController> playerControllers;
    public GameObject iconOrderActive;
    public void Show_IconOrderWhenActive(PlayerController playerController)
    {
        if (iconOrderActive == null)
        {
            Debug.LogError("iconOrderActive chưa được gán trong Inspector!");
            return; // Dừng thực hiện nếu iconOrderActive là null
        }

        if (playerController == null)
        {
            Debug.LogError("playerController là null!");
            return; // Dừng thực hiện nếu playerController là null
        }

        GameObject icon = Instantiate(iconOrderActive, playerController.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        IconOrder iconOrderComponent = icon.GetComponent<IconOrder>();
        if (iconOrderComponent != null)
        {
            iconOrderComponent.SetIconOrder(playerController);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy IconOrder component trên icon.");
        }
    }
    void Start()
    {
        players = GameObject.Find("PlayerList(Clone)");
        player_ListObject = new Player_ListObject(); // Khởi tạo player_ListObject

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
        isAtk_Active = true;
        def_Child.GetComponent<DefenseOrder>().isDef_Active = false;
        fallBack_Child.GetComponent<FallBackOrder>().isFallBack_Active = false;
        hold_Child.GetComponent<HoldOrder>().isHold_Active = false;

        // Tìm tất cả các PlayerController
        // PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();
        playerControllers = player_ListObject.FindAllPlayerInList(players);
        // Cập nhật isAtk_Order thành true cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {// toàn bộ player trên sân
            SetAttack_Active(playerController);
            Show_IconOrderWhenActive(playerController);
            //   ("Attack Order!!!");
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
            return;
        }

        // Tìm lis
        string unitNameToFind = chosenPlayer.name; // Sử dụng tên của chosenPlayer
        units = unitListManager.FindUnitsByName(unitNameToFind);

        foreach (UnitListOrder unit in units)
        {
            // So sánh với prefab của unit
            if (unit.prefab == chosenPlayer)
            {
                //   continue; // Bỏ qua chosenPlayer
            }

            // Lấy PlayerController từ prefab
            PlayerController pl = unit.prefab.GetComponent<PlayerController>();
            if (pl != null)
            {
                SetAttack_Active(pl); // Kích hoạt tấn công
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
        // Chỉ điều khiển cho tất cả đơn vị đội quan cùng loại
        GameObject chosenPlayer = PlayerController.chosenPlayer;

        // Tìm instance của UnitListManager
        UnitListManager unitListManager = FindObjectOfType<UnitListManager>();
        if (unitListManager == null)
        {
            return;
        }

        // Tìm lis
        string unitNameToFind = chosenPlayer.name; // Sử dụng tên của chosenPlayer
        units = unitListManager.FindUnitsByName(unitNameToFind);

        foreach (UnitListOrder unit in units)
        {
            // So sánh với prefab của unit
            if (unit.prefab == chosenPlayer)
            {
                PlayerController pl = unit.prefab.GetComponent<PlayerController>();
                if (pl != null)
                {
                    SetAttack_Active(pl);
                    Show_IconOrderWhenActive(pl); // Kích hoạt tấn công
                }
            }
        }
        ShowOffThisCanva();
        CancelChosen();
    }


    public void SetAttack_Active(PlayerController playerController)
    {
        playerController.isAtk_Order = true;
        playerController.isDef_Order = false;
        playerController.isFallBack_Order = false;
        playerController.isHold_Order = false;
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
