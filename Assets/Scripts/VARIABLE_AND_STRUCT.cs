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





