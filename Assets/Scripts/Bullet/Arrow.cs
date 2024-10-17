using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Arrow : MonoBehaviour
{
    public int arow_Dmg; // basic_atk
    public float speedArrow = 2f;
    private float distanceTravelled = 0f; // Khoảng cách đã bay
    private bool arrowDirection = true; // Hướng bay mũi tên
    public float maxArrowDistance = 8f;
    public bool isChangre = false;// chưởng thì có animation destroy
    public bool isColide = false;
    // public string destroyName = "IceBall_Destroy";
    private Animator amt;
    private int extraDmg;
    void Start()
    {
        // Có thể khởi tạo mũi tên ở đây nếu cần   
        amt = GetComponent<Animator>();
        // if (amt != null)
        // {
        //     isChangre = true;
        // }
        //thiết lập orderlayout cho nó
        //SetOrderLayerForBullet();
    }


    public void SetArrowMaxDistante(float newMax)
    {
        maxArrowDistance = newMax;
    }

    void Update()
    {
        if (!isColide)
        {
            ArrowFly();
        }
    } //ss

    public void SetArrowDmg_Direction(int basic_Dmg, bool direction)
    {
        arow_Dmg = basic_Dmg;
        arrowDirection = direction; // Lưu lại hướng bay
    }

    public void OnTriggerEnter2D(Collider2D target)
    {

        target.GetComponent<Health>().TakeDamage(arow_Dmg);
        OnDestroy(); // Hủy mũi tên sau khi trúng đích
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
            OnDestroy(); // Hủy mũi tên
        }
    }

    public void OnDestroy()
    {
        isColide = true;
        if (isChangre)// changre là chưởng ;v
        {
            amt.SetBool("Destroy", true); // Bắt đầu animation isDestroy
            Invoke("DeleteSelf", 1f);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            Destroy(gameObject); // Nếu không có animation, hủy ngay lập tức
        }
    }

    void DeleteSelf()
    {
        Destroy(gameObject);
    }

}