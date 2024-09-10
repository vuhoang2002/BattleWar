
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class EnemyBehavius : MonoBehaviour
{
    // Start is called before the first frame update
     public bool atk_Order = true;
     public bool def_Order = false;
     public bool fallBack_Order = false;

    //thiết kế hành vi spwan quái và hành vi của tất cả enemy
    public GameObject[] prefabList;// danh sach cac enemy
    public GameObject enemySpawnArea;
    private Vector3 maxPos;
    private Vector3 minPos;
    private GameObject[] enemies;

    void Start()
    {
        if (enemySpawnArea == null)
        {
            enemySpawnArea = GameObject.Find("EnemySpawnArea");
         }

        if (enemySpawnArea != null)
        {
            Renderer renderer= enemySpawnArea.GetComponent<Renderer>();
             maxPos= renderer.bounds.max;
             minPos=renderer.bounds.min;
             Debug.Log("Khu vuc Spawn la "+ maxPos+","+minPos);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
     // các hàm ở đây sẻ kiểm soát việc tấn công và phòng thủ của enemy
    }

    void setAllEnemyBehavius(bool atk, bool def, bool fallBack_Order)
    {
        {
            // chinh toan bo hanh vi o day
        }
    }

    public void SpawnEnemy(string enemyName_Prefab, int index)
    {
        if(index<=1){
            index=1;
        }
        // Tìm prefab theo tên (ví dụ, tìm "Minion")
        GameObject prefabToSpawn = FindPrefabByName(enemyName_Prefab);
       
        for(int i=1;i<=index; i++){
        if (prefabToSpawn != null)
        {
             float positionSpawn_X= UnityEngine.Random.Range(minPos.x, maxPos.x + 1);
             float randomY= UnityEngine.Random.Range(minPos.y, maxPos.y + 1);
            Vector3 spawnPosition = new Vector3(positionSpawn_X, randomY, 0f);
            GameObject enemySpawn=Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            enemySpawn.GetComponent<EnemyController>().setBehavius(this.atk_Order, this.def_Order, this.fallBack_Order);
        }
        }
       
    }
    // tìm kiếm enemy có tên trong list
     public GameObject FindPrefabByName(string prefabName)
    {
        foreach (GameObject prefab in prefabList)
        {
            if (prefab.name == prefabName) // So sánh tên của prefab với tên cần tìm
            {
                return prefab;
            }
        }

        Debug.LogWarning($"Không tìm thấy prefab có tên '{prefabName}' trong danh sách.");
        return null; // Trả về null nếu không tìm thấy
    }
    public void setAllEnemyBehavius(){
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var enemy in enemies){
           EnemyController ec= enemy.GetComponent<EnemyController>();
           ec.setBehavius(this.atk_Order, this.def_Order, this.fallBack_Order);
        }
    }

}
