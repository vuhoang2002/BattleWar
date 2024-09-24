using UnityEngine;

public class FallBackOrder : MonoBehaviour
{
    //public GameObject Def_Btn;  // Thay Object bằng GameObject
    public GameObject def_Child;      
    public  GameObject atk_Child;// retreatOrder
    public bool isFallBack_Active; 
    void Start()  // Viết đúng tên phương thức khởi tạo
    {
        // Nếu cần, có thể thêm mã khởi tạo ở đây
       
    }
    

    public void HandleButtonClick()
    {
         isFallBack_Active=true;
        atk_Child.GetComponent<AttackOrder>().isAtk_Active=false;
        def_Child.GetComponent<DefenseOrder>().isDef_Active=false;
        // Tìm tất cả các PlayerController
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();

        // Cập nhật isAtk_Order thành false cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.isAtk_Order = false;
            playerController.isDef_Order = false;
            playerController.isFallBack_Order = true;  // Thiết lập isFallBack_Order thành true
            Debug.Log("Fall Back Order!!!");
            //Sound here
        }      
    }
}
