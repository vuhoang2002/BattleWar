using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    public int modPlay; // 0 classic, 1 defense, 2 attack, 3 attack with current gold, 4 sinh tồn
    public bool is_Classic_Mod;
    public bool is_Defense_Mod = true;
    public bool is_Attack_Mod;
    public bool is_War_Mod;
    public bool is_Survival_Mod;
    private Timer timer; // Thời gian từ lúc bắt đầu chơi
    private EnemyBehavius eB;
    public int currentGold = 10000;
    public int timePlay;
    public Text title;

    private float elapsedTime = 0f; // Biến theo dõi thời gian đã trôi qua

    void Start()
    {if(title!=null){
        // Thiết lập tiêu đề
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
        timer = new Timer(1000); // 1 giây
        timer.Elapsed += OnTimerElapsed; // Đăng ký sự kiện
        timer.AutoReset = true; // Đặt AutoReset là true để sự kiện được gọi mỗi giây
        timer.Enabled = true; // Bắt đầu timer
    }
private void OnTimerElapsed(object sender, ElapsedEventArgs e)
{
    elapsedTime += 1; // Tăng elapsedTime mỗi giây
   // Debug.Log("Thời gian đã trôi qua: " + elapsedTime + " giây "); // Hiện thời gian lên console
  

    
}
    void FixedUpdate()
    {
        // Kiểm tra nếu elapsedTime đạt 10 giây
       // eB.setAllEnemyBehavius();
       if (elapsedTime == 3) // Sử dụng >= để đảm bảo gọi nhiều lần nếu cần
    {
        eB.SpawnEnemy("E_Knight", 4); // Gọi hàm SpawnEnemy
        elapsedTime += 1; // Giảm elapsedTime xuống 10
    }
      if (elapsedTime == 15) // Sử dụng >= để đảm bảo gọi nhiều lần nếu cần
    {
        eB.SpawnEnemy("B_Earth_Golem", 1); // Gọi hàm SpawnEnemy
        eB.SpawnEnemy("E_Knight", 2);
        elapsedTime += 1; // Giảm elapsedTime xuống 10
    }
       
    }
}