using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yamabushi_tengu_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 targetPosition;
    public float flySpeed = 2.3f;
    public float distanceFly = 5f;
    public bool forceFly;
    PlayerController playerController;
    Attacks attacks;
    public GameObject flyArea;
    private Animator amt;
    public float oldmoveSpeed;
    public float searchAbilytyRaduis = 10f;
    void Start()
    {
        amt = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        oldmoveSpeed = playerController.moveSpeed;
        attacks = GetComponent<Attacks>();
        flyArea.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (attacks.abl1_Cd_Time <= 0 && playerController.isAtk_Order)
        {
            //flyArea.SetActive(true);
            FindFarthestRangerEnemy(searchAbilytyRaduis);

        }
        if (forceFly)
        {
            if (!playerController.isChosen)
            {
                if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    Vector3 movement = (targetPosition - transform.position).normalized * flySpeed * Time.deltaTime;
                    transform.Translate(movement);
                }
                else
                {
                    forceFly = false;
                }
            }
            else
            {
                playerController.moveSpeed = flySpeed;

            }
        }


    }


    void Tengu_Fly()
    {
        //StartCoroutine(flyTotarget());
        forceFly = true;
    }
    IEnumerator flyTotarget()
    {
        bool flag = true;
        while (flag)
        {
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 movement = (targetPosition - transform.position).normalized * flySpeed * Time.deltaTime;
                transform.Translate(movement);
            }
            else
            {
                flag = false;
                forceFly = false;
            }

        }
        yield return new WaitForEndOfFrame();
    }
    void FindFarthestRangerEnemy(float searchRadius)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerController.findTarget);
        float farthestDistance = 0;
        GameObject farthestTarget = null;

        // Kiểm tra mảng players có rỗng không
        if (players.Length == 0)
        {
            return; // Nếu không có kẻ thù nào, thoát hàm
        }

        foreach (GameObject target in players) // targets
        {
            if (target == null) continue; // Bỏ qua nếu target là null

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            //  Debug.Log("target là " + target);
            // Kiểm tra xem kẻ thù có trong bán kính tìm kiếm và có isRanger=true không
            PlayerController playerController = target.GetComponent<PlayerController>(); // Giả sử PlayerController là tên script chứa thuộc tính isRanger
            if (distanceToTarget < searchRadius && playerController != null && playerController.isRanger)
            {
                if (distanceToTarget > farthestDistance) // Tìm kẻ địch xa nhất
                {
                    farthestDistance = distanceToTarget;
                    farthestTarget = target;
                }
            }
        }
        if (farthestTarget == null)
        {
            return;
        }


        // Kiểm tra farthestTarget trước khi truy cập transform.position
        if (farthestTarget != null)
        {
            targetPosition = farthestTarget.transform.position;
        }
        amt.SetTrigger("isAbl1");
        attacks.abl1_Cd_Time = attacks.abl1_Cd;
    }
    public void ResetSpeedPlayer()
    {
        playerController.moveSpeed = this.oldmoveSpeed;
    }
}
