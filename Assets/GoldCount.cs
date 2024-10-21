using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Thêm thư viện để sử dụng IPointerClickHandler
using TMPro;
using Unity.VisualScripting;
using System;

public class GoldCount : MonoBehaviour, IPointerClickHandler
{
    public Button gold_Button;
    public int currentGold = 0;
    public int MAX_GOLD;
    public float time_add_gold = 2f;
    private float time_add_gold_TIMER = 0;
    public int gold_value = 5; // giá trị tăng thêm mỗi time_add_gold

    public int MAX_VALUE_GOLD = 50; // Giá trị tăng thêm cho MAX_GOLD
    public int upgrade_Gold_Value = 50;
    public EnemyBehavius enemyBehavius;
    public TextMeshProUGUI upgradeGold_btn;
    public int goldAge = 0;
    public Level_War_Mod levelWarMod;
    public bool isWarMod = false;



    void Start()
    {
        if (gold_Button == null)
        {
            Debug.LogError("Button không được gán! Hãy kiểm tra lại trong Inspector.");
        }
        enemyBehavius = FindAnyObjectByType<EnemyBehavius>();
        upgradeGold_btn.text = upgrade_Gold_Value.ToString();
        GameObject gameManager = GameObject.Find("GAME_MANAGER");
        levelWarMod = gameManager.GetComponentInChildren<Level_War_Mod>();
        Debug.Log(levelWarMod.gameMod);
        if (levelWarMod.gameMod == GameMod.War)
        {
            //đăng kí sự kiện
            Debug.Log("đăng kí sk con");
            levelWarMod.OnGameModeChanged_War += HandleGameModeChanged_War;
            isWarMod = true;
            MAX_GOLD = levelWarMod.currentGold;
            this.currentGold = MAX_GOLD;
            upgradeGold_btn.text = "WAR";
        }
    }

    private void HandleGameModeChanged_War()
    {
        Debug.Log("Xử lí sk");

    }

    void FixedUpdate()
    {

        if (!isWarMod)
        {
            if (currentGold > MAX_GOLD)
            {
                currentGold = MAX_GOLD;
            }

            if (currentGold < MAX_GOLD)
            {
                if (time_add_gold_TIMER >= time_add_gold)
                {
                    currentGold += gold_value;
                    time_add_gold_TIMER = 0;
                }
            }
        }


        if (gold_Button != null)
        {
            TextMeshProUGUI buttonText = gold_Button.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                buttonText.text = currentGold.ToString();
            }
            else
            {
                Debug.LogError("Không tìm thấy thành phần TextMeshProUGUI trên Button.");
            }
        }
        time_add_gold_TIMER += Time.deltaTime;
    }

    void Update()
    {
    }

    // Xử lý sự kiện khi người dùng chạm vào Button
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isWarMod)
        {
            if (currentGold >= upgrade_Gold_Value)
            {
                currentGold -= upgrade_Gold_Value;
                int old_Max_Gold = MAX_GOLD;
                MAX_GOLD += MAX_VALUE_GOLD;
                // ChangeUpgradeGoldValue(old_Max_Gold);
                upgrade_Gold_Value = (int)(MAX_GOLD * (0.75f));
                upgradeGold_btn.text = upgrade_Gold_Value.ToString(); ;

                goldAge++;
            }
        }
        else
        {
            upgradeGold_btn.text = "WAR";
        }
    }
    public void ChangeUpgradeGoldValue(int oldMaxGold)
    {
        upgrade_Gold_Value = (int)(MAX_GOLD * (0.75f));
        // upgrade_Gold_Value = (MAX_GOLD + oldMaxGold) / 2;
        //upgrade_Gold_Value = (MAX_GOLD + MAX_VALUE_GOLD) / (goldAge * 2);

        // do
        // {
        //     upgrade_Gold_Value = 50;
        //     ++goldAge;
        //     MAX_GOLD = 50 * goldAge;
        //     upgrade_Gold_Value += (MAX_GOLD) / 4;
        //     upgradeGold_btn.text = goldAge.ToString();
        // } while (upgrade_Gold_Value < MAX_GOLD);

    }
}