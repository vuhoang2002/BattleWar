using UnityEngine;

public class CancelChosen : MonoBehaviour
{
    public GameObject OrderPanel_For_1_Unit;
    public GameObject FunctionCanva;
    public void HandleButtonClick()
    {
        // Tìm tất cả các gameobject có tag "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        offChosenPlayerCam();
        // Duyệt qua tất cả các gameobject "Player" và thiết lập isChosen = false
        foreach (GameObject player in players)
        {
            if (player.TryGetComponent(out PlayerController playerController))
            {
                playerController.isChosen = false;
                playerController.Set_isSelect(false);
                playerController.Set_CanChosen(true);
                playerController.SetChosenPlayer(null);
            }
        }
        //tắt OrderCanva unit
        Debug.Log("Cancel Chosen");
        offJoyStickCanva();
    }
    public void offJoyStickCanva()
    {
        GameObject BattleCanvas = GameObject.Find("BattleCanva");
        Transform joyStickCanvaTransform = BattleCanvas.transform.Find("JoyStickCanva");
        joyStickCanvaTransform.gameObject.SetActive(false);
        // if (FunctionCanva == null)
        // {
        //     FunctionCanva = BattleCanvas.transform.Find("FunctionCanva").gameObject;

        // }
        // FunctionCanva.SetActive(false);
        new FindObjectAndUI().Off_FunctionButton();
        if (OrderPanel_For_1_Unit == null)
        {
            OrderPanel_For_1_Unit = new FindObjectAndUI().Find_OrderPanel_OneUnit();
        }

        OrderPanel_For_1_Unit.SetActive(false);

    }
    public void offChosenPlayerCam()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        CameraControl cam = mainCamera.GetComponent<CameraControl>();
        cam.setChosenPlayer(null, false);
        cam.Set_IsLockCamera(false);
        Debug.Log("Xóa chosenPlayer");
    }
    public void offSelectCanva()
    {
        GameObject orderUnitType = new FindObjectAndUI().Find_OrderPanelFor_OneUnitType();
        orderUnitType.SetActive(false);
        orderUnitType = new FindObjectAndUI().Find_OrderPanel_OneUnit();
        orderUnitType.SetActive(false);
        orderUnitType = new FindObjectAndUI().Find_OrderSelectUnit_Buton();
        orderUnitType.SetActive(false);
    }
}