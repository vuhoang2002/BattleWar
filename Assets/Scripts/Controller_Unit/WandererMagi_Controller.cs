using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandererMagi_Controller : MonoBehaviour
{
    [Header("Abl1: ThunderStrike")]
    public GameObject thunerStrike; // Prefab của FireBall
    public Transform spawnLocation; // Vị trí để spawn FireBall
    public int thunerCount = 6;// số lượng tia sét

    private int thunderAtk;
    private bool arrowDirection; // Khai báo biến arrowDirection
                                 // private Animator amtFireBall;
    public static bool isUpgrade = false;
    private int extraDmg = 0;
    public float spaceBtwThunder;
    public float currentPositionThunder;//tọa độ X tia sét hiện tại
    private int nextThunder = 1;// player là 1, enemy là -1
    void Start()
    {
        thunderAtk = GetComponent<Attacks>().abl1_Atk;
        extraDmg = GetComponent<Attacks>().extraDmg;
        OnUpgrade();

    }
    void OnUpgrade()
    {

        if (isUpgrade)
        {
            gameObject.GetComponent<Animator>().SetBool("isUpgrade", true);
            thunderAtk += 5;
        }
    }
    public void CheckNextThunder()
    {
        if (gameObject.GetComponent<PlayerController>().isRightWay)
        {
            nextThunder = 1;
        }
        else
        {
            nextThunder = -1;
        }
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

    public void Spawn_ThunderStrike()// ko xài nx
    {
        StartCoroutine(SpawnThunderStrikesCoroutine());

    }

    void SpawceToTarget(GameObject target)
    {
        // ko cần nx
        // StartCoroutine(SpawnThunderStrikesCoroutine());
    }

    private IEnumerator SpawnThunderStrikesCoroutine()
    {
        int deadDmg = thunderAtk;
        var controller = GetComponent<PlayerController>() ?? (Component)GetComponent<PlayerController>();
        bool arrowDirection = false;

        if (controller != null)
        {
            arrowDirection = controller is PlayerController player ? player.isRightWay : ((PlayerController)controller).isRightWay;
        }

        // Spawn tia sét với thời gian trễ
        for (int i = 0; i < thunerCount; i++)
        {
            Vector3 newSpawn = new Vector3(i, 0, 0);
            GameObject thunderStrikeInstance = Instantiate(thunerStrike, spawnLocation.position + newSpawn, Quaternion.identity);
            thunderStrikeInstance.SetActive(true);

            // Đợi 0,3 giây trước khi spawn tia sét tiếp theo
            yield return new WaitForSeconds(0.3f);
        }
    }

    //h dùng phương pháp này
    void Spawn_1st_Thunder()
    {
        int deadDmg = thunderAtk;
        CheckNextThunder();
        Vector3 newSpawn = new Vector3(1 * nextThunder * spaceBtwThunder, 0, 0);
        GameObject thunderStrikeInstance = Instantiate(thunerStrike, spawnLocation.position + newSpawn, Quaternion.identity);
        thunderStrikeInstance.SetActive(true);

    }
    void Spawn_2nd_Thunder()
    {
        CheckNextThunder();

        int deadDmg = thunderAtk;
        Vector3 newSpawn = new Vector3(2 * nextThunder * spaceBtwThunder, 0, 0);
        GameObject thunderStrikeInstance = Instantiate(thunerStrike, spawnLocation.position + newSpawn, Quaternion.identity);
        thunderStrikeInstance.SetActive(true);


    }
    void Spawn_3th_Thunder()
    {
        //int deadDmg = thunderAtk;
        Vector3 newSpawn = new Vector3(3 * nextThunder * spaceBtwThunder, 0, 0);
        GameObject thunderStrikeInstance = Instantiate(thunerStrike, spawnLocation.position + newSpawn, Quaternion.identity);
        thunderStrikeInstance.SetActive(true);

    }
    void Spawn_4th_Thunder()
    {
        CheckNextThunder();
        Vector3 newSpawn = new Vector3(4 * nextThunder * spaceBtwThunder, 0, 0);
        GameObject thunderStrikeInstance = Instantiate(thunerStrike, spawnLocation.position + newSpawn, Quaternion.identity);
        thunderStrikeInstance.SetActive(true);

    }
    void Spawn_5th_Thunder()
    {
        CheckNextThunder();

        Vector3 newSpawn = new Vector3(5 * nextThunder * spaceBtwThunder, 0, 0);
        GameObject thunderStrikeInstance = Instantiate(thunerStrike, spawnLocation.position + newSpawn, Quaternion.identity);
        thunderStrikeInstance.SetActive(true);

    }
    void Spawn_6th_Thunder()
    {
        CheckNextThunder();
        Vector3 newSpawn = new Vector3(6 * nextThunder * spaceBtwThunder, 0, 0);
        GameObject thunderStrikeInstance = Instantiate(thunerStrike, spawnLocation.position + newSpawn, Quaternion.identity);
        thunderStrikeInstance.SetActive(true);

    }
}

