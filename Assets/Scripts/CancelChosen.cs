using UnityEngine;

public class CancelChosen : MonoBehaviour
{
    public GameObject OrderPanel_For_1_UnitsType;
    public GameObject FunctionCanva;
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
        //tắt OrderCanva unit

        offChosenPlayerCam();
        offJoyStickCanva();
    }
    public void offJoyStickCanva()
    {
        GameObject BattleCanvas = GameObject.Find("BattleCanva");
        Transform joyStickCanvaTransform = BattleCanvas.transform.Find("JoyStickCanva");
        joyStickCanvaTransform.gameObject.SetActive(false);
        if (FunctionCanva == null)
        {
            FunctionCanva = BattleCanvas.transform.Find("FunctionCanva").gameObject;

        }

        FunctionCanva.SetActive(false);
        OrderPanel_For_1_UnitsType.SetActive(false);

    }
    public void offChosenPlayerCam()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        CameraControl cam = mainCamera.GetComponent<CameraControl>();
        cam.setChosenPlayer(null, false);
    }
}