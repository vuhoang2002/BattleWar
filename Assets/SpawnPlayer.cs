using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayer : MonoBehaviour
{
    public Sprite unitAvatar; // Hình đại diện cho đơn vị
    //public Transform objectLocation; // Vị trí spawn
    public GameObject mapSize; // Kích thước của bản đồ
    public GameObject prefabToSpawn; // Prefab để spawn
    public int priceUnit; // Giá tiền

    private Renderer renderer;
    private PlayerCountDisplay playerCountDisplay; // Tham chiếu đến PlayerCountDisplay
    private bool isMaxPlayer = false;
    private float highest_Y;
    private float lowest_Y;
    private float positionSpawn_X;
    public UnitListManager unitListManager;

    void Start()
    {
        playerCountDisplay = FindObjectOfType<PlayerCountDisplay>(); // Lấy tham chiếu đến PlayerCountDisplay
        isMaxPlayer = playerCountDisplay.get_isMaxPlayer();
         this.mapSize = mapSize;
         setSpawnLocation(); // Gọi hàm để thiết lập vị trí spawn
          GetComponent<Image>().sprite = unitAvatar; 
          if(unitListManager==null){
            GameObject ob= GameObject.Find("PUnit_List");
            unitListManager=ob.GetComponent<UnitListManager>();
          }
    }
    
    public void OnMouseDown()
    {
        spawnUnit(); // Gọi hàm spawn khi bấm chuột
    }

    public void setUpButton(GameObject unitToSpawn, Sprite unitAvatar, GameObject mapSize,  int price)
    {
        gameObject.SetActive(true);
        // Thiết lập nút cho đơn vị để spawn
        this.prefabToSpawn = unitToSpawn; // Gán prefab để spawn
        this.unitAvatar = unitAvatar; // Gán hình đại diện
        GetComponent<Image>().sprite = unitAvatar; // Thiết lập hình đại diện cho UI
        //transform.position = objectLocation.position; // Thiết lập vị trí button
        this.mapSize = mapSize; // Gán kích thước bản đồ
        this.priceUnit = price; // Gán giá
        setSpawnLocation(); // Gọi hàm để thiết lập vị trí spawn
    }

    private void spawnUnit()
    {
        // Thiết lập vị trí spawn ngẫu nhiên
        float positionSpawn_Y = Random.Range(lowest_Y, highest_Y); 
        isMaxPlayer = playerCountDisplay.get_isMaxPlayer(); // Lấy thông tin về số lượng người chơi hiện tại

        if (!isMaxPlayer)
        {
            if (prefabToSpawn != null)
            {
                Vector3 spawnPosition = new Vector3(positionSpawn_X, positionSpawn_Y, 0f);
                GameObject newUnit=Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity); // Spawn prefab
                 unitListManager.AddUnitToTagList(prefabToSpawn.name,newUnit);
            }
           
        }
        else
        {
            Debug.Log("Số lượng Player đã đạt tối đa");
        }
    }

    private void setSpawnLocation()
    {
       
            renderer = mapSize.GetComponent<Renderer>();
            Vector3 min = renderer.bounds.min; // Tọa độ thấp nhất
            Vector3 max = renderer.bounds.max; // Tọa độ cao nhất
            highest_Y = max.y; // Lấy tọa độ Y cao nhất
            lowest_Y = min.y; // Lấy tọa độ Y thấp nhất
            positionSpawn_X = min.x; // Lấy tọa độ X thấp nhất
       
    }
}