using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClicked : MonoBehaviour
{
    public GameObject minionPrefab;
    public float fixedX = 0f; // Vị trí cố định trên trục X
    public float minY = -5f; // Điểm bắt đầu trục Y
    public float maxY = 5f; // Điểm kết thúc trục Y
    public GameObject abc;
    void Start()
    {
        if (minionPrefab == null)
        {
            // Gán minionPrefab bằng cách tìm một GameObject có tên "Minion" trong scene
            minionPrefab = GameObject.Find("Minion");
        }
    }
    public void OnMouseDown(){
            abc.GetComponent<FomationManager_NewUprade>().Create_defPosition();
           
    }
}