using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Btn : MonoBehaviour
{
    public List<GameObject> prefabList; // Danh sách các Prefab
                                        // public float fixedX = 0f;
                                        //public float minY = -5f;
                                        //  public float maxY = 5f;
    public GameObject mapSize;
    public GameObject maxY;
    public GameObject minY;

    public GameObject leftY;
    public GameObject Player_Count;

    public float highest_Y = 2.5f;
    public float lowest_Y = -2.5f;
    public float positionSpawn_X = 2;
    public string prefabName;
    Renderer renderer ;
    PlayerCountDisplay PlayerCountDisplay;
    private bool isMaxPlayer=false;
    


    void Start()
    {
        if (prefabList == null || prefabList.Count == 0)
        {
            Debug.LogError("Danh sách prefab rỗng!");
            return;
        }

        if (mapSize == null)
        {
            mapSize = GameObject.Find("battle_Ground");
        }
        if (maxY != null)
        {
            highest_Y = maxY.transform.position.y;
        }

        if (minY != null)
        {
            lowest_Y = minY.transform.position.y;
        }

        // Lấy tọa độ X bên trái
        if (leftY != null)
        {
            positionSpawn_X = leftY.transform.position.x;
        }
        if (mapSize != null)
        {
            renderer = mapSize.GetComponent<Renderer>();
            Vector3 min = renderer.bounds.min; // Tọa độ thấp nhất
            Vector3 max = renderer.bounds.max;
            highest_Y = max.y;
            lowest_Y = min.y;
            positionSpawn_X = min.x;
            // highest_Y = GetHighestY();
            //lowest_Y = GetLowestY();
            //positionSpawn_X = GetLeftMostX();
            Debug.Log("Tọa độ spawn" + highest_Y + "," + lowest_Y + ", " + positionSpawn_X);
        }
       // Player_Count= transform.parent.transform.Find("PlayerCount").gameObject;
       // playerCount=GetComponent<PlayerCountDisplay>();
        PlayerCountDisplay = Player_Count.GetComponent<PlayerCountDisplay>();
        isMaxPlayer=PlayerCountDisplay.get_isMaxPlayer();
    }

    public void Set_Unit_Button(string prefabName_Unit)
    {
        this.prefabName = prefabName_Unit;
        //  FindPrefabByName();
        Spawn();
    }

    public void Set_PrefabName(string pbName)
    {
        this.prefabName = pbName;
    }

    // Hàm tìm prefab trong danh sách theo tên
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

    public void OnButtonClick()
    {

        Debug.Log("Nút đã được click!");
        this.Spawn();
    }

    void Spawn()
    {
        isMaxPlayer=PlayerCountDisplay.get_isMaxPlayer();
        if(!isMaxPlayer){
        // Tìm prefab theo tên (ví dụ, tìm "Minion")
        GameObject prefabToSpawn = FindPrefabByName(this.prefabName);

        if (prefabToSpawn != null)
        {
            float randomY = Random.Range(lowest_Y, highest_Y);
            Vector3 spawnPosition = new Vector3(positionSpawn_X, randomY, 0f);

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
        }else{
            Debug.Log("Số lượng Player đã đạt tối đa");
        }
    }

    float GetHighestY()
    {
        Collider collider = mapSize.GetComponent<Collider>();

        if (collider != null)
        {
            return transform.position.y + collider.bounds.size.y / 2;
        }

        Renderer renderer = mapSize.GetComponent<Renderer>();

        if (renderer != null)
        {
            return transform.position.y + renderer.bounds.size.y / 2;
        }

        return transform.position.y;
    }

    float GetLowestY()
    {
        Collider collider = mapSize.GetComponent<Collider>();

        if (collider != null)
        {
            return transform.position.y - collider.bounds.size.y / 2;
        }

        Renderer renderer = mapSize.GetComponent<Renderer>();

        if (renderer != null)
        {
            return transform.position.y - renderer.bounds.size.y / 2;
        }

        return transform.position.y;
    }

    float GetLeftMostX()
    {
        Collider collider = mapSize.GetComponent<Collider>();

        if (collider != null)
        {
            return transform.position.x - collider.bounds.size.x / 2;
        }

        Renderer renderer = mapSize.GetComponent<Renderer>();

        if (renderer != null)
        {
            return transform.position.x - renderer.bounds.size.x / 2;
        }

        return transform.position.x; // Nếu không có Collider hoặc Renderer
    }
}