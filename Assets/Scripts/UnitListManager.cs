using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using UnityEngine;
using System;

// Serializable class to hold unit prefab and current order
using System;
using UnityEngine;

public class UnitListManager : MonoBehaviour
{
    public GameObject unitDef_Area;// là player battle cũ đổi tên cho sang // khu vực phòng thủ
    public List<string> selectedUnitTags;
    public UnitPanelFunction unitPanelFunction;
    public int unitCountAll;
    public int unitStrength;

    // Danh sách để lưu trữ các tag và danh sách tương ứng
    public List<TagList> unitTagLists = new List<TagList>();
    private PlayerCountDisplay playerCountDisplay;

    void Start()
    {
        if (unitPanelFunction == null)
        {
            FindUnitCountObject();
        }

        if (unitPanelFunction != null)
        {
            LoadSelectedUnits();

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
                //  AddUnitToTagList(player.name, player, player.GetComponent<PlayerController>().id);
            }
            foreach (string tag in selectedUnitTags)
            {
                // Tạo danh sách tương ứng với unitTag

                Create_DefUnitAreas(tag);
            }
        }

    }
    public void Caculate_UnitCountAll(bool flag)
    {
        unitCountAll += flag ? 1 : -1;
        playerCountDisplay.playerCount = unitCountAll;

    }

    void Update()
    {
        // Logic cập nhật cho các đơn vị nếu cần
    }

    private void FindUnitCountObject()
    {
        GameObject battleCanvas = GameObject.Find("BattleCanva");
        Transform unitCanvas = battleCanvas.transform.Find("UnitCanva");
        Transform panel = unitCanvas.Find("Panel");
        unitPanelFunction = panel.GetComponent<UnitPanelFunction>();
        panel = unitCanvas.Find("Population");
        panel = panel.Find("PlayerCount");
        playerCountDisplay = panel.GetComponent<PlayerCountDisplay>();
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
            }
        }
    }

    public void AddUnitToTagList(string unitTag, GameObject prefab, string create_id)
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
                currentOrder = currentOrder, // Gán currentOrder từ PlayerController
                id = create_id
            };

            // Thêm đối tượng vào danh sách units của TagList
            tagList.my_Units.Add(newUnit); // Thêm vào danh sách my_Units

            // Cập nhật số lượng đơn vị
            tagList.unitCount++;
            Caculate_UnitCountAll(true);


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
        //  newArea.GetComponent<FomationManager_NewUprade>().GetRenderAreaNew();
        newArea.GetComponent<FomationManager_NewUprade>().Create_defPosition();

        newArea.name = unitName + "_DefArea";
    }
    public void CreatDef(string unitName)
    {
        //tạo defposition cho unit đo
        Transform childArea = transform.Find(unitName + "_DefArea");
        childArea.gameObject.GetComponent<FomationManager_NewUprade>().Create_defPosition();
    }
    public bool RemoveUnitFromTagList(string unitTag, GameObject prefab, string id)
    {
        string prefabName = unitTag;
        int index = prefabName.IndexOf('(');
        if (index >= 0)
        {
            prefabName = prefabName.Substring(0, index).Trim(); // Cắt chuỗi từ đầu đến dấu (
        }

        // Tìm TagList có tagName tương ứng
        TagList tagList = unitTagLists.Find(tagList => tagList.tagName == prefabName);

        if (tagList != null)
        {
            UnitListOrder unitToRemove = tagList.my_Units.Find(unit => unit.id == id);

            if (unitToRemove != null)
            {
                // Xóa unit khỏi danh sách
                tagList.my_Units.Remove(unitToRemove);

                // Cập nhật số lượng đơn vị
                tagList.unitCount--;
                Caculate_UnitCountAll(false);

                // Tìm Transform của khu vực và tạo lại
                Transform childArea = transform.Find(prefabName + "_DefArea");
                if (childArea != null)
                {
                    childArea.gameObject.GetComponent<FomationManager_NewUprade>().Create_defPosition();
                }

                // Đợi cho đến khi UnitListOrder bị xóa thì mới tiến hành killSelf
                // Sử dụng Coroutine để delay việc gọi killSelf
                StartCoroutine(DelayedKillSelf2(prefab));

                return true;
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy unit '{prefab.name}' trong tag '{unitTag}'");
                // ko có id túc là


            }
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy TagList với '{unitTag}'");
        }


        return false;
    }

    // Coroutine để delay việc killSelf
    private IEnumerator DelayedKillSelf2(GameObject prefab)
    {
        // Kiểm tra xem prefab có phải là null không
        if (prefab == null)
        {
            Debug.LogWarning("Prefab là null, không thể gọi killSelf.");
            yield break; // Kết thúc coroutine nếu prefab là null
        }

        // Chờ một khoảng thời gian trước khi gọi killSelf
        yield return new WaitForEndOfFrame(); // Có thể thay đổi thành WaitForSeconds(0.5f) nếu muốn trì hoãn lâu hơn

        // Lấy component Health và gọi killSelf
        Health healthComponent = prefab.GetComponent<Health>();
        if (healthComponent != null)
        {
            healthComponent.killSelf();
        }
        else
        {
            Debug.LogWarning("Prefab không có component Health.");
        }
    }
    public List<UnitListOrder> FindUnitsByName(string prefabName)
    {
        // Cắt tên prefab đến dấu '('
        int index = prefabName.IndexOf('(');
        if (index >= 0)
        {
            prefabName = prefabName.Substring(0, index).Trim(); // Cắt chuỗi từ đầu đến dấu '('
        }

        List<UnitListOrder> foundUnits = new List<UnitListOrder>();
        // Duyệt qua tất cả các TagList

        TagList tagList = unitTagLists.Find(tagList => tagList.tagName == prefabName);
        foundUnits = tagList.my_Units;

        if (foundUnits.Count == 0)
        {
            Debug.LogWarning($"Không tìm thấy unit nào với tên '{prefabName}'");
        }
        else
        {
        }

        return foundUnits; // Trả về danh sách các unit tìm thấy
    }
    public byte Find_UnitCount(string prefabName)
    {
        // Cắt tên prefab đến dấu '('
        int index = prefabName.IndexOf('(');
        if (index >= 0)
        {
            prefabName = prefabName.Substring(0, index).Trim(); // Cắt chuỗi từ đầu đến dấu '('
        }
        TagList tagList = unitTagLists.Find(tagList => tagList.tagName == prefabName);
        return tagList.unitCount;
    }
}