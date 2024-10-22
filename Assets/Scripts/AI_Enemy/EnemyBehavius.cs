using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavius : MonoBehaviour
{
    // Start is called before the first frame update
    public GameMod gameMod;
    public EnemyManager enemyManager;
    public UnitListManager playerManager;
    // public List<EnemyType> enemyType;
    public UnitOrder enemyOrder;
    public int currentGold;
    public int MAX_GOLD;
    public int playerStrength;
    public int enemyStrength;
    public int playerCountAll;
    public int enemyCountAll;
    public float checkInterval = 1f;
    public GameObject enemyList;// tham chiếu đến enemyList
                                // public GameObject[] enemy;
    private int maxEnemy;

    void Start()
    {
        //enemyOrder = UnitOrder.Defend;


        if (playerManager == null)
        {
            playerManager = GameObject.FindAnyObjectByType<UnitListManager>();
        }
        if (enemyManager == null)
        {
            enemyManager = GameObject.FindAnyObjectByType<EnemyManager>();
        }
        //enemyType = enemyManager.enemyType;
        if (enemyList == null)
        {
            StartCoroutine(FindEneemyList());
        }
        StartCoroutine(AIBehavior());
    }
    IEnumerator FindEneemyList()
    {
        while (enemyList == null)
        {
            enemyList = GameObject.Find("EnemyList(Clone)");
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator AIBehavior()
    {
        while (true)
        {
            if (gameMod == GameMod.War)
            {
                yield break;// dừng coroutine ở chế độ chiến tranh
                // hành vi được dựa trên cod bên WarMod
            }
            //UpdateEnemyTroopCount(); // Cập nhật số lượng quân địch nếu cần
            // CheckOrderTransition(); // Kiểm tra điều kiện chuyển đổi lệnh
            CurrentEnemyOrder();
            yield return new WaitForSeconds(checkInterval); // Kiểm tra hành vi theo khoảng thời gian
        }
    }

    void CurrentEnemyOrder()
    {
        switch (enemyOrder)
        {
            case UnitOrder.Attack:
                HandleAttack();

                break;
            case UnitOrder.Defend:
                HandleDefend();
                break;
            case UnitOrder.Retreat:
                HandleRetreat();
                break;
            case UnitOrder.Hold:
                HandleHold();
                break;
        }
    }
    public void Current_EnemyOrder(UnitOrder orderFromOtherFile)
    {
        enemyOrder = orderFromOtherFile;
        switch (enemyOrder)
        {
            case UnitOrder.Attack:
                HandleAttack();
                break;
            case UnitOrder.Defend:
                HandleDefend();
                break;
            case UnitOrder.Retreat:
                HandleRetreat();
                break;
            case UnitOrder.Hold:
                HandleHold();
                break;
        }
    }

    void UpdateEnemyTroopCount()
    {
        // Logic để cập nhật số lượng quân địch từ một nguồn dữ liệu khác (nếu có)
        // Ví dụ: enemyTroops = GameManager.Instance.GetEnemyCount();
    }

    void CheckOrderTransition()
    {
        if (enemyCountAll < maxEnemy && currentGold >= 0) //goldCostPerTroop)
        {
            //enemyOrder = UnitOrder.Idle; // Nếu có thể tăng cường quân, quay lại Idle
        }
        else if (enemyCountAll < playerCountAll)
        {
            enemyOrder = UnitOrder.Defend; // Chuyển sang phòng thủ nếu quân địch đông hơn
        }
        else if (enemyStrength < playerStrength / 2)
        {
            enemyOrder = UnitOrder.Retreat; // Rút lui nếu sức khỏe quá thấp
        }
        else if (playerCountAll <= enemyCountAll)
        {
            enemyOrder = UnitOrder.Attack; // Nếu quân địch ít hơn hoặc bằng, chuyển sang tấn công
        }
    }

    void HandleAttack()
    {
        // Thực hiện tấn công
        //tìm tất cả các object trong enemylist và dùng foreach
        // GameObject enemyList;
        Debug.Log("AI attacked all enemies!");
        foreach (Transform enemy in enemyList.transform)
        {
            if (enemy != null)
            {
                Debug.Log("enemy is" + enemy + "??");
                PlayerController playerController = enemy.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.Set_CurrentOrder(enemyOrder); // Tấn công từng kẻ thù
                }
                else
                {
                    Debug.Log("Ko có PlayerCOntroller cho enemy???");
                }
            }
            else
            {
                Debug.Log("Không có enemy");
            }
        }

    }

    void HandleDefend()
    {
        foreach (Transform enemy in enemyList.transform)
        {
            enemy.GetComponent<PlayerController>().Set_CurrentOrder(enemyOrder); // Tấn công từng kẻ thù
            Debug.Log(enemy + "enemy is");

        }
        // Thực hiện hành động phòng thủ
        Debug.Log("AI is defending!");
    }

    void HandleRetreat()
    {
        foreach (Transform enemy in enemyList.transform)
        {
            enemy.GetComponent<PlayerController>().Set_CurrentOrder(enemyOrder); // Tấn công từng kẻ thù
        }
    }

    void HandleHold()
    {
        foreach (Transform enemy in enemyList.transform)
        {
            enemy.GetComponent<PlayerController>().Set_CurrentOrder(enemyOrder); // Tấn công từng kẻ thù
        }
    }

    void SpawEnemyUnit()
    {

    }
}
