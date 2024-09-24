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
    public GameObject unitDef_Area;// là player battle cũ đổi tên cho sang // khu vực phòng thủ
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
              //  Create_DefUnitAreas(tag);
            }

            // Tìm tất cả các GameObject có tag "Player"
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                // Thêm object vào danh sách
                AddUnitToTagList(player.name, player);
            }
             foreach (string tag in selectedUnitTags)
            {
                // Tạo danh sách tương ứng với unitTag
               
                Create_DefUnitAreas(tag);
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
        //chỉnh sửa position
    }
public void Create_DefUnitAreas(string unitName)
{
    // Tìm TagList với tagName tương ứng
    TagList tagList = unitTagLists.Find(tagList => tagList.tagName == unitName);
        // Lấy danh sách my_Units
        List<UnitListOrder> myUnits = tagList.my_Units;

        // Thực hiện các thao tác với myUnits nếu cần
    // Tiếp tục với việc tạo khu vực phòng thủ
    float totalLength = unitDef_Area.GetComponent<Renderer>().bounds.size.x; // Lấy chiều dài theo trục x

    GameObject newArea = Instantiate(unitDef_Area, gameObject.transform);
    newArea.SetActive(true);
    
    // Truyền các unit vào các area con nếu cần
    newArea.GetComponent<FomationManager_NewUprade>().SetUnitForArea(myUnits);
     newArea.GetComponent<FomationManager_NewUprade>().Create_defPosition();

    newArea.name = unitName + "_DefArea";
}
    public void CreatDef(string unitName ){
        //tạo defposition cho unit đo
        Transform childArea = transform.Find(unitName+"_DefArea");
        childArea.gameObject.GetComponent<FomationManager_NewUprade>().Create_defPosition();
    }
public bool RemoveUnitFromTagList(string unitTag, GameObject prefab)
{   
      string prefabName = unitTag;
        int index = prefabName.IndexOf('(');
        if (index >= 0)
        {
            prefabName = prefabName.Substring(0, index).Trim(); // Cắt chuỗi từ đầu đến dấu (
        }
        Debug.Log("Unit sắp xóa tên"+prefabName);
    // Tìm TagList có tagName tương ứng
    TagList tagList = unitTagLists.Find(tagList => tagList.tagName == prefabName);

    if (tagList != null)
    {
        // Lấy tên prefab mà không có dấu (
      

        // Tìm UnitListOrder có prefab tương ứng
        UnitListOrder unitToRemove = tagList.my_Units.Find(unit => 
        {
           prefab = unit.prefab;
           
            return prefab; // So sánh với tên đã xử lý
        });

        if (unitToRemove != null)
        {
            // Xóa unit khỏi danh sách
            tagList.my_Units.Remove(unitToRemove);
             Transform childArea = transform.Find(prefabName+"_DefArea");
             //tạo lại
            childArea.gameObject.GetComponent<FomationManager_NewUprade>().Create_defPosition();


            // Cập nhật số lượng đơn vị
            tagList.unitCount--;

            // Nếu cần, có thể hủy GameObject của unit trong scene
           unitToRemove.prefab.GetComponent<Health>().killSelf();


            Debug.Log($"Đã xóa unit '{prefab.name}' khỏi tag '{unitTag}'");
            return true;
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy unit '{prefab.name}' trong tag '{unitTag}'");
        }
    }
    else
    {
        Debug.LogWarning($"Không tìm thấy TagList với    '{unitTag}'");
    }
    // chỉnh sửa
    return false;
}
}