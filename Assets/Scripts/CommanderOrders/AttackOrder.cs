using UnityEngine;
using System.Collections.Generic; 
public class AttackOrder : MonoBehaviour
{
    //public GameObject Def_Btn;  // Sử dụng GameObject thay vì Object
    public  GameObject def_Child;
    public  GameObject fallBack_Child;// retreatOrder
     public  GameObject hold_Child;
    public bool isAtk_Active;
    public List<UnitListOrder> units;

    void Start()
    {
        // Tìm đối tượng con bằng tên
        //def_Child= transform.parent.Find("Def_Btn");

        
    }

    public void HandleButtonClick()
    {
        isAtk_Active=true;
        def_Child.GetComponent<DefenseOrder>().isDef_Active=false;
        fallBack_Child.GetComponent<FallBackOrder>().isFallBack_Active=false;
         hold_Child.GetComponent<HoldOrder>().isHold_Active=false;

        // Tìm tất cả các PlayerController
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();

        // Cập nhật isAtk_Order thành true cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {// toàn bộ player trên sân
           SetAttack_Active(playerController);
         //   Debug.Log("Attack Order!!!");
        }  
    }
public void OrderOneUnitType()
{
    // Chỉ điều khiển cho tất cả đơn vị đội quan cùng loại
    GameObject chosenPlayer = PlayerController.playerHasBeenChosen;
    Debug.Log("chosen là " + chosenPlayer);

    // Tìm instance của UnitListManager
    UnitListManager unitListManager = FindObjectOfType<UnitListManager>();
    if (unitListManager == null)
    {
        Debug.LogError("Không tìm thấy UnitListManager trong scene.");
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
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy PlayerController trên prefab '{unit.prefab.name}'");
        }
    }
}

    
    public void  SetAttack_Active(PlayerController playerController){
         playerController.isAtk_Order = true;
            playerController.isDef_Order = false;
            playerController.isFallBack_Order = false;
            playerController.isHold_Order=false;
    }
    
}
