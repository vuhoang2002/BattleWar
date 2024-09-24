using UnityEngine;

public class AttackOrder : MonoBehaviour
{
    //public GameObject Def_Btn;  // Sử dụng GameObject thay vì Object
    public  GameObject def_Child;
    public  GameObject fallBack_Child;// retreatOrder
    public bool isAtk_Active;

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
        // Tìm tất cả các PlayerController
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();

        // Cập nhật isAtk_Order thành true cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.isAtk_Order = true;
            playerController.isDef_Order = false;
            playerController.isFallBack_Order = false;

         //   Debug.Log("Attack Order!!!");
        }  
    }
    
}
