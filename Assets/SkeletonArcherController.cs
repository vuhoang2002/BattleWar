using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay2D(Collider2D other)
    {
        // Kiểm tra xem collider va chạm có phải là Enemy hay EnemyCastle không
        GetComponent<Attacks>().isAbl1 = Vector3.Distance(other.transform.position, transform.position) >= 1f;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Nếu ra ngoài BoxCollider, đặt lại trạng thái isAbl1
        GetComponent<Attacks>().isAbl1 = false; // Đặt lại trạng thái
    }
}
