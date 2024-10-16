using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireVizardController : MonoBehaviour
{
    public GameObject fireBallAbl1; // Prefab của FireBall
    public Transform spawnFireBall; // Vị trí để spawn FireBall
    private int fireBallAtk;
    private bool arrowDirection; // Khai báo biến arrowDirection
                                 // private Animator amtFireBall;
    public static bool isUpgrade = false;
    private int extraDmg = 0;
    void Start()
    {
        //isUpgrade=true;
        fireBallAtk = GetComponent<Attacks>().abl1_Atk;
        //     amtFireBall = fireBallAbl1.GetComponent<Animator>();
        extraDmg = GetComponent<Attacks>().extraDmg;
        OnUpgrade();
    }
    void OnUpgrade()
    {

        if (isUpgrade)
        {
            gameObject.GetComponent<Animator>().SetBool("isUpgrade", true);
            fireBallAtk += 5;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Kiểm tra xem collider va chạm có phải là Enemy hay EnemyCastle không
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyCastle"))
        {
            if (other.GetComponent<BoxCollider2D>() != null)
            {
                GetComponent<Attacks>().isAbl1 = Vector3.Distance(other.transform.position, transform.position) >= 1f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Nếu ra ngoài BoxCollider, đặt lại trạng thái isAbl1
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyCastle"))
        {
            GetComponent<Attacks>().isAbl1 = false; // Đặt lại trạng thái
        }
    }

    public void Spawn_FireBall()
    {
        int deadDmg = fireBallAtk;
        var controller = GetComponent<PlayerController>() ?? (Component)GetComponent<PlayerController>();

        if (controller != null)
        {
            arrowDirection = controller is PlayerController player ? player.isRightWay : ((PlayerController)controller).isRightWay;
        }

        if (fireBallAbl1 != null && spawnFireBall != null) // Kiểm tra prefab và vị trí không null
        {
            // Spawn ra FireBall
            GameObject arrowInstance = Instantiate(fireBallAbl1, spawnFireBall.position, spawnFireBall.rotation);
            if (isUpgrade)
            {
                // arrowInstance.transform.localScale *= 3f; // Tăng kích thước gấp đôi
                Animator amtFireBall = arrowInstance.GetComponent<Animator>();
                amtFireBall.SetBool("isUpgrade", true);
                SpriteRenderer spriteRenderer = arrowInstance.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.red;
            }
            arrowInstance.SetActive(true);
            if (GetComponent<Attacks>().Get_IsDealExtraDmg())
            {
                deadDmg += extraDmg;
            }
            arrowInstance.GetComponent<Arrow>().SetArrowDmg_Direction(deadDmg, arrowDirection);
            //arrowInstance.transform.SetParent(transform);

            // Phóng to arrowInstance lên 2 lần nếu isUpgrade=true

        }
    }
}