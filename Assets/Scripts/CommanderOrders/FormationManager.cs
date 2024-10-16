
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FormationManager : MonoBehaviour
{
    public GameObject maxHeightPoint;  // Điểm giới hạn trên
    public GameObject minHeightPoint;  // Điểm giới hạn dưới
    public GameObject playerBattleArea;
    public List<GameObject> units;  // Danh sách các đơn vị (unit)
    public int maxUnitsPerColumn = 5;  // Số lượng đơn vị tối đa trong mỗi cột dọc (trục Y)
    public float spacingX = 1f;  // Khoảng cách giữa các đơn vị theo trục X (hàng ngang)
    public float spaceing_Btw_Y = 0.3f;// cái này khaongr cách chiều ngang giữa các đơn vị ở hàng dọc
    public float moveSpeed = 2.0f;  // Tốc độ di chuyển của các đơn vị
    Vector2 position;
    public Vector2 maxPos;
    public Vector2 minPos;
    private List<Vector2> targetPositions;  // Danh sách các vị trí mục tiêu cho các đơn vị
    Renderer renderer;

    void Awake()
    {
        if (units == null)
        {
            Debug.LogWarning("The 'units' list in FormationManager is not assigned.");
            return; // Ngừng thực hiện nếu units là null
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
        ArrangeFormation();
    }

    public void Active_Def_Function()
    {
        ArrangeFormation();
    }

    public void ArrangeFormation()
    {
        if (playerBattleArea == null)
        {
            maxPos = maxHeightPoint.transform.position;
            minPos = minHeightPoint.transform.position;
        }
        else
        {
            renderer = playerBattleArea.GetComponent<Renderer>();

            if (renderer != null)
            {
                // Lấy tọa độ của bounds
                Vector3 min = renderer.bounds.min; // Tọa độ thấp nhất
                Vector3 max = renderer.bounds.max; // Tọa độ cao nhất
                                                   // maxPos=max.y;
                                                   // minPos=min.y;
                maxPos = new Vector2(max.x, max.y);
                minPos = new Vector2(min.x, min.y);

            }
            else
            {
            }
        }

        targetPositions.Clear();  // Xóa danh sách cũ

        // Tính toán số lượng hàng dọc cần thiết
        int totalRows = Mathf.CeilToInt((float)units.Count / maxUnitsPerColumn);

        // Chia đều khoảng cách giữa các hàng dọc
        float spacingY = (maxPos.y - minPos.y) / (maxUnitsPerColumn - 1);

        float currentX = maxPos.x;  // Bắt đầu từ vị trí của điểm maxHeight trên trục X
        float currentY = maxPos.y;  // Bắt đầu từ maxHeight trên trục Y
        float preY = 0f;
        float preX = currentX;


        int unitIndex = 0;  // Chỉ số của unit trong danh sách

        // Sắp xếp đơn vị theo hàng dọc và hàng ngang
        for (int row = 0; row < totalRows && unitIndex < units.Count; row++)
        {
            // Thêm đơn vị vào hàng dọc cho tới khi đạt số lượng tối đa cho một cột
            preX = currentX;
            for (int i = 0; i < maxUnitsPerColumn && unitIndex < units.Count; i++)
            {
                position = new Vector2(preX - spaceing_Btw_Y, currentY);
                targetPositions.Add(position);
                currentY -= spacingY;  // Di chuyển xuống theo trục Y
                unitIndex++;
                preX = position.x;
                // preY=currentY;
            }
            // Sau khi hoàn thành một cột, di chuyển sang cột tiếp theo trên trục X và đặt lại vị trí Y
            currentX -= spacingX;  // Di chuyển sang tría theo trục X
            currentY = maxPos.y;  // Đặt lại vị trí Y cho cột tiếp theo
        }

        StartCoroutine(MoveUnitsToPositions());
    }

    private IEnumerator MoveUnitsToPositions()
    {
        int unitCount = Mathf.Min(units.Count, targetPositions.Count);
        List<Coroutine> moveCoroutines = new List<Coroutine>();

        for (int i = 0; i < unitCount; i++)
        {
            GameObject unit = units[i];
            Vector2 targetPos = targetPositions[i];

            PlayerController playerController = unit.GetComponent<PlayerController>();
            Coroutine moveCoroutine = StartCoroutine(MoveUnitToPosition(unit, targetPos, playerController));
            moveCoroutines.Add(moveCoroutine);
        }

        foreach (var coroutine in moveCoroutines)
        {
            yield return coroutine;
        }

        // Sau khi tất cả đơn vị đã di chuyển tới vị trí mục tiêu, xoay chúng về hướng đúng
        foreach (var unit in units)
        {
            PlayerController playerController = unit.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.Flip_To_Default_Direction();  // Xoay nhân vật
            }
        }
    }

    private IEnumerator MoveUnitToPosition(GameObject unitInstance, Vector2 targetPos, PlayerController playerController)
    {
        while (Vector2.Distance(unitInstance.transform.position, targetPos) > 0.1f)
        {
            unitInstance.transform.position = Vector2.MoveTowards(unitInstance.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    // thêm unit vào units
    public void AddUnit(GameObject unit)
    {
        if (unit != null && !units.Contains(unit))
        {
            units.Add(unit);
            ArrangeFormation();  // Cập nhật lại formation sau khi thêm đơn vị
        }
    }
    // xóa unit khỏi units(khi units diea á)
    public void DeleteUnit(GameObject unit)
    {
        if (unit != null && !units.Contains(unit))
        {
            units.Remove(unit);
            ArrangeFormation();  // Cập nhật lại formation sau khi xóa đơn vị
        }
    }

    public void ClearUnits()
    {
        units.Clear();
    }
}