using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FomationManager_NewUprade : MonoBehaviour
{
    // Start is called before the first frame update

    public List<UnitListOrder> unit = new List<UnitListOrder>();
    
    public int Max_Unit_Per_Col=5; // 1 cột có tối đa 5 đơn vị
    public float space_X_Btw_Col=1f; // khoảng cách giữa các cột với nhau
    public float space_X_In_Col=0f; // khoảng cách theo chiều ngang giữa các đơn vị trong 1 cột( để nghiêng nhìn cho dễ)
    Vector2 position;
    public Vector2 maxPos;
    public Vector2 minPos;
    public List<Vector2> targetPositions;  // Danh sách các vị trí mục tiêu cho các đơn vị
    public List<int> unitPerCol; // cột 1 thì unitPerCol[1-1];
    public int totalColum=0; // tính tư cột 1
    private float minY, maxY, lenghtUnits, toaDoX;
    Renderer renderer;

    void Start()
    {
        GetRenderArea();
       // Create_defPosition();// tạo defense với các unit ban đầu
    }
    public void SetUnitForArea(List<UnitListOrder> unit){
            this.unit=unit;
            Debug.Log("Đã tạo ra listunit cho area");
    }

    // Update is called once per frame
    void GetRenderArea(){// lấy các tọa độ cần thiết cho việc sắp xếp
        renderer = GetComponent<Renderer>();
                // Lấy tọa độ của bounds
               // Vector2 min = renderer.bounds.min; // Tọa độ thấp nhất
                //Vector2 max = renderer.bounds.max; // Tọa độ cao nhất                                 
                maxPos = renderer.bounds.max;
                minPos =  renderer.bounds.min;
                Debug.Log("Tọa độ thấp nhất: " + minPos);
                Debug.Log("Tọa độ cao nhất: " + maxPos);
                 minY=minPos.y;
                 maxY=maxPos.y;
                 toaDoX= maxPos.x;// điểm spawns
                 lenghtUnits=Mathf.Abs(maxY-minY);       
    }
   public void Create_defPosition(){// tạo các tọa đố 
         targetPositions.Clear();  // Xóa danh sách cũ
         unitPerCol.Clear();
         totalColum=0;
        // sắp xếp bắt đầu
        Debug.Log("Start def");
        //tính số lượng hàng dọc cần thiết cho việc sắp xếp
        totalColum=unit.Count/Max_Unit_Per_Col;
        for(int i=1; i<=totalColum; i++){
            unitPerCol.Add(Max_Unit_Per_Col);        
        }
        if(unit.Count%Max_Unit_Per_Col!=0){
            totalColum++;
            unitPerCol.Add(unit.Count%Max_Unit_Per_Col);
        }

        float preX=toaDoX;
        float preY=maxY;
        // tạo các tọa độ
       Debug.Log("totalColumla"+totalColum);
        for(int col=0;col<totalColum;col++){ //xét các cột
            Debug.Log(preX+","+ preY+","+ minY+"lần thứ "+col);
            // xem cột 1 có bao nhiêu unit
            Debug.Log("Xét cột "+ col+" có số vị trí là: "+ unitPerCol[col]);
               float spaceBtwUnitIn_Col=lenghtUnits/(unitPerCol[col]+1); // khoảng cách giữa các đơn vị trong 1 cột
            for(int i=0; i< unitPerCol[col] ;i++){// thiết lập tọa độ vị trị 
             
                position= new Vector2(preX-space_X_In_Col,preY- spaceBtwUnitIn_Col*(i+1));
                   ;
               int unitIndex =i+(col)*Max_Unit_Per_Col; // Tính chỉ số của unit
                   Debug.Log("Điểm thứ  "+ unitIndex+"có vector là: "+position);
            if (unitIndex < unit.Count && unit[unitIndex].prefab.CompareTag("Player")) // Kiểm tra chỉ số
            {
                unit[unitIndex].prefab.GetComponent<PlayerController>().Set_Def_Position(position);
                unit[unitIndex].currentOrder = "def" + position; // Cập nhật currentOrder
            }
               // Debug.Log("Thêm tọa độ vào điểm thứ "+i+" cột"+col+"tọa độ"+position);
                targetPositions.Add(position);
            }
            Debug.Log("Trừ toaDoX kết thúc lần "+col);
            preX=preX -space_X_Btw_Col;   
            preY=maxY;
        }
    }

}
