using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Timers;
using UnityEngine.UI;



public class Level_War_Mod : MonoBehaviour
{
    public GameMod gameMod; // 0 classic, 1 defense, 2 attack, 3 attack with current gold, 4 sinh tồn
    private EnemyBuild eB;
    public int currentGold = 10000;
    public Text title;
    public int timePlay;
    public float elapsedTime = 0f; // Biến theo dõi thời gian đã trôi qua
    //private Timer timer; // Thời gian từ lúc bắt đầu chơi

    void Start()
    {
        // Thiết lập tiêu đề
        if (title != null)
        {
            title.text = "Chiến thắng với " + currentGold + " vàng";
        }
        eB = GetComponent<EnemyBuild>();

    }



}



