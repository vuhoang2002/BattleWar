using UnityEngine;

public class FallBackOrder : MonoBehaviour
{
    //public GameObject Def_Btn;  // Thay Object bằng GameObject
        private  Transform def_Child;
    void Start()  // Viết đúng tên phương thức khởi tạo
    {
        // Nếu cần, có thể thêm mã khởi tạo ở đây
        def_Child= transform.parent.Find("Def_Btn");
    }
    

    public void HandleButtonClick()
    {
        // Tìm tất cả các PlayerController
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();

        // Cập nhật isAtk_Order thành false cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.isAtk_Order = false;
            playerController.isDef_Order = false;
            playerController.isFallBack_Order = true;  // Thiết lập isFallBack_Order thành true
            Debug.Log("Fall Back Order!!!");
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
