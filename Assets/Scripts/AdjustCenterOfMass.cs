using UnityEngine;

public class AdjustCenterOfMass : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Điều chỉnh center of mass
        rb.centerOfMass = new Vector2(0, -5f); // Ví dụ: di chuyển xuống dưới
    }
}