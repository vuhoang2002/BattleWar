using UnityEngine;

public class ScaleMovement : MonoBehaviour
{
    public float highPos = -4.5f; // Giá trị Y khi prefab đạt kích thước lớn nhất
    public float lowPos = -2.5f;  // Giá trị Y khi prefab đạt kích thước nhỏ nhất
    public float minScale = 0.75f; // Kích thước nhỏ nhất
     public float maxScale = 1.5f; // Kích thước lớn nhất

    void Update()
    {
            float currentY = transform.position.y;

            // Tính toán tỉ lệ scale dựa trên tọa độ Y
            float scale = Mathf.Lerp(minScale, maxScale, Mathf.InverseLerp(lowPos, highPos, currentY));

        // Đặt scale cho prefab
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
