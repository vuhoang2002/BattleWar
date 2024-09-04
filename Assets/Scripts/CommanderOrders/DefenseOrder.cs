using UnityEngine;

public class DefenseOrder : MonoBehaviour
{
    // Tham chiếu đến FormationManager
    private FormationManager fM;
    public bool isDef_Btn_Active=true;

    private void Start()
    {
        // Lấy tham chiếu đến FormationManager từ cùng một GameObject
        
    }

    public void HandleButtonClick()
    {
       FormationUnitByDefenseButton();
        
        isDef_Btn_Active=true;
        
    }
    public bool getDefOrder(){
        return isDef_Btn_Active;
    }
    
     public void  FormationUnitByDefenseButton(){
        fM = GetComponent<FormationManager>();

        // Kiểm tra xem đối tượng FormationManager đã được tìm thấy chưa
        if (fM == null)
        {
            Debug.LogWarning("FormationManager component is not found on the same GameObject!");
        }
        // Tìm tất cả các PlayerController
        PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Cập nhật isAtk_Order thành false và isDef_Order thành true cho tất cả các PlayerController
        foreach (PlayerController playerController in playerControllers)
        {
            playerController.isAtk_Order = false;
            playerController.isDef_Order = true;
                        playerController.isFallBack_Order = false;

            Debug.Log("Defense Order!!!");
        }

        // Thêm tất cả các gameObject có tag "Player" vào FormationManager
        if (fM != null)
        {
            foreach (GameObject player in players)
            {
                fM.AddUnit(player);
                Debug.Log("Defense Formation!!!");
            }
            fM.Active_Def_Function();
            fM.ClearUnits();
        }
    }
}
