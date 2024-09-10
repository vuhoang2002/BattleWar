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
        offChosenPlayerCam();
        offJoyStickCanva();
    }
      private void offJoyStickCanva(){
          GameObject BattleCanvas=GameObject.Find("BattleCanva");
          Transform joyStickCanvaTransform = BattleCanvas.transform.Find("JoyStickCanva");
          joyStickCanvaTransform.gameObject.SetActive(false);
    }
    private void offChosenPlayerCam(){
        GameObject mainCamera= GameObject.Find("Main Camera");
        CameraControl cam= mainCamera.GetComponent<CameraControl>();
        cam.setChosenPlayer(null, false);
    }
}