using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClicked : MonoBehaviour
{
    public GameObject minionPrefab;
    public float fixedX = 0f; // Vị trí cố định trên trục X
    public float minY = -5f; // Điểm bắt đầu trục Y
    public float maxY = 5f; // Điểm kết thúc trục Y

    void Start()
    {
        if (minionPrefab == null)
        {
            // Gán minionPrefab bằng cách tìm một GameObject có tên "Minion" trong scene
            minionPrefab = GameObject.Find("Minion");
        }
    }

    public void Update()
    {

    }

    public void OnButtonClick()
    {
        Debug.Log("Nút đã được click!");
        this.Spawn();
    }

    void Spawn()
    {
        // Tạo vị trí random trên trục Y
        float randomY = Random.Range(minY, maxY);
        // Tạo vị trí spawn ở trục X cố định
        Vector3 spawnPosition = new Vector3(fixedX, randomY, 0f);

        Instantiate(this.minionPrefab, spawnPosition, Quaternion.identity);
    }
}