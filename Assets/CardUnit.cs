using UnityEngine;
using UnityEngine.UI;

public class CardUnit : MonoBehaviour
{
    public string unitTag; // Thẻ của đơn vị
    public CardUnitManager cardUnitManager; // Tham chiếu đến CardUnitManager
    private GameObject card_in4;
    //  public bool isSelection=false;
    void Start()
    {
        Button button = GetComponent<Button>();
        // button.onClick.AddListener(OnCardUnitClick);
        Find_Card_In4();
        //showOff_X_Button();

    }
    public void OnCardUnitClick()
    {
        // Gọi hàm chọn đơn vị và truyền chính đối tượng CardUnit
        cardUnitManager.SelectUnit(unitTag, gameObject);
        card_in4.GetComponent<Card_Detail>().SetUp_CardDetail(unitTag);

    }
    public void showOff_X_Button()
    {
        Transform child = transform.GetChild(0); // Lấy đối tượng con đầu tiên
        child.gameObject.SetActive(false); // Vô hiệu hóa đối tượng con
    }
    public void showOn_X_Button()
    {
        Transform child = transform.GetChild(0); // Lấy đối tượng con đầu tiên
        child.gameObject.SetActive(true); // Vô hiệu hóa đối tượng con
    }
    public void Find_Card_In4()
    {
        Transform parent = transform.parent;
        parent = parent.transform.parent;
        parent = parent.transform.parent;
        card_in4 = parent.Find("Card_In4").gameObject;
        Debug.Log("CardIn4 là:" + card_in4);
    }

}