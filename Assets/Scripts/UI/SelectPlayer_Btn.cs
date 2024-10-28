using System;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectPlayer_Btn : MonoBehaviour, IPointerClickHandler
{
    public delegate void SelectionChangedEventHandler();
    public event SelectionChangedEventHandler OnChosenPlayerSelect;
    public GameObject choSen;
    public TextMeshProUGUI unitCount;

    public void OnPointerClick(PointerEventData eventData)
    {
        eventData.Use();
        ShowJoyStickCanva();
        // Gọi sự kiện khi người chơi được chọn
        OnChosenPlayerSelect?.Invoke();
        Debug.Log("Clicked on UI!");
    }

    private void ShowJoyStickCanva()
    {
        GameObject battleCanvas = GameObject.Find("BattleCanva");
        Transform joyStickCanvaTransform = battleCanvas.transform.Find("JoyStickCanva");
        joyStickCanvaTransform.gameObject.SetActive(true);

        PlayerController playerController = new PlayerController();
        choSen = playerController.Get_SelectPlayer();
        choSen.GetComponent<PlayerController>().isChosen = true;
        new FindObjectAndUI().Show_FunctionButton(choSen.GetComponent<Attacks>().abilityCount);
        GameObject functionCanva = battleCanvas.transform.Find("FunctionCanva").gameObject;
        functionCanva.GetComponent<FunctionPanel>().chosen = choSen;
        functionCanva.GetComponent<FunctionPanel>().attacks = choSen.GetComponent<Attacks>();
        MakeOtherPlayerCanNotSelect();
        LockCamForTheChosen(choSen);
        functionCanva = battleCanvas.transform.Find("OrderCanva").gameObject;
        joyStickCanvaTransform = functionCanva.transform.Find("PanelOrder_UnitType");
        joyStickCanvaTransform.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }



    private void LockCamForTheChosen(GameObject chosen)
    {
        CameraControl cam = FindObjectOfType<CameraControl>();
        if (cam != null)
        {
            cam.setChosenPlayer(chosen, true);
            cam.Set_IsLockCamera(true);
        }
    }

    private void MakeOtherPlayerCanNotSelect()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.TryGetComponent(out PlayerController playerController))
            {
                playerController.canChosen = false;
            }
        }
    }

    public void SetUp_SelectUnitButton(Sprite unitAvatar, byte uintCount)
    {
        // Lấy component Image từ đối tượng hiện tại
        Image imageComponent = GetComponent<Image>();
        // Cập nhật sprite của Image
        imageComponent.sprite = unitAvatar;
        // Cập nhật số lượng hiển thị
        unitCount.text = uintCount.ToString();
    }
}