using UnityEngine;

public class DefenseOrder : MonoBehaviour
{
    // Tham chiếu đến FormationManager
    private FormationManager fM;
    private FormationManagerUpgrade fmU;
    
    public  GameObject atk_Child;
    public  GameObject fallBack_Child;// retreatOrder
    public bool isDef_Active;

    private void Start()
    {
        // Lấy tham chiếu đến FormationManager từ cùng một GameObject
         fmU=GetComponent<FormationManagerUpgrade>();
    }

    public void HandleButtonClick()
    {
        isDef_Active=true;
        atk_Child.GetComponent<AttackOrder>().isAtk_Active=false;
        fallBack_Child.GetComponent<FallBackOrder>().isFallBack_Active=false;

        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();

        // Cập nhật isAtk_Order thành true cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.isAtk_Order = false;
            playerController.isDef_Order = true;
            playerController.isFallBack_Order = false;
           
         //   Debug.Log("Attack Order!!!");
        }

        
    }

}
   
    
    

