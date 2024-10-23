using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectPlayer_Btn : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    public GameObject choSen;
    void Start()
    {

    }
    void Update()
    {

    }
    public void ShowJoyStickCanva()
    {   //joytick

        GameObject BattleCanvas = GameObject.Find("BattleCanva");
        Transform joyStickCanvaTransform = BattleCanvas.transform.Find("JoyStickCanva");
        joyStickCanvaTransform.gameObject.SetActive(true);
        // tìm order unittype

        //function
        joyStickCanvaTransform = BattleCanvas.transform.Find("FunctionCanva");
        joyStickCanvaTransform.gameObject.SetActive(true);
        GameObject atkbtn = joyStickCanvaTransform.Find("Atck_Btn").gameObject;
        PlayerController playerController = new PlayerController();
        choSen = playerController.GetChosenPlayer();
        choSen.GetComponent<PlayerController>().isChosen = true;
        atkbtn.GetComponent<ButtonHandler>().setAttacks_Var(choSen);//truyền attack vào ở đây
        MakeOtherPlayerCanNotSelect();
        LockCamForTheChosen(choSen);
        // tắt 1 số Canva không cần thiết
        joyStickCanvaTransform = BattleCanvas.transform.Find("OrderCanva");
        Transform child = joyStickCanvaTransform.transform.Find("PanelOrder_UnitType");
        child.gameObject.SetActive(false);
        // child = joyStickCanvaTransform.transform.Find("PanelOrder_OneUnit");
        //child.SetActive(false);
        gameObject.SetActive(false);

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // Ngăn chặn sự kiện click tiếp tục xuống dưới
        eventData.Use();
        ShowJoyStickCanva();
        // Thực hiện hành động UI của bạn
        Debug.Log("Clicked on UI!");
    }
    private void LockCamForTheChosen(GameObject chosen)
    {

        // Tìm kiếm camera
        GameObject mainCamera = GameObject.Find("Main Camera");

        if (mainCamera != null)
        {
            CameraControl cam = mainCamera.GetComponent<CameraControl>();
            if (cam != null)
            {
                cam.setChosenPlayer(chosen, true);
                cam.Set_IsLockCamera(true);
                //cam.MoveCameraToPosition(transform.position);
            }
        }
    }
    private void MakeOtherPlayerCanNotSelect()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Duyệt qua tất cả các gameobject "Player" và thiết lập canChosen = false
        foreach (GameObject player in players)
        {
            if (player.TryGetComponent(out PlayerController playerController))
            {
                playerController.canChosen = false;
            }
        }
    }


}
