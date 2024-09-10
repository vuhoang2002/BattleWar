using UnityEngine;

public class DefenseOrder : MonoBehaviour
{
    // Tham chiếu đến FormationManager
    private FormationManager fM;
    private FormationManagerUpgrade fmU;
    public bool isDef_Btn_Active=true;

    private void Start()
    {
        // Lấy tham chiếu đến FormationManager từ cùng một GameObject
         fmU=GetComponent<FormationManagerUpgrade>();
    }

    public void HandleButtonClick()
    {
       //FormationUnitByDefenseButton();
        haha();
        isDef_Btn_Active=true;
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
   
   private void haha() {
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    Debug.Log("Số lượng players tìm thấy: " + players.Length);

    // Kiểm tra tất cả game object trong scene
 
     foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player")) {
        Debug.Log("Tên GameObject Players: " + obj.name + ", Tag: " + obj.tag);
          fmU.AddUnit(obj);
        Debug.Log("Đã tìm thấy player: " + obj.name);
       

    }
     fmU.Active_Def_Function(true);


}
    public bool getDefOrder(){
        return isDef_Btn_Active;
    }
    
     public void  FormationUnitByDefenseButton(){
      //  fM = GetComponent<FormationManager>();
        fmU=GetComponent<FormationManagerUpgrade>();

        // Kiểm tra xem đối tượng FormationManager đã được tìm thấy chưa
        if (fM == null)
        {
            Debug.LogWarning("FormationManager component is not found on the same GameObject!");
        }
        // Tìm tất cả các PlayerController
      //  PlayerController[] playerControllers = GameObject.FindObjectsOfType<PlayerController>();
      //  GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Cập nhật isAtk_Order thành false và isDef_Order thành true cho tất cả các PlayerController
      //  foreach (PlayerController playerController in playerControllers)
       // {
       //     playerController.isAtk_Order = false;
       //     playerController.isDef_Order = true;
        //    playerController.isFallBack_Order = false;
//
       // //    Debug.Log("Defense Order!!!");
       // }

        // Thêm tất cả các gameObject có tag "Player" vào FormationManager
     //   if (fM != null)
      //  {
      //      foreach (GameObject player in players)
       //     {
       //         fM.AddUnit(player);
       //         Debug.Log("Defense Formation!!!");
       //     }
      //      fM.Active_Def_Function();
      //      fM.ClearUnits();
     //   }
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    Debug.Log("Số lượng players tìm thấy: " + players.Length);

    // Tiến hành xử lý nếu có player
    if (players.Length > 0) {
        foreach (GameObject player in players) {
            fmU.AddUnit(player);
        }
        fmU.Active_Def_Function(true);
    } else {
        Debug.LogWarning("Không tìm thấy player nào!");
    }
    }
}
