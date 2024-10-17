using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Chosen : MonoBehaviour, IPointerClickHandler
{
    // Khai báo PlayerController
    private PlayerController playerController;
    private GameObject BattleCanvas;

    void Start()
    {
    }
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;

            // Kiểm tra xem có chạm vào UI hay không
            if (IsPointerOverButton(touchPosition))
            {
                return; // Không xử lý nếu chạm vào một button
            }
            //Vector2 touchPosition = Input.GetTouch(0).position;
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                GameObject player = transform.parent.gameObject;
                Debug.Log("GameObject clicked via touch: " + gameObject.name);
                if (player.gameObject.CompareTag("Player"))
                {
                    if (player.GetComponent<PlayerController>().canChosen)
                    {
                        Collider collider = GetComponent<Collider>();
                        //isChosen = true;
                        player.GetComponent<PlayerController>().MoveCamToSelectUnit();
                        player.GetComponent<PlayerController>().Show_OrderCanva();
                        Debug.Log(" Đã chạm " + gameObject);
                    }
                }
            }
        }
    }

    private bool IsPointerOverButton(Vector2 touchPosition)
    {
        // Tạo một PointerEventData từ vị trí chạm
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = touchPosition
        };

        // Danh sách các raycast hit
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // Kiểm tra xem có button nào trong danh sách không
        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null)
            {
                return true; // Chạm vào một button
            }
        }
        return false; // Không chạm vào button nào
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.CompareTag("Player"))
        {
            if (GetComponent<PlayerController>().canChosen)
            {
                Collider collider = GetComponent<Collider>();
                //isChosen = true;
                // MoveCamToSelectUnit();
                // Show_OrderCanva();
                Debug.Log(" Đã chạm vào" + gameObject);
            }
        }
    }


}
