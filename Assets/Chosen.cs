using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chosen : MonoBehaviour
{
    // Khai báo PlayerController
    private PlayerController playerController;
    private GameObject BattleCanvas;

    void Start()
    {
        // Tìm và gán PlayerController từ đối tượng cha
        playerController = GetComponentInParent<PlayerController>();

        if (playerController != null)
        {
           // Debug.Log("Tìm thấy PlayerController trong đối tượng cha.");
        }
        if(playerController==null)
        {
           // Debug.LogError("Không tìm thấy PlayerController trong đối tượng cha.");
        }
    }

    void OnMouseDown()
    {
        if (playerController != null && playerController.canChosen)
        {
             Debug.Log("Nhân vật đã được click bời cái thứ 2!!");
            Collider collider = GetComponent<Collider>();
            playerController.isChosen = true;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            // Duyệt qua tất cả các gameobject "Player" và thiết lập isChosen = false
            foreach (GameObject player in players)
            {
                if (player.TryGetComponent(out PlayerController pc))
                {
                    pc.canChosen = false;
                    showJoyStickCanva();
                }
            }
            // khi click thành công, đồng thời ta phải set JoyStick hiện
          
        }
        else
        {
           // Debug.LogWarning("PlayerController không tìm thấy hoặc canChosen không phải true.");
        }
    }
    // hiện JoyStickCanva
    private void showJoyStickCanva(){
          BattleCanvas=GameObject.Find("BattleCanva");
          Transform joyStickCanvaTransform = BattleCanvas.transform.Find("JoyStickCanva");
          joyStickCanvaTransform.gameObject.SetActive(true);
          // tìm order unittype
         joyStickCanvaTransform = BattleCanvas.transform.Find("OrderCanva");
         joyStickCanvaTransform = joyStickCanvaTransform.Find("PanelOrder_UnitType");
         joyStickCanvaTransform.gameObject.SetActive(true);

    }

}
