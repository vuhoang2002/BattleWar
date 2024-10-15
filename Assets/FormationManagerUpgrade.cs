using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManagerUpgrade : MonoBehaviour
{
    // Start is called before the first frame update

    [System.Serializable]
    public class UnitInfo
    {
        public string unitName; // Tên của unit
        public int count;   // Số lượng của unit

        public UnitInfo(string units_name, int unitCount)
        {
            unitName = units_name;
            count = unitCount;
        }
    }
    public GameObject playerBattleArea;
    public List<GameObject> units;  // chứa thông tin về số lượng unit có trong scence và unit đó
    public List<UnitInfo> unitInfo_List; // chứa thông tin về số lượng unit có trong scence và unit đó
    public int Max_Unit_Per_Col = 5; // 1 cột có tối đa 5 đơn vị
    public float space_X_Btw_Col = 1f; // khoảng cách giữa các cột với nhau
    public float space_X_In_Col = 0f; // khoảng cách theo chiều ngang giữa các đơn vị trong 1 cột( để nghiêng nhìn cho dễ)
    Vector2 position;
    public Vector2 maxPos;
    public Vector2 minPos;
    public List<Vector2> targetPositions;  // Danh sách các vị trí mục tiêu cho các đơn vị
    public List<int> unitPerCol; // cột 1 thì unitPerCol[1-1];
    public int totalColum = 0; // tính tư cột 1
    private float minY, maxY, lenghtUnits, toaDoX;

    Renderer renderer;

    void Start()
    {
        //     unitInfo_List = new List<UnitInfo>(); // Khởi tạo danh sách
        //  unitPerCol= new List<int>();
        if (units == null)
        {
            Debug.LogWarning("The 'units' list in FormationManager is not assigned.");
            return; // Ngừng thực hiện nếu units là null
        }
        else
        {
            // thêm vào unitInfo
        }
        if (playerBattleArea == null)
        {
            {
                GameObject playerBattleArea = GameObject.Find("PlayerBattleArea");
            }
            if (playerBattleArea == null)
            {
            }
        }

        targetPositions = new List<Vector2>();



    }

    public void Active_Def_Function(bool Def_Order)
    {
        //if(!Def_Order){
        //  return;// ngừng thực hiện nếu Def_Order là false
        //}
        // thực hiện
        ArrangeFormation();
        MoveUnitsToPosition();

    }
    private void ArrangeFormation()
    {

        if (playerBattleArea != null)
        {
            renderer = playerBattleArea.GetComponent<Renderer>();
            // Lấy tọa độ của bounds
            // Vector2 min = renderer.bounds.min; // Tọa độ thấp nhất
            //Vector2 max = renderer.bounds.max; // Tọa độ cao nhất                                 
            maxPos = renderer.bounds.max;
            minPos = renderer.bounds.min;

            minY = minPos.y;
            maxY = maxPos.y;
            toaDoX = maxPos.x;
            lenghtUnits = Mathf.Abs(maxY - minY);

        }
        targetPositions.Clear();  // Xóa danh sách cũ
        // sắp xếp bắt đầu

        //tính số lượng hàng dọc cần thiết cho việc sắp xếp( khi defend bình thường)
        for (int i = 1; i <= unitInfo_List.Count; i++)
        {// tối đa 6

            totalColum += unitInfo_List[i - 1].count / Max_Unit_Per_Col;
            for (int j = 1; j <= (unitInfo_List[i - 1].count / Max_Unit_Per_Col); j++)
            {
                // xét xem cột thứ i có bn phần từ
                // nếu chia hết thì cột có có Max phần tử
                unitPerCol.Add(Max_Unit_Per_Col);
            }
            if (unitInfo_List[i - 1].count % Max_Unit_Per_Col != 0)
            {
                totalColum++;// dư thì thêm cột
                unitPerCol.Add(unitInfo_List[i - 1].count % Max_Unit_Per_Col);
            }
        }

        float preX = toaDoX;
        float preY = maxY;
        // tạo các tọa độ

        for (int col = 1; col <= totalColum; col++)
        { //xét các cột
            // xem cột 1 có bao nhiêu unit
            float spaceBtwUnitIn_Col = lenghtUnits / (unitPerCol[col - 1] + 1);
            for (int i = 0; i < unitPerCol[col - 1]; i++)
            {// thiết lập tọa độ vị trị 
             //("Điểm thứ  "+ i);
                position = new Vector2(preX - space_X_In_Col, preY - spaceBtwUnitIn_Col * (i + 1));
                targetPositions.Add(position);
            }
            preX = preX - space_X_Btw_Col;
            preY = maxY;
        }



    }
    private void MoveUnitsToPosition()
    {
        int unitCount = Mathf.Min(units.Count, targetPositions.Count);

        for (int i = 0; i < unitCount; i++)
        {
            PlayerController pc = units[i].GetComponent<PlayerController>();
            pc.StartMovingToPosition(targetPositions[i]);
        }
    }
    private void UpdateUnitList_Info(bool add_Or_Del, string unitName)
    {
        bool flag = false;

        foreach (var info in unitInfo_List)
        {
            if (info.unitName == unitName)
            {
                flag = true;// unit đã có trong danh sách thì set là true
                info.count = add_Or_Del ? info.count + 1 : info.count - 1;
                return;
            }
        }
        // kiểm tra 1 lượt nếu thấy unitName chưa đc thêm vào=> chưa có trong danh sách   
        if (!flag)
        {//unitl chưa có trong danh sách(chưa có tức là phải add)
            UnitInfo newUnit = new UnitInfo(unitName, 1);
            unitInfo_List.Add(newUnit);
        }

    }
    private void UpdateUnitList()
    {

    }

    // thêm unit vào units
    public void AddUnit(GameObject unit)
    {
        if (unit != null && !units.Contains(unit))
        {
            units.Add(unit);
            UpdateUnitList_Info(true, unit.name);

            //  ArrangeFormation();  // Cập nhật lại formation sau khi thêm đơn vị
            //  ("Thêm unit thành công");
        }
    }
    // xóa unit khỏi units(khi units diea á)
    public void DeleteUnit(GameObject unit)
    {
        if (unit != null && !units.Contains(unit))
        {
            units.Remove(unit);
            UpdateUnitList_Info(false, unit.name);
            //  ArrangeFormation();  // Cập nhật lại formation sau khi xóa đơn vị

        }
    }

    public void ClearUnits()
    {
        units.Clear();
    }

}
