using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public string unitTag; // Thẻ để phân biệt các đơn vị
    public GameObject prefab; // Prefab của đơn vị
    public Sprite prefabSprite;
    public int unitPrice;//giá tiền
    public float cdTimerUnit;//tg hồi
    public int unitStrength;
    public CardType isUnit;// là unit hay phép bổ trợ
}

[System.Serializable]
public class SaveData
{
    public List<string> selectedUnitTags; // Danh sách các thẻ đơn vị đã chọn
}

public class CardUnitManager : MonoBehaviour
{
    public List<UnitData> unitDataList; // Danh sách các đơn vị lính
    public GameObject selectedPanel;
    public GameObject cardArea;
    public List<string> selectedUnitTags = new List<string>(); // Lưu trữ các thẻ đơn vị đã chọn
    public SelectCard selectCard;

    private void Start()
    {
        Time.timeScale = 1f;
        LoadSelectedUnits(); // Tải các đơn vị đã chọn khi bắt đầu
        LoadSavedUnits();
    }

    private void Update()
    {
        //ShowSelectedUnits();
    }

    public void SelectUnit(string unitTag, GameObject cardPrefab)
    {
        if (!selectCard.CheckCountOfCard_IsMax())
        {
            // Kiểm tra xem thẻ đã được chọn hay chưa
            if (!selectedUnitTags.Contains(unitTag))
            {
                selectedUnitTags.Add(unitTag);
                GameObject selectedUnit = Instantiate(cardPrefab, selectedPanel.transform);
                selectedUnit.GetComponent<CardUnit>().showOn_X_Button();
                SaveSelectedUnits();
            }
        }
        else
        {
            Debug.LogError("Đã có đủ số lượng card tối đa");
        }
    }

    public void LoadSavedUnits()
    {
        string path = Application.persistentDataPath + "/savedata.dat"; // Đường dẫn đến file lưu

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                SaveData data = formatter.Deserialize(stream) as SaveData;

                if (data != null)
                {
                    // Cập nhật selectedUnitTags với các thẻ đã lưu
                    selectedUnitTags = new List<string>(data.selectedUnitTags);

                    foreach (string unitTag in selectedUnitTags)
                    {
                        // Tạo bản sao của mỗi đơn vị và thêm vào SelectedPanel
                        CardUnit unitPrefab = FindMatchingCardUnits(unitTag);

                        if (unitPrefab != null) // Kiểm tra xem có tìm thấy prefab không
                        {
                            CardUnit selectedUnit = Instantiate(unitPrefab, selectedPanel.transform);
                            selectedUnit.name = unitTag; // Đặt tên cho đối tượng
                            selectedUnit.showOn_X_Button(); // Hiện nút X
                        }
                        else
                        {
                            //Debug.LogWarning("Không tìm thấy CardUnit với unitTag: " + unitTag);
                        }
                    }
                }
            }
            //Debug.Log("Đã tải dữ liệu từ file.");
        }
        else
        {
            //Debug.Log("Không tìm thấy file lưu.");
        }
    }

    public CardUnit FindMatchingCardUnits(string currentUnitTag)
    {
        // Tìm CardArea trong Canvas

        // Lấy tất cả các CardUnit trong CardArea

        CardUnit[] cardUnits = cardArea.GetComponentsInChildren<CardUnit>();

        // Kiểm tra và in ra các unitTag trùng khớp
        foreach (CardUnit cardUnit in cardUnits)
        {
            if (cardUnit.unitTag == currentUnitTag)
            {
                //Debug.Log("Tìm thấy CardUnit trùng khớp với unitTag: " + currentUnitTag);
                // Thực hiện hành động với cardUnit tìm thấy
                return cardUnit;
            }
        }
        return null;
    }

    private void LoadSelectedUnits()
    {
        string path = Application.persistentDataPath + "/savedata.dat";
        //Debug.Log("Path is:" + path);
        //     //Debug.Log("Dữ liệu được lưu ở: " + path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            selectedUnitTags = data.selectedUnitTags; // Khôi phục danh sách thẻ đơn vị đã chọn
            //Debug.Log("Đã tải các đơn vị đã chọn: " + string.Join(", ", selectedUnitTags));
        }
    }

    public void SaveSelectedUnits()
    {
        SaveData data = new SaveData();
        data.selectedUnitTags = selectedUnitTags; // Lưu danh sách thẻ đơn vị đã chọn

        string path = Application.persistentDataPath + "/savedata.dat";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();

        //Debug.Log("Đã lưu các đơn vị đã chọn.");
    }

    private void OnApplicationQuit()
    {
        SaveSelectedUnits(); // Lưu khi ứng dụng thoát
    }

    public void ClearSelection()
    {
        selectedUnitTags.Clear();
        SaveSelectedUnits();
        //Debug.Log("Đã xóa tất cả lựa chọn đơn vị.");
    }

    public void ShowSelectedUnits()
    {
        if (selectedUnitTags.Count > 0)
        {
            string selectedUnits = string.Join(", ", selectedUnitTags);
            //  Debug.Log("Danh sách các đơn vị đã chọn: " + selectedUnits);
        }
        else
        {
            Debug.Log("Không có đơn vị nào được chọn.");
        }
    }

    public void removeSelectCard(string unitName)
    {
        if (selectedUnitTags.Contains(unitName))
        {
            selectedUnitTags.Remove(unitName);
            SaveSelectedUnits();

            //   Debug.Log($"Đã xóa '{unitName}' khỏi danh sách các đơn vị đã chọn.");
        }
        else
        {
            // Debug.LogWarning($"'{unitName}' không có trong danh sách các đơn vị đã chọn.");
        }
    }
}