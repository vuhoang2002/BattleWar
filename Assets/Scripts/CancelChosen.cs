using UnityEngine;

public class CancelChosen : MonoBehaviour
{
    public void HandleButtonClick()
    {
        // Tìm tất cả các gameobject có tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Duyệt qua tất cả các gameobject "Player" và thiết lập isChosen = false
        foreach (GameObject player in players)
        {
            if (player.TryGetComponent(out PlayerController playerController))
            {
                playerController.isChosen = false;
                playerController.Set_CanChosen(true);
            }
        }
    }
}