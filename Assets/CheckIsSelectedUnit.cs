using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CheckIsSelectedUnit : MonoBehaviour
{
    private string unitTagToRemove; // Tag của đơn vị cần xóa
    public CardUnitManager cum;

    void Start()
    {
        // Giả sử đối tượng cha có thành phần CardUnit
        CardUnit cardUnit = GetComponentInParent<CardUnit>();
        if (cardUnit != null)
        {
            unitTagToRemove = cardUnit.unitTag; // Gán unitTag từ đối tượng cha

        }
        else
        {
            Debug.LogWarning("Không tìm thấy CardUnit trong đối tượng cha.");
        }
      //  cum=new CardUnitManager();
    }

    public void OnButtonClick()
    {
        RemoveUnitTag(unitTagToRemove);
    }

    private void RemoveUnitTag(string unitTag)
    {
        //remove từ list là đc
        cum.removeSelectCard(unitTag);
        Destroy(transform.parent.gameObject);
    }
}