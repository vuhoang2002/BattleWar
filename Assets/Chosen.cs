using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chosen : MonoBehaviour
{
    // Khai báo PlayerController
    private PlayerController playerController;

    void Start()
    {
        // Tìm và gán PlayerController từ đối tượng cha
        playerController = GetComponentInParent<PlayerController>();

        if (playerController != null)
        {
            Debug.Log("Tìm thấy PlayerController trong đối tượng cha.");
        }
        if(playerController==null)
        {
            Debug.LogError("Không tìm thấy PlayerController trong đối tượng cha.");
        }
    }

    void OnMouseDown()
    {
        Debug.Log("Nhân vật đã được click bời cái thứ 2!!");

        if (playerController != null && playerController.canChosen)
        {
            Collider collider = GetComponent<Collider>();
            playerController.isChosen = true;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            // Duyệt qua tất cả các gameobject "Player" và thiết lập isChosen = false
            foreach (GameObject player in players)
            {
                if (player.TryGetComponent(out PlayerController pc))
                {
                    pc.canChosen = false;
                }
            }
        }
        else
        {
            Debug.LogWarning("PlayerController không tìm thấy hoặc canChosen không phải true.");
        }
    }
}
