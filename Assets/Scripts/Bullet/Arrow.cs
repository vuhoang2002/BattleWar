using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int arow_Dmg; // basic_atk
    public float speedArrow = 3f;
    public bool isGoodBullet = true; // player là true
    private float distanceTravelled = 0f; // Khoảng cách đã bay
    private bool arrowDirection = true; // Hướng bay mũi tên
    public float maxArrowDistance=8f;

    void Start()
    {
        // Có thể khởi tạo mũi tên ở đây nếu cần
    }

    void Update()
    {
        ArrowFly();
    }

    public void SetArrowDmg_Direction(int basic_Dmg, bool direction)
    {
        arow_Dmg = basic_Dmg;
        arrowDirection = direction; // Lưu lại hướng bay
    }

    public void OnTriggerEnter2D(Collider2D target)
    {
        
        if (target.CompareTag("Enemy") && isGoodBullet)// dành cho player
        {
            Debug.Log("Bắn trúng địch " + target);
            target.GetComponent<Health>().TakeDamage(arow_Dmg);
            Destroy(gameObject); // Hủy mũi tên sau khi trúng đích
        }
        else if (target.CompareTag("Player") && !isGoodBullet) // dành cho enemy
        {
            Debug.Log("Bắn trúng " + target);
            target.GetComponent<Health>().TakeDamage(arow_Dmg);
            Destroy(gameObject); // Hủy mũi tên sau khi trúng đích
        }
    }

    public void ArrowFly()
    {
        // Tính khoảng cách đã bay
        float distance = speedArrow * Time.deltaTime;

        // Di chuyển theo hướng đã xác định
        if (arrowDirection)
        {
            transform.Translate(Vector2.right * distance); // Bay sang phải
        }
        else
        {
            transform.Translate(Vector2.left * distance); // Bay sang trái
        }

        // Cập nhật khoảng cách đã bay
        distanceTravelled += distance;

        // Kiểm tra xem mũi tên có bay quá 6f không
        if (distanceTravelled > maxArrowDistance)
        {
            Destroy(gameObject); // Hủy mũi tên
        }
    }
}