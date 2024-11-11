using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAbility : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedArrow = 2f;
    private float distanceTravelled = 0f; // Khoảng cách đã bay
    private bool arrowDirection = true; // Hướng bay mũi tên
    public float maxArrowDistance = 8f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ArrowFly();
    }
    public void SetUp(bool diraction)
    {
        arrowDirection = diraction;
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
            if (GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().SetBool("Destroy", true);
            }
            Destroy(gameObject); // Hủy mũi tên

        }
    }
}

