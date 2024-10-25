using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCardControl : MonoBehaviour
{
    // private Animator animator;

    // private bool isRunning = true; // Trạng thái chạy
    // private bool isAttack = true; // Trigger tấn công
    // private bool isShot = true; // Trigger bắn
    // private bool isAbl1 = true; // Trigger khả năng 1
    // public bool isShowOnCard = true; // Cờ để hiển thị trên card
    // private bool isCoroutineRunning = false; // Cờ để theo dõi trạng thái của coroutine

    // void Start()
    // {
    //     animator = GetComponent<Animator>(); // Lấy Animator
    // }

    // void Update()
    // {
    //     if (GetComponent<PlayerController>().target == null)
    //     {
    //         if (isShowOnCard && !isCoroutineRunning) // Kiểm tra nếu coroutine chưa chạy
    //         {
    //             StartCoroutine(ShowOnCard_Update());
    //         }
    //     }
    // }

    // public IEnumerator ShowOnCard_Update()
    // {
    //     isCoroutineRunning = true; // Đánh dấu rằng coroutine đang chạy

    //     // Tạo danh sách các hành động có thể thực hiện
    //     List<string> actions = new List<string>();

    //     // Thêm trigger vào danh sách nếu chúng đang được kích hoạt
    //     if (isAttack)
    //     {
    //         actions.Add("Attack");
    //     }
    //     if (isShot)
    //     {
    //         actions.Add("Shot");
    //     }
    //     if (isAbl1)
    //     {
    //         actions.Add("Ability1");
    //     }

    //     // Nếu isRunning là true, cũng thêm vào danh sách
    //     if (isRunning)
    //     {
    //         actions.Add("Run");
    //     }

    //     // Nếu có hành động khả thi, thực hiện một hành động ngẫu nhiên
    //     if (actions.Count > 0)
    //     {
    //         int randomIndex = UnityEngine.Random.Range(0, actions.Count);
    //         Debug.Log("Hành động thứ: " + actions[randomIndex]);
    //         yield return new WaitForSeconds(5f); // Chờ trước khi thực hiện hành động
    //         PerformAction(actions[randomIndex]);
    //     }

    //     isCoroutineRunning = false; // Đánh dấu rằng coroutine đã kết thúc
    // }

    // private void PerformAction(string action)
    // {
    //     switch (action)
    //     {
    //         case "Attack":
    //             animator.SetTrigger("isAttack");
    //             break;
    //         case "Shot":
    //             animator.SetTrigger("isShot");
    //             break;
    //         case "Ability1":
    //             animator.SetTrigger("isAbl1");
    //             break;
    //         case "Run":
    //             animator.SetBool("isRunning", true);
    //             break;
    //     }
    // }
}