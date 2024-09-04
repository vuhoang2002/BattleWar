using UnityEngine;

public class AttackOrder : MonoBehaviour
{
    //public GameObject Def_Btn;  // Sử dụng GameObject thay vì Object
     private  Transform def_Child;
    void Start()
    {
        // Tìm đối tượng con bằng tên
        def_Child= transform.parent.Find("Def_Btn");

        
    }

    public void HandleButtonClick()
    {
        // Tìm tất cả các PlayerController
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();

        // Cập nhật isAtk_Order thành true cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.isAtk_Order = true;
            playerController.isDef_Order = false;
            playerController.isFallBack_Order = false;

            Debug.Log("Attack Order!!!");
        }

        // Kiểm tra và cập nhật Def_Btn nếu nó không phải là null
        if (def_Child != null)
        {
            // Truy cập script DefenseOrder trên đối tượng con và thay đổi giá trị của isDef_Btn_Active
            DefenseOrder defenseOrder = def_Child.GetComponent<DefenseOrder>();
            if (defenseOrder != null)
            {
                defenseOrder.isDef_Btn_Active = false;
            }
            else
            {
                Debug.LogWarning("DefenseOrder component not found on def_Child.");
            }
        }
        else
        {
            Debug.LogWarning("def_Child is not assigned.");
        }
    }
    
}
