using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Thêm thư viện để sử dụng List

public class Unit_Buttons_Panel : MonoBehaviour
{
    public GameObject buttonPrefab; // Prefab của Button
    public Transform buttonContainer; // Panel để chứa các button
    private List<Button> buttonList = new List<Button>(); // Danh sách để lưu trữ các Button
    

    // Hàm để tạo button mới
    public void CreateButton()
    {
        GameObject newButtonObject = Instantiate(buttonPrefab, buttonContainer); // Tạo button mới
        Button newButton = newButtonObject.GetComponent<Button>(); // Lấy component Button từ GameObject
        newButton.GetComponentInChildren<Text>().text = "Button " + (buttonContainer.childCount); // Đặt tên cho button

        buttonList.Add(newButton); // Thêm Button vào danh sách
    }

    // Hàm để xóa tất cả các button
    public void ClearButtons()
    {
        foreach (Button button in buttonList)
        {
            Destroy(button.gameObject); // Xóa GameObject của Button
        }
        buttonList.Clear(); // Xóa danh sách
    }

    // Hàm ví dụ để cập nhật tất cả các button
    public void UpdateAllButtons()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponentInChildren<Text>().text = "Updated Button";
        }
    }
}
