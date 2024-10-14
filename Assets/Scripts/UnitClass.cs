using UnityEngine;

public class UnitClass : MonoBehaviour
{
    // Enum cho trọng lượng đơn vị
    public enum Weight
    {
        Light,
        Heavy,
        Null
    }

    // Enum cho chủng tộc đơn vị
    public enum Race
    {
        Golem,
        OutWorld,
        Human,
        Undead
    }

    // Thuộc tính để chọn trọng lượng
    [Header("Unit Settings")]
    public Weight unitWeight;
    public Weight extraDMGWeight;// kiểm tra weight của kẻ địch

    // Thuộc tính để chọn chủng tộc
    public Race unitRace;

    // Phương thức để hiển thị thông tin
    public void DisplayUnitInfo()
    {
        Debug.Log($"Unit Weight: {unitWeight}, Unit Race: {unitRace}");
    }
}