using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using System.Timers;
using UnityEngine.UI;



public class Level_Controller : MonoBehaviour
{
    [Header("Điều khiển chế độ")]
    public GameMod gameMod; // 0 classic, 1 defense, 2 attack, 3 attack with current gold, 4 sinh tồn
    private EnemyManager enemyManager;
    private UnitListManager playerManager;
    public int currentGold = 10000;
    public string title;
    public int timer;
    //public GameObject enemyList;
    public float elapsedTime = 0f; // Biến theo dõi thời gian đã trôi qua
                                   //private Timer timer; // Thời gian từ lúc bắt đầu chơi
    public UnitPanelFunction unitDataPanel;
    public List<UnitData> currentMatchUnitData;
    public delegate void GameModeChangedHandler_War();
    public delegate void GameModeChangedHandler_Survival();

    public event GameModeChangedHandler_War OnGameModeChanged_War;
    public event GameModeChangedHandler_War OnBattleStart;
    public event GameModeChangedHandler_Survival OnGameModeChanged_Survival;
    // public event GameModeChangedHandler_War OnBattleStart;
    public GameObject WarUI;
    public GameObject BeginUI;// mission to win
    private Level_Surival_Mode level_Surival_Mode;


    void Start()
    {
        // Thiết lập tiêu đề
        Time.timeScale = 0f;
        Find_NecescaryObject();
        Find_UnitEnemyInThisMath();
        GameObject battleCanva = GameObject.Find("BattleCanva");
        if (gameMod == GameMod.War)
        {
            // chế độ WarMode có thay đổi về vàng
            Debug.Log("khai báo sk");
            OnGameModeChanged_War?.Invoke();

            GameObject War_UI = Instantiate(WarUI, battleCanva.transform.position + new Vector3(0, 151, 0), Quaternion.identity);
            War_UI.transform.SetParent(battleCanva.transform);
            StartCoroutine(On_WarModeActive(timer, War_UI));
        }
        else if (gameMod == GameMod.Survival)
        {

            level_Surival_Mode = GetComponent<Level_Surival_Mode>();
            timer = (int)level_Surival_Mode.timeSur;
            GameObject War_UI = Instantiate(WarUI, battleCanva.transform.position + new Vector3(0, 151, 0), Quaternion.identity);
            OnGameModeChanged_Survival?.Invoke();
            title = "Sống sót trong vòng " + timer + "s";
            War_UI.transform.SetParent(battleCanva.transform);
            StartCoroutine(On_SurvivalModeActive(timer, War_UI));

        }
        //tạo UI nhiệm vụ
        GameObject beginUI_Ins = Instantiate(BeginUI, battleCanva.transform.position, Quaternion.identity);
        beginUI_Ins.transform.SetParent(battleCanva.transform);
        beginUI_Ins.GetComponent<BeginCanva>().SetTitleMissiton(title);


    }
    void Find_NecescaryObject()
    {
        // gameMod = GameMod.War;
        if (title != null)
        {
            title = "Chiến thắng với " + currentGold + " vàng";
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
    IEnumerator On_WarModeActive(float timeToBegin, GameObject warUI)
    {
        TextMeshProUGUI textUI = warUI.GetComponentInChildren<TextMeshProUGUI>();
        for (int i = (int)timeToBegin; i >= 0; i--)
        {
            textUI.text = i + "s";
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
        textUI.text = "Battle Start";
        //yield return new WaitForSeconds(timeToBegin);
        GetComponent<EnemyBehavius>().Current_EnemyOrder(UnitOrder.Attack);
        OnBattleStart?.Invoke();
        Debug.Log("Khai báo sk OnBattleStart");
        yield return new WaitForSeconds(3f);
        Destroy(warUI);
    }
    IEnumerator On_SurvivalModeActive(float timeToPlay, GameObject warUI)
    {
        TextMeshProUGUI textUI = warUI.GetComponentInChildren<TextMeshProUGUI>();
        for (int i = (int)timeToPlay; i >= 0; i--)
        {
            textUI.text = "Survival in: " + i + "s";
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
        //yield return new WaitForSeconds(timeToBegin);
        GetComponent<EnemyBehavius>().Current_EnemyOrder(UnitOrder.Attack);
        // OnBattleStart?.Invoke();
        // Debug.Log("Khai báo sk OnBattleStart");
        yield return new WaitForSeconds(1f);
        Destroy(warUI);
        new Victory_Or_Loss().Get_Victory();
    }
    public EnemyManager Get_EnemyManager()
    {
        return enemyManager;
    }
    public UnitListManager Get_UnitListManager()
    {
        return playerManager;
    }


}



