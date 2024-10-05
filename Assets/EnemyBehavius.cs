using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using UnityEngine;
using System;

public class EnemyBehavius : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab; // danh sách các loại enemy
        public bool isInCurrentMatch; // unit đó có ở trong trận đấu này hay không
    }

    public List<EnemyType> enemyType = new List<EnemyType>();
    public List<TagList> unitTagLists= new List<TagList>();
    // Trạng thái hành vi
    public bool atk_Order = true;
    public bool def_Order = false;
    public bool fallBack_Order = false;
    public bool hold_Order = false;

    // Thiết kế hành vi spawn quái và hành vi của tất cả enemy
    public GameObject enemySpawnArea;
    private float highest_Y;
    private float lowest_Y;
    private float positionSpawn_X;
    private int nextId = 1; // Khởi tạo ID
    public GameObject unitDef_Area;
    public GameObject enemyList;
   // public FomationManager_NewUprade fm_upgrade;
   // public List<TagList> EunitTagLists = new List<TagList>();

    void Start()
    {
        if (enemySpawnArea == null)
        {
            enemySpawnArea = transform.Find("EnemySpawnArea").gameObject;
        }

        if (enemySpawnArea != null)
        {
            SetSpawnLocation();
        }
        CreateTagLists();
        enemyList=Instantiate(enemyList);
    }

    private void CreateTagLists()
    {
        // Xóa danh sách cũ nếu có
        unitTagLists.Clear();

        foreach (var enemy in enemyType)
        {
            if (enemy.isInCurrentMatch)
            {
                string enemyName = enemy.enemyPrefab.name; // Lấy tên enemy từ prefab

                // Tạo hoặc tìm TagList với tagName là enemyName
                TagList tagList = unitTagLists.Find(tag => tag.tagName == enemyName);
                if (tagList == null)
                {
                    tagList = new TagList { tagName = enemyName };
                    unitTagLists.Add(tagList);
                    Create_DefUnitAreas(enemyName);
                   // GameObject newArea= Instantiate(enemyBattleArea);
                }

            }
        }
    }

   public void SpawnEnemy(string enemyName, int index)
{
    // Kiểm tra nếu tên enemy hợp lệ
    if (!string.IsNullOrEmpty(enemyName))
    {
        for (int i = 0; i < index; i++)
        {
            // Tính toán vị trí spawn Y ngẫu nhiên
            float positionSpawn_Y = UnityEngine.Random.Range(lowest_Y, highest_Y);
            Vector3 spawnPosition = new Vector3(positionSpawn_X, positionSpawn_Y, 0f);
            
            // Spawn prefab enemy
            GameObject enemyPrefab = GetEnemyPrefabByName(enemyName);
            if (enemyPrefab != null)
            {
                GameObject newUnit = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // Spawn prefab
                newUnit.transform.SetParent(enemyList.transform);
                // Gán ID cho đơn vị mới
                string creat_ID_For_Unit = "E" + nextId; // Tạo ID với chữ "E" trước
                nextId++; // Tăng ID cho lần tạo tiếp theo

                // Nếu cần, có thể gán ID cho đối tượng mới
                EnemyController enemyController = newUnit.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.id = creat_ID_For_Unit; // Gán ID cho enemy
                }

                // Thêm đơn vị vào danh sách của TagList tương ứng
                AddUnitToTagList(enemyName, enemyController, newUnit); // Gọi hàm để thêm đơn vị vào danh sách
            }
            else
            {
                Debug.LogWarning($"No prefab found for enemy name: {enemyName}");
            }
        }
       // Create_defPosition();
       CreatDef(enemyName);
    }
    else
    {
        Debug.LogWarning("Enemy name is null or empty.");
    }
}   
   public void SpawnEnemy(int enemyIndex, int count)
{
    // Kiểm tra chỉ số enemyIndex hợp lệ
    if (enemyIndex < 0 || enemyIndex >= enemyType.Count)
    {
        Debug.LogWarning("Enemy index out of range.");
        return ; // Trả về null nếu chỉ số không hợp lệ
    }

    // Lấy tên prefab của enemy
    EnemyType enemy = enemyType[enemyIndex];
    string enemyName;
    if (enemy.isInCurrentMatch)
    {
        enemyName= enemy.enemyPrefab.name; // Trả về tên prefab
        SpawnEnemy(enemyName, count);
    }
    
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
    Debug.Log("myUnits is" +myUnits);
    newArea.GetComponent<FomationManager_NewUprade>().SetUnitForArea(myUnits);
  //  newArea.GetComponent<FomationManager_NewUprade>().GetRenderAreaNew();
     newArea.GetComponent<FomationManager_NewUprade>().Create_defPosition();

    newArea.name = unitName + "_DefArea";
}
    public void CreatDef(string unitName ){
        //tạo defposition cho unit đo
        Transform childArea = transform.Find(unitName+"_DefArea");
        childArea.gameObject.GetComponent<FomationManager_NewUprade>().Create_defPosition();
    }
    private GameObject GetEnemyPrefabByName(string enemyName)
    {
        foreach (var enemy in enemyType)
        {
            if (enemy.enemyPrefab.name.Equals(enemyName, StringComparison.OrdinalIgnoreCase) && enemy.isInCurrentMatch)
            {
                return enemy.enemyPrefab;
            }
        }
        return null; // Không tìm thấy prefab
    }

    public void SetAllEnemyBehavius()
    {
        // Lấy tất cả enemy trong trận hiện tại và thiết lập hành vi cho chúng
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            EnemyController ec = enemy.GetComponent<EnemyController>();
            if (ec != null)
            {
                ec.SetBehavius(atk_Order, def_Order, fallBack_Order);
                // hazz, xẻm ra phải tạo holdPosition ở đây
                if(hold_Order){
                    ec.hold_Position=ec.transform.position;
                }

            }
        }
    }
    public bool RemoveUnitFromTagList(string unitTag, GameObject prefab, string id)
{   
    string prefabName = unitTag;
    int index = prefabName.IndexOf('(');
    if (index >= 0)
    {
        prefabName = prefabName.Substring(0, index).Trim(); // Cắt chuỗi từ đầu đến dấu (
    }
    Debug.Log("Unit sắp xóa tên: " + prefabName);

    // Tìm TagList có tagName tương ứng
    TagList tagList = unitTagLists.Find(tagList => tagList.tagName == prefabName);

    if (tagList != null)
    {
        UnitListOrder unitToRemove = tagList.my_Units.Find(unit => unit.id == id);
        
        if (unitToRemove != null)
        {
            // Xóa unit khỏi danh sách
            tagList.my_Units.Remove(unitToRemove);
            Debug.Log($"Đã xóa unit '{prefab.name}' khỏi tag '{unitTag}'");

            // Cập nhật số lượng đơn vị
            tagList.unitCount--;

            // Tìm Transform của khu vực và tạo lại
            Transform childArea = transform.Find(prefabName + "_DefArea");
            if (childArea != null)
            {
                childArea.gameObject.GetComponent<FomationManager_NewUprade>().Create_defPosition();
            }

            // Đợi cho đến khi UnitListOrder bị xóa thì mới tiến hành killSelf
            // Sử dụng Coroutine để delay việc gọi killSelf
            StartCoroutine(DelayedKillSelf(prefab));

            return true;
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy unit '{prefab.name}' trong tag '{unitTag}'");
        }
    }
    else
    {
        Debug.LogWarning($"Không tìm thấy TagList với '{unitTag}'");
    }
    
    return false;
}

// Coroutine để delay việc killSelf
private IEnumerator DelayedKillSelf(GameObject prefab)
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
    private void SetSpawnLocation()
    {
        Renderer renderer = enemySpawnArea.GetComponent<Renderer>();
        Vector3 min = renderer.bounds.min; // Tọa độ thấp nhất
        Vector3 max = renderer.bounds.max; // Tọa độ cao nhất
        highest_Y = max.y; // Lấy tọa độ Y cao nhất
        lowest_Y = min.y; // Lấy tọa độ Y thấp nhất
        positionSpawn_X = min.x; // Lấy tọa độ X thấp nhất
    }
    private void AddUnitToTagList(string enemyName, EnemyController enemyController, GameObject enemyPrefab)
{
    TagList tagList = unitTagLists.Find(tag => tag.tagName == enemyName);
    if (tagList != null)
    {
        UnitListOrder unitOrder = new UnitListOrder
        {
            // Giả sử bạn có thông tin cần thiết để gán cho đơn vị
            // Ví dụ: id hoặc các thuộc tính khác từ enemyController
            prefab=enemyPrefab,  // Cho phép gán trong Inspector
            currentOrder="Spawn",
            id = enemyController.id // Gán ID cho UnitListOrder
        };
        tagList.my_Units.Add(unitOrder); // Thêm đơn vị vào danh sách
        tagList.unitCount++; // Cập nhật số lượng đơn vị
    }
}

    public void Set_eAtk()
    {
        atk_Order = true;
        def_Order = false;
        fallBack_Order = false;
        hold_Order = false;
    }

    public void Set_eDef()
    {
        atk_Order = false;
        def_Order = true;
        fallBack_Order = false;
        hold_Order = false;
    }

    public void Set_eRetreat()
    {
        atk_Order = false;
        def_Order = false;
        fallBack_Order = true;
        hold_Order = false;
    }

    public void Set_eHold()
    {
        atk_Order = false;
        def_Order = false;
        fallBack_Order = false;
        hold_Order = true;
        // tạo hold_Position nữa
    }
}