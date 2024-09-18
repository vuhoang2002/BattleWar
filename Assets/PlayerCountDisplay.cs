using UnityEngine;
using TMPro;

public class PlayerCountDisplay : MonoBehaviour
{
    public TextMeshProUGUI playerCountText; // Tham chiếu đến TextMeshPro để hiển thị số lượng player
    public int MAX_PLAYER_COUNT=30;
    public bool isMaxPlayer=false;

    void Update()
    {
        int playerCount = GameObject.FindGameObjectsWithTag("Player").Length; // Tìm tất cả đối tượng có tag "Player"
        playerCountText.text = $"{playerCount}"+"/"+MAX_PLAYER_COUNT; // Cập nhật Text với số lượng player
        if(playerCount==MAX_PLAYER_COUNT){
            isMaxPlayer=true;// true thì ko thể spawn thêm
        }else{
            isMaxPlayer=false;
        }
    }
    public bool get_isMaxPlayer(){
        return isMaxPlayer;
    }
}