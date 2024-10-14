using UnityEngine;

public class UnitClass : MonoBehaviour
{
    // Enum cho trọng lượng đơn vị

    // Thuộc tính để chọn trọng lượng
    [Header("Unit Settings")]
    public WeightUnit unitWeight;
    public WeightUnit extraDMGWeight;// kiểm tra weight của kẻ địch

    // Thuộc tính để chọn chủng tộc
    public RaceUnit unitRace;

    // Phương thức để hiển thị thông tin
    public void DisplayUnitInfo()
    {
        Debug.Log($"Unit Weight: {unitWeight}, Unit Race: {unitRace}");
    }
}