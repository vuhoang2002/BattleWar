using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// Start is called before the first frame update


public class VARIABLE_AND_STRUCT : MonoBehaviour
{
}
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
public enum AbilityCount
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
public enum SpeardDamage
{
    NormalAttack,
    HaveCount,
    Ability

}

//dành cho việc sắp xếp
[System.Serializable]
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

[System.Serializable]
public class TagList
{
    public string tagName; // Tên tag cho danh sách
    public List<UnitListOrder> my_Units = new List<UnitListOrder>(); // Danh sách các đơn vị tương ứng
    public byte unitCount; // Số lượng đơn vị
}
[System.Serializable]
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
[System.Serializable]
public class SpawnButtonInfo
{
    public Vector2 position; // Tọa độ
    public GameObject button; // Tham chiếu đến nút

    public SpawnButtonInfo(GameObject btn, Vector2 pos)
    {
        position = pos;
        button = btn;
    }
    public void setPostionButton(Vector2 postion2)
    {
        this.position = postion2;
    }

    public static implicit operator Vector2(SpawnButtonInfo v)
    {
        throw new NotImplementedException();
    }
}





