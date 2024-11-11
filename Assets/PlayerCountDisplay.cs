using UnityEngine;
using TMPro;

public class PlayerCountDisplay : MonoBehaviour
{
    public TextMeshProUGUI playerCountText; // Tham chiếu đến TextMeshPro để hiển thị số lượng player
    public int MAX_PLAYER_COUNT = 30;
    public bool isMaxPlayer = false;
    public int playerCount;

    public GameObject gameManager; // Tham chiếu đến GameManager
    public Level_Controller myLevelMode;
    public bool isPlayerInWarMod = false;
    public GameObject loseUi;

    void Start()
    {
        // Tìm GameManager trong scene
        gameManager = GameObject.Find("GAME_MANAGER");
        myLevelMode = gameManager.GetComponent<Level_Controller>();

        myLevelMode.OnBattleStart += HandleBattleStart; // Đăng ký vào sự kiện
        Debug.Log("Đăng kí sự kiện OnBattleStart");


    }

    void Update()
    {
        playerCountText.text = $"{playerCount}/{MAX_PLAYER_COUNT}"; // Cập nhật Text với số lượng player

        // Cập nhật trạng thái isMaxPlayer
        isMaxPlayer = playerCount >= MAX_PLAYER_COUNT;
        if (isPlayerInWarMod)
        {
            if (playerCount <= 0)
            {
                // thua
                //  Debug.Log("Thua");
                new Victory_Or_Loss().Get_Loss();
            }
        }

    }

    public bool get_isMaxPlayer()
    {
        return isMaxPlayer;
    }

    private void HandleBattleStart()
    {
        isPlayerInWarMod = true;
        // Kiểm tra số lượng người chơi khi trận chiến bắt đầu
    }

    // void OnDestroy()
    // {
    //     // Hủy đăng ký sự kiện để tránh lỗi
    //     if (nyLevelMode != null)
    //     {
    //         nyLevelMode.OnBattleStart -= HandleBattleStart;
    //     }
    // }
}