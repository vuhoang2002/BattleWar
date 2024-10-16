using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Timers;
using UnityEngine.UI;

[System.Serializable]
public class SpawnTimer
{
    public int enemyName; // Tên kẻ địch
    public int timerSpawn; // Thời gian để spawn (tính bằng giây)
    public int spawnCount; // Số lượng kẻ địch cần spawn
}

public class Level1 : MonoBehaviour
{
    public int modPlay; // 0 classic, 1 defense, 2 attack, 3 attack with current gold, 4 sinh tồn
    public bool is_Classic_Mod;
    public bool is_Defense_Mod = true;
    public bool is_Attack_Mod;
    public bool is_War_Mod;
    public bool is_Survival_Mod;
    public List<SpawnTimer> spawnTimer = new List<SpawnTimer>();
    private EnemyBehavius eB;
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
            if (is_War_Mod)
            {
                title.text = "Chiến thắng với " + currentGold + " vàng";
            }
            if (is_Survival_Mod)
            {
                title.text = "Sống sót trong vòng " + timePlay + "s";
            }
            if (is_Defense_Mod)
            {
                title.text = "Phòng thủ khỏi các đợt tấn công của kẻ địch";
            }
            if (is_Classic_Mod)
            {
                title.text = "Phá hủy lâu đài đối phương và bảo vệ lâu đài của mình";
            }
            if (is_Attack_Mod)
            {
                title.text = "Phá hủy lâu đài kẻ địch trong " + timePlay + "s";
            }
        }

        eB = GetComponent<EnemyBehavius>();

        // Khởi tạo Timer
        //  timer = new Timer(1000); // 1 giây
        //timer.Elapsed += OnTimerElapsed; // Đăng ký sự kiện
        //timer.AutoReset = true; // Tự động reset
        //timer.Enabled = true; // Bắt đầu timer

        // Bắt đầu Coroutine để spawn kẻ địch
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        StartCoroutine(SpawnEnemies());
        while (timePlay > 0)
        {
            yield return new WaitForSeconds(1); // Đợi 1 giây
            elapsedTime++; // Tăng elapsedTime mỗi giây
            if (is_Defense_Mod)
            {
                timePlay--; // Giảm thời gian chơi
            }
            // Kiểm tra nếu timePlay hết
            if (timePlay <= 0)
            {
                // Thực hiện các hành động khi thời gian chơi kết thúc
                break; // Thoát khỏi vòng lặp
            }
        }

        // Bắt đầu Coroutine để spawn kẻ địch

    }

    void FixedUpdate()
    {
        if (elapsedTime == 15)
        {
            eB.Set_eAtk();
            eB.SetAllEnemyBehavius();
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (spawnTimer.Count > 0) // Lặp cho đến khi danh sách rỗng
        {
            for (int i = 0; i < spawnTimer.Count; i++)
            {
                SpawnTimer spawn = spawnTimer[i];

                // Đợi cho đến khi đến thời gian spawn
                float waitTime = spawn.timerSpawn - elapsedTime;

                // Kiểm tra nếu thời gian còn lại lớn hơn 0 trước khi chờ
                if (waitTime > 0)
                {
                    yield return new WaitForSeconds(waitTime);
                }

                // Gọi hàm SpawnEnemy với số lượng kẻ địch tương ứng
                eB.SpawnEnemy(spawn.enemyName, spawn.spawnCount);
                //eB.Set_eAtk();
                // Loại bỏ mục khỏi danh sách sau khi đã spawn
                spawnTimer.RemoveAt(i);
                i--; // Giảm chỉ số để tránh bỏ sót mục tiếp theo
            }
        }
    }
}