using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningMageControllder : MonoBehaviour
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
    void Start()
    {
        //isUpgrade=true;
        thunderAtk = GetComponent<Attacks>().abl1_Atk;
        //     amtFireBall = fireBallAbl1.GetComponent<Animator>();
        extraDmg = GetComponent<Attacks>().extraDmg;
        OnUpgrade();
        //lastPoint = spawnLocation.position.x + 6f;
    }
    void OnUpgrade()
    {

        if (isUpgrade)
        {
            gameObject.GetComponent<Animator>().SetBool("isUpgrade", true);
            thunderAtk += 5;
        }
    }

    void SpawceToTarget(GameObject target)
    {
        // ko cần nx
        // StartCoroutine(SpawnThunderStrikesCoroutine());
    }

    public void Spawn_ThunderStrike()
    {
        StartCoroutine(SpawnThunderStrikesCoroutine());
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
        for (int i = 1; i <= thunerCount; i++)
        {
            Vector3 newSpawn;
            if (GetComponent<PlayerController>().isRightWay)
            {
                newSpawn = new Vector3(i, 0, 0);
            }
            else
            {
                newSpawn = new Vector3(-i, 0, 0);
            }

            GameObject thunderStrikeInstance = Instantiate(thunerStrike, spawnLocation.position + newSpawn, Quaternion.identity);
            thunderStrikeInstance.SetActive(true);

            // Đợi 0,3 giây trước khi spawn tia sét tiếp theo
            yield return new WaitForSeconds(0.3f);
        }
    }
}

