using UnityEngine;
using UnityEngine.UI;

public class FunctionPanel : MonoBehaviour
{
  public Button myButtonBasic;
  public Button myButtonBasic1;
  public Button myButtonBasic2;
  public Button myButtonBasic3; // Tham chiếu đến button

  public Attacks attacks; // Tham chiếu đến Attacks
  public GameObject chosen;

  private void Start()
  {
    // Đăng ký sự kiện cho các nút bấm
    myButtonBasic.onClick.AddListener(OnBasicButtonClick);
    myButtonBasic1.onClick.AddListener(OnAbility1ButtonClick);
    myButtonBasic2.onClick.AddListener(OnAbility2ButtonClick);
    myButtonBasic3.onClick.AddListener(OnAbility3ButtonClick);
  }

  private void OnBasicButtonClick()
  {
    Debug.Log("Basic Attack Button clicked!");
    if (attacks != null)
    {
      attacks.BasicAttackByBtn(); // Gọi hàm PerformAttack khi nhấn nút
      //Debug.Log("nút basic được nhấn");
    }
  }

  private void OnAbility1ButtonClick()
  {
    //Debug.Log("Ability 1 Button clicked!");
    if (attacks != null)
    {
      // attacks.Abl1_Active(); // Gọi hàm liên quan đến Ability 1
      attacks.Abl1_AttackBtn();
    }
  }

  private void OnAbility2ButtonClick()
  {
    //Debug.Log("Ability 2 Button clicked!");
    if (attacks != null)
    {
      attacks.Abl2_AttackByBTn();
    }
  }

  private void OnAbility3ButtonClick()
  {
    //Debug.Log("Ability 3 Button clicked!");
    if (attacks != null)
    {
      attacks.Abl3_AttackByBTn();
    }
  }
}