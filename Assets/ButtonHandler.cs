using System;
using UnityEngine;
using UnityEngine.UI;

public class BasicAttack_Btn : MonoBehaviour
{
    public Button myButton; // Tham chiếu đến button

    public Attacks attacks; // Tham chiếu đến Attacks
    public GameObject chosen;
    public SelectPlayer_Btn selectPlayerBtn;



    void Start()
    {
        // Nếu myButton chưa được gán qua Inspector, tìm component Button trên chính GameObject này
        if (myButton == null)
        {
            myButton = GetComponent<Button>();
        }

        selectPlayerBtn.OnChosenPlayerSelect += HandlePlayerSelected;

        // setAttacks_Var();
        // Đảm bảo rằng attacks cũng được gán
        // if (attacks != null)
        // {
        //     //myButton.onClick.AddListener(OnButtonClick);
        //     Debug.Log("Có attacks");
        // }
        // else
        // {
        //     Debug.LogWarning("Biến attacks chưa được gán!", this);
        // }
    }

    private void HandlePlayerSelected()
    {
        PlayerController playerController = new PlayerController();
        chosen = playerController.Get_SelectPlayer();
        playerController = chosen.GetComponent<PlayerController>();
        playerController.isChosen = true;
        attacks = chosen.GetComponent<Attacks>();
    }

    public void BasicAttackActive()
    {
        if (attacks != null)
        {
            attacks.BasicAttackByBtn(); // Gọi hàm PerformAttack khi nhấn nút
        }
    }
    public void Abl1_Active()
    {
        if (attacks != null)
        {
            attacks.BasicAttackByBtn(); // Gọi hàm PerformAttack khi nhấn nút
        }
    }
    public void Abl2_Active()
    {
        if (attacks != null)
        {
            attacks.BasicAttackByBtn(); // Gọi hàm PerformAttack khi nhấn nút
        }
    }
    public void Abl3_Active()
    {
        if (attacks != null)
        {
            attacks.BasicAttackByBtn(); // Gọi hàm PerformAttack khi nhấn nút
        }
    }


    public void setAttacks_Var(GameObject chosenPlayer)
    {
        //GameObject chosenPlayer = PlayerController.playerHasBeenChosen;
        if (chosenPlayer != null)
        {
            attacks = chosenPlayer.GetComponent<Attacks>();

        }
        else
        {
            //("Ko có chosenPlayer");
        }
    }
}