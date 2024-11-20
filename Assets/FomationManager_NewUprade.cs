using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FomationManager_NewUprade : MonoBehaviour
{
    // Start is called before the first frame update

    public List<UnitListOrder> unit = new List<UnitListOrder>();

    private int Max_Unit_Per_Col = 5; // 1 cột có tối đa 5 đơn vị
    public float space_X_Btw_Col = 1f; // khoảng cách giữa các cột với nhau
    private float space_X_Btw_Unit = 1.2f;
    public float space_X_In_Col = 0.2f; // khoảng cách theo chiều ngang giữa các đơn vị trong 1 cột( để nghiêng nhìn cho dễ)
    Vector2 position;
    public Vector2 maxPos;
    public Vector2 minPos;
    public List<Vector2> targetPositions;  // Danh sách các vị trí mục tiêu cho các đơn vị
    public List<int> unitPerCol; // cột 1 thì unitPerCol[1-1];
    public int totalColum = 0; // tính tư cột 1
    private int private_TotalColum = 0;
    private float minY, maxY, lenghtUnits;
    public float toaDoX;
    Renderer renderer;
    public bool thisIsEnemy = false;


    void Start()
    {
        GetRenderArea();
        if (thisIsEnemy)
        {
            changeVaule_Col();//đổi dấu  - sang +
        }
    }
    public void SetUnitForArea(List<UnitListOrder> unit)
    {
        this.unit = unit;
    }

    // Update is called once per frame
    void GetRenderArea()
    {// lấy các tọa độ cần thiết cho việc sắp xếp
        renderer = GetComponent<Renderer>();
        // Lấy tọa độ của bounds
        // Vector2 min = renderer.bounds.min; // Tọa độ thấp nhất
        //Vector2 max = renderer.bounds.max; // Tọa độ cao nhất                                 
        maxPos = renderer.bounds.max;
        minPos = renderer.bounds.min;
        minY = minPos.y;
        maxY = maxPos.y;
        toaDoX = maxPos.x;// điểm spawns
        lenghtUnits = Mathf.Abs(maxY - minY);
    }
    //cách mới: thực chất cách này phải có tham số truyền vào tọa độ owrr bên UnitListManager
    public void GetRenderAreaNew()
    {
        // cái này ko xài nx
        //toaDoX= parentLocation.position.x;
        // int colBeforeTHis= Col_Counter_Before_This();
        //  toaDoX-=(space_X_Btw_Col*colBeforeTHis);
    }
    public void Create_defPosition()
    {// tạo các tọa đố 
        Col_Counter_Before_This();// lấy tọa độ X mới
        targetPositions.Clear();  // Xóa danh sách cũ
        unitPerCol.Clear();
        totalColum = 0;
        // sắp xếp bắt đầu
        //tính số lượng hàng dọc cần thiết cho việc sắp xếp
        totalColum = unit.Count / Max_Unit_Per_Col;
        for (int i = 1; i <= totalColum; i++)
        {
            unitPerCol.Add(Max_Unit_Per_Col);
        }
        if (unit.Count % Max_Unit_Per_Col != 0)
        {
            totalColum++;
            unitPerCol.Add(unit.Count % Max_Unit_Per_Col);
        }
        if (totalColum == 0)
        {
            //  return;
        }

        float preX = toaDoX;
        float preY = maxY;
        // tạo các tọa độ
        for (int col = 0; col < totalColum; col++)
        { //xét các cột
          //    (preX+","+ preY+","+ minY+"lần thứ "+col);
          // xem cột 1 có bao nhiêu unit
            float spaceBtwUnitIn_Col = lenghtUnits / (unitPerCol[col] + 1); // khoảng cách giữa các đơn vị trong 1 cột
            for (int i = 0; i < unitPerCol[col]; i++)
            {// thiết lập tọa độ vị trị 

                position = new Vector2(preX - space_X_In_Col * i, preY - spaceBtwUnitIn_Col * (i + 1));
                // (preX space_X_In_Col+"amen"+position);
                int unitIndex = i + (col) * Max_Unit_Per_Col; // Tính chỉ số của unit

                if (unitIndex < unit.Count && unit[unitIndex].prefab.CompareTag("Player")) // Kiểm tra chỉ số
                {
                    unit[unitIndex].prefab.GetComponent<PlayerController>().Set_Def_Position(position);
                    unit[unitIndex].currentOrder = "def" + position; // Cập nhật currentOrder
                }
                else if (unitIndex < unit.Count && unit[unitIndex].prefab.CompareTag("Enemy"))
                {
                    unit[unitIndex].prefab.GetComponent<PlayerController>().Set_Def_Position(position);
                    unit[unitIndex].currentOrder = "def" + position; // Cập nhật currentOrder
                }
                // ("Thêm tọa độ vào điểm thứ "+i+" cột"+col+"tọa độ"+position);
                targetPositions.Add(position);
            }
            preX = preX - space_X_Btw_Col;
            preY = maxY;

        }
        //hazz, tính lại tọa độ các cột sau nếu colum có sự thay đổi
        if (private_TotalColum != totalColum)
        {
            StartCoroutine(UpdateFormation_For_LOWER_Unit());
        }
        //hazz, tính lại tọa độ các cột sau nếu colum có sự thay đổi
        private_TotalColum = totalColum;
    }
    public IEnumerator UpdateFormation_For_LOWER_Unit()
    {
        Transform parent = transform.parent;
        int numberOfChildren = parent.childCount;
        int Index = transform.GetSiblingIndex();
        yield return new WaitForSeconds(1f);
        for (int i = Index + 1; i < numberOfChildren; i++)
        {
            //i+1 tránh làm cho object hiện tại vòng lặp vô cmn hạn;
            Transform child = parent.GetChild(i);
            child.GetComponent<FomationManager_NewUprade>().Create_defPosition(); ;
        }
    }
    public void showColum()
    {
    }
    private int Col_Counter_Before_This()
    {
        // Lấy GameObject cha
        Transform parent = transform.parent;
        int col = 0;
        if (parent != null)
        {
            int currentIndex = transform.GetSiblingIndex();

            // Tạo vòng lặp để tìm kiếm component trong các GameObject con trước GameObject hiện tại
            for (int i = 0; i < currentIndex; i++)
            {
                Transform child = parent.GetChild(i);
                // Tìm kiếm một component cụ thể (ví dụ: Collider)

                col += child.GetComponent<FomationManager_NewUprade>().totalColum;


            }
        }
        else
        {
        }
        toaDoX = parent.position.x;

        toaDoX -= (space_X_Btw_Col * col);
        return col;

    }

    private void changeVaule_Col()
    {
        //đổi dấu các giá trị Btw_Col và In_Col
        // space_X_Btw_Unit=-space_X_Btw_Unit; // cái này ko dùng
        space_X_Btw_Col = -space_X_Btw_Col;
        // space_X_In_Col=-space_X_In_Col;
    }

}
