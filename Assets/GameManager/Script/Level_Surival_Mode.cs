using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level_Surival_Mode : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeSur;
    public string titleSur;

    public EnemyManager enemyManager;
    public float minSpawnTime = 4f; // Thời gian tối thiểu giữa các lần spawn
    public float maxSpawnTime = 10f; // Thời gian tối đa giữa các lần spawn
    public int enemyTypeCount;
    public GameObject bossUnit;
    public float timeCallingBoss;
    public GameObject WarUI;
    public bool stopCallingMoster = false;
    public int monsters_WithBoss = 7;

    void Start()
    {
        if (enemyManager == null)
        {
            enemyManager = new FindObjectAndUI().Find_EnemyManager();
        }
        titleSur = "Sống sót trong vòng " + timeSur + "s";
        //enemyManager = GetComponent<Level_Controller>().Get_EnemyManager();
        enemyTypeCount = enemyManager.enemyType.Count;

        // GameObject battleCanva = GameObject.Find("BattleCanva");
        // GameObject War_UI = Instantiate(WarUI, battleCanva.transform.position + new Vector3(0, 151, 0), Quaternion.identity);
        // War_UI.transform.SetParent(battleCanva.transform);
        //War_UI.
        StartCoroutine(SpawnRandomEnemy_RandomTime());
        StartCoroutine(SpawnBossAfterTime());
    }

    // Update is called once per frame
    void Update()
    {
        // sau 60s gọi boss 1 lần
    }
    public IEnumerator SpawnRandomEnemy_RandomTime()
    {
        //spawn kẻ địch ngẫu nhiên tại thời điẻm
        //gọi thời gian ngẫu nhiên, cứ cách 1 khoảng thời gian spawn ra 1 kẻ địch đi về phía trước
        while (!stopCallingMoster)
        {
            float randomTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(randomTime);
            int randomEnemyIndex = UnityEngine.Random.Range(0, enemyTypeCount - 1);
            GameObject enemySpawner = enemyManager.SpawnEnemy2(randomEnemyIndex, 1);
            enemySpawner.GetComponent<PlayerController>().Set_BehaviusForPrefab(true, false, false);

        }
    }
    private IEnumerator SpawnBossAfterTime()
    {
        while (!stopCallingMoster)
        {
            yield return new WaitForSeconds(timeCallingBoss);
            if (bossUnit != null)
            {
                // Spawn boss
                GameObject bossUnit_Ins = enemyManager.SpawnUnit_ByPrefab(bossUnit);
                bossUnit.GetComponent<PlayerController>().SetBehavius(true, false, false, false);
                for (int i = 0; i < monsters_WithBoss; i++)
                {
                    enemyManager.SpawnEnemy(get_RandomUnitIndex(), 1);
                }
                yield return new WaitForSeconds(10f);// đợi nó xếp đội hình
                GetComponent<EnemyBehavius>().Current_EnemyOrder(UnitOrder.Attack);
            }
        }
    }
    public int get_RandomUnitIndex()
    {
        return UnityEngine.Random.Range(0, enemyTypeCount - 1);
    }



}
