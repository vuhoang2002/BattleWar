using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Thêm thư viện để sử dụng IPointerClickHandler
using TMPro;

public class GoldCount : MonoBehaviour, IPointerClickHandler
{
    public Button gold_Button;
    public int currentGold = 0;
    public int MAX_GOLD;
    public float time_add_gold = 2f;
    private float time_add_gold_TIMER = 0;
    public int gold_value = 5;
    public int MAX_VALUE_GOLD = 10; // Giá trị tăng thêm cho MAX_GOLD

    void Start()
    {
        if (gold_Button == null)
        {
            Debug.LogError("Button không được gán! Hãy kiểm tra lại trong Inspector.");
        }
    }

 void FixedUpdate()
{
   if (currentGold > MAX_GOLD)
        {
            currentGold = MAX_GOLD;
        }

        if (gold_Button != null)
        {
            TextMeshProUGUI buttonText = gold_Button.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                buttonText.text = currentGold.ToString();
            }
            else
            {
                Debug.LogError("Không tìm thấy thành phần TextMeshProUGUI trên Button.");
            }
        }

        if (currentGold < MAX_GOLD)
        {
            if (time_add_gold_TIMER >= time_add_gold)
            {
                currentGold += gold_value;
                time_add_gold_TIMER = 0;
            }
        }

        time_add_gold_TIMER += Time.deltaTime;   
}
    void Update()
    {
      
    }

    // Xử lý sự kiện khi người dùng chạm vào Button
    public void OnPointerClick(PointerEventData eventData)
    {Debug.Log("Nút đc nhấn");
        if (currentGold == MAX_GOLD)
        {
            currentGold = 0;
            MAX_GOLD += MAX_VALUE_GOLD;
        }
    }
}
