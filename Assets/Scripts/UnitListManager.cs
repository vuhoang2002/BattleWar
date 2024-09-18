using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;

// Serializable class to hold unit prefab and current order
[Serializable]
public class UnitListOrder
{
    public GameObject prefab; // Cho phép gán trong Inspector
    public string currentOrder; // Hành vi hiện tại của unit đó
}

[Serializable]
public class TagList
{
    public string tagName; // Tên tag cho danh sách
    public List<UnitListOrder> my_Units = new List<UnitListOrder>(); // Danh sách các đơn vị tương ứng
    public int unitCount; // Số lượng đơn vị
}

public class UnitListManager : MonoBehaviour
{
    public List<string> selectedUnitTags;
    public UnitPanelFunction unitPanelFunction;

    // Danh sách để lưu trữ các tag và danh sách tương ứng
    public List<TagList> unitTagLists = new List<TagList>();

    void Start()
    {
        if (unitPanelFunction == null)
        {
            FindUnitCountObject();
        }

        if (unitPanelFunction != null)
        {
            LoadSelectedUnits();

            Debug.Log("tag là: " + string.Join(", ", selectedUnitTags));
            foreach (string tag in selectedUnitTags)
            {
                // Tạo danh sách tương ứng với unitTag
                TagList newTagList = new TagList
                {
                    tagName = tag // Gán tên tag cho danh sách
                };
                unitTagLists.Add(newTagList);
            }

            // Tìm tất cả các GameObject có tag "Player"
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                // Thêm object vào danh sách
                AddUnitToTagList(player.name, player);
            }
        }
    }

    void Update()
    {
        // Logic cập nhật cho các đơn vị nếu cần
    }

    private void FindUnitCountObject()
    {
        GameObject battleCanvas = GameObject.Find("BattleCanvas");
        Transform unitCanvas = battleCanvas.transform.Find("UnitCanvas");
        Transform panel = unitCanvas.Find("Panel");
        unitPanelFunction = panel.GetComponent<UnitPanelFunction>();
    }

    public void LoadSelectedUnits()
    {
        string path = Application.persistentDataPath + "/savedata.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                SaveData data = formatter.Deserialize(stream) as SaveData;
                selectedUnitTags = data.selectedUnitTags; // Khôi phục danh sách thẻ đơn vị đã chọn
                Debug.Log("Đã tải các đơn vị đã chọn: " + string.Join(", ", selectedUnitTags));
            }
        }
    }

    public void AddUnitToTagList(string unitTag, GameObject prefab)
    {
        // Tìm TagList có tagName tương ứng
        TagList tagList = unitTagLists.Find(tagList => tagList.tagName == unitTag);

        if (tagList != null)
        {
            // Lấy currentOrder từ PlayerController
            string currentOrder = prefab.GetComponent<PlayerController>().Get_Current_Order_toString();

            // Tạo một đối tượng UnitListOrder mới
            UnitListOrder newUnit = new UnitListOrder
            {
                prefab = prefab,
                currentOrder = currentOrder // Gán currentOrder từ PlayerController
            };

            // Thêm đối tượng vào danh sách units của TagList
            tagList.my_Units.Add(newUnit); // Thêm vào danh sách my_Units

            // Cập nhật số lượng đơn vị
            tagList.unitCount++;

            Debug.Log($"Đã thêm unit vào tag '{unitTag}': {prefab.name} với order: {currentOrder}");
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy TagList với tagName '{unitTag}'");
        }
    }
}