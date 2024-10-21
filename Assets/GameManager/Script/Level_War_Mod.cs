using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Timers;
using UnityEngine.UI;



public class Level_War_Mod : MonoBehaviour
{
    [Header("Chế độ tấn công đến khi 1 trong 2 đội quân bị tiêu diệt")]
    public GameMod gameMod; // 0 classic, 1 defense, 2 attack, 3 attack with current gold, 4 sinh tồn
    private EnemyManager enemyManager;
    private UnitListManager playerManager;
    public int currentGold = 10000;
    public Text title;
    public int timePlay;
    //public GameObject enemyList;
    public float elapsedTime = 0f; // Biến theo dõi thời gian đã trôi qua
                                   //private Timer timer; // Thời gian từ lúc bắt đầu chơi
    public UnitPanelFunction unitDataPanel;
    public List<UnitData> currentMatchUnitData;
    public delegate void GameModeChangedHandler_War();
    public event GameModeChangedHandler_War OnGameModeChanged_War;


    void Start()
    {
        // Thiết lập tiêu đề
        Find_NecescaryObject();
        Find_UnitEnemyInThisMath();
        if (gameMod == GameMod.War)
        {
            // chế độ WarMode có thay đổi về vàng
            Debug.Log("khai báo sk");
            OnGameModeChanged_War?.Invoke();
        }
        StartCoroutine(On_WarModeActive(timePlay));

    }
    void Find_NecescaryObject()
    {
        gameMod = GameMod.War;
        if (title != null)
        {
            title.text = "Chiến thắng với " + currentGold + " vàng";
        }
        // if (enemyList == null)
        // {
        //     enemyList = GameObject.Find("EnemyList(Clone)");
        // }
        if (playerManager == null)
        {
            playerManager = GameObject.FindAnyObjectByType<UnitListManager>();
        }
        if (enemyManager == null)
        {
            enemyManager = GameObject.FindAnyObjectByType<EnemyManager>();
        }
    }
    void Find_UnitEnemyInThisMath()
    {
        foreach (var enemy in enemyManager.enemyType)
        {
            string enemyName = enemy.enemyPrefab.name;
            UnitData data = unitDataPanel.unitData.Find(unit => unit.unitTag == enemyName);
            currentMatchUnitData.Add(data);
        }
    }
    IEnumerator On_WarModeActive(float timeToBegin)
    {
        yield return new WaitForSeconds(timeToBegin);
        GetComponent<EnemyBehavius>().Current_EnemyOrder(UnitOrder.Attack);
    }


}



