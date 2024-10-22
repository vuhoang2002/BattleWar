using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HoldOrder : MonoBehaviour
{
    public GameObject def_Child;
    public GameObject fallBack_Child; // retreatOrder
    public GameObject attack_Child;
    public bool isHold_Active;


    public GameObject players;
    Player_ListObject player_ListObject;
    List<PlayerController> playerControllers;
    public GameObject iconOrderActive;

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
        isHold_Active = true;
        def_Child.GetComponent<DefenseOrder>().isDef_Active = false;
        fallBack_Child.GetComponent<FallBackOrder>().isFallBack_Active = false;
        attack_Child.GetComponent<AttackOrder>().isAtk_Active = false;
        // Cập nhật isHold_Order thành true cho tất cả các PlayerController
        Debug.Log(playerControllers + "pl");
        playerControllers = FindAllPlayerInList(players);
        foreach (PlayerController playerController in playerControllers)
        {
            SetHoldActive(playerController);
            Show_IconOrderWhenActive(playerController);
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
                Show_IconOrderWhenActive(pl);
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
                    Show_IconOrderWhenActive(pl);
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
        Show_IconOrderWhenActive(playerController);
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
    public void Show_IconOrderWhenActive(PlayerController playerController)
    {
        GameObject player = playerController.gameObject;
        GameObject icon = Instantiate(iconOrderActive, player.transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        icon.GetComponent<IconOrder>().SetIconOrder(playerController);
    }
    public List<PlayerController> FindAllPlayerInList(GameObject gameObject)
    {
        List<PlayerController> playerControllers = new List<PlayerController>();

        // Kiểm tra xem gameObject có phải null không
        if (gameObject == null)
        {
            Debug.LogWarning("GameObject truyền vào là null!");
            return playerControllers; // Trả về danh sách rỗng
        }

        foreach (Transform child in gameObject.transform)
        {
            PlayerController playerController = child.gameObject.GetComponent<PlayerController>();
            Debug.Log("pl là " + playerController + " và child: " + child);

            if (playerController != null) // Kiểm tra nếu playerController không null
            {
                playerControllers.Add(playerController); // Thêm playerController vào danh sách
            }
        }

        return playerControllers;
    }
}