using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button myButton; // Tham chiếu đến button

    public Attacks attacks; // Tham chiếu đến Attacks

    void Start()
    {
        // Nếu myButton chưa được gán qua Inspector, tìm component Button trên chính GameObject này
        if (myButton == null)
        {
            myButton = GetComponent<Button>();
        }



        // setAttacks_Var();
        // Đảm bảo rằng attacks cũng được gán
        if (attacks != null)
        {
            myButton.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogWarning("Biến attacks chưa được gán!", this);
        }
    }

    void OnButtonClick()
    {
        if (attacks != null)
        {
            attacks.AttackByButton(); // Gọi hàm PerformAttack khi nhấn nút
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
            Debug.Log("Ko có chosenPlayer");
        }
    }
}