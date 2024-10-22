using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Start is called before the first frame update
public enum WeightUnit
{// loại
    Light,
    Heavy,
    Rock,
    Null
}

// Enum cho chủng tộc đơn vị
public enum RaceUnit
{
    Golem,
    OutWorld,
    Human,
    Undead
}
public enum AbilityUnit
{
    Zero,
    One,
    Two,
    Three
}
public enum CardType
{
    Unit,
    Spell,
    Enhancement
}
public enum GameMod
{
    Classic,
    Defend,
    Attack,
    War,
    Survival
}
public enum UnitOrder
{
    Attack,
    Defend,
    Retreat,
    Hold,
    Null
}
//dành cho việc sắp xếp
public class UnitListOrder
{
    public string id; // Thêm trường ID
    public GameObject prefab; // Cho phép gán trong Inspector
    public string currentOrder; // Hành vi hiện tại của unit đó

    // Constructor để khởi tạo ID
    public void Set_Id(string id)
    {
        this.id = id;
    }
}
public class TagList
{
    public string tagName; // Tên tag cho danh sách
    public List<UnitListOrder> my_Units = new List<UnitListOrder>(); // Danh sách các đơn vị tương ứng
    public int unitCount; // Số lượng đơn vị
}
public class Player_ListObject
{
    public List<PlayerController> FindAllPlayerInList(GameObject gameObject)
    {
        List<PlayerController> playerControllers = new List<PlayerController>();
        playerControllers.Clear();

        foreach (Transform child in gameObject.transform)
        {
            try
            {
                PlayerController playerController = child.gameObject.GetComponent<PlayerController>();
                if (playerController != null) // Kiểm tra nếu playerController không null
                {
                    playerControllers.Add(playerController); // Thêm playerController vào danh sách
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Lỗi khi xử lý GameObject: " + child.gameObject.name + ". Lỗi: " + ex.Message);
                // Sa ca lay túc tiệp túc tiệp
            }
        }
        return playerControllers;
    }
}





