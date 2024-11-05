using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

// [Serializable]
// public class SpawnButtonInfo
// {
//     public Vector2 position; // Tọa độ
//     public GameObject button; // Tham chiếu đến nút

//     public SpawnButtonInfo(GameObject btn, Vector2 pos)
//     {
//         position = pos;
//         button = btn;
//     }
// }

public class UnitPanelFunction : MonoBehaviour
{
    public List<UnitData> unitData;
    public List<string> selectedUnitTags;
    public GameObject UnitPanel;
    public GameObject cloneSpawnBtn;
    public GameObject mapSize;
    private Level_Controller levelWarMod;
    public GameMod mod;
    public List<SpawnButtonInfo> spawnBtnInfoList; // Thay đổi kiểu dữ liệu

    void Start()
    {
        spawnBtnInfoList = new List<SpawnButtonInfo>();
        GetSaveUnit(); // lấy unit trong bộ bài
        Create_Spawn_Button();
    }

    void Awake()
    {
        GameObject gameManager = GameObject.Find("GAME_MANAGER");
        if (gameManager != null)
        {
            levelWarMod = gameManager.GetComponentInChildren<Level_Controller>();
            if (levelWarMod.gameMod == GameMod.War)
            {
                levelWarMod.OnGameModeChanged_War += HandleGameModeChanged_War;
                mod = GameMod.War;
            }
        }
    }

    private void HandleGameModeChanged_War()
    {
        mod = GameMod.War;
    }

    void GetSaveUnit()
    {
        selectedUnitTags = new List<string>();
        LoadSelectedUnits();
    }

    public void LoadSelectedUnits()
    {
        string path = Application.persistentDataPath + "/savedata.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            selectedUnitTags = data.selectedUnitTags; // Khôi phục danh sách thẻ đơn vị đã chọn
        }
    }

    private void Create_Spawn_Button()
    {
        foreach (var save_unit in selectedUnitTags)
        {
            Create_Current_Button(save_unit);
        }
        //StartCoroutine(SetUp_SpawnBtnLocation_List());
        SetUp_SpawnBtnLocation_List();
    }

    public void SetUp_SpawnBtnLocation_List()
    {
        // yield return new WaitForEndOfFrame();
        // Cập nhật spawnBtnLocation với danh sách SpawnButtonInfo
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnBtnInfoList[i].position = transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
        }
    }
    public void CouroutineTime()
    {
        StartCoroutine(SetUp_SpawnBtnLocation_List2());
    }
    public IEnumerator SetUp_SpawnBtnLocation_List2()//có biến bool truyền vào lấy cả 
    {

        yield return new WaitForSeconds(1f);
        // yeild return new WaitForEndOfFrame();
        for (int i = 0; i < transform.childCount; i++)
        {

            spawnBtnInfoList[i].button = transform.GetChild(i).gameObject;
            spawnBtnInfoList[i].position = spawnBtnInfoList[i].button.GetComponent<RectTransform>().anchoredPosition;
        }

    }

    public void Create_Current_Button(string save_unit)
    {
        UnitData unitDataEntry = unitData.Find(unit => unit.unitTag == save_unit);
        Debug.LogWarning("save là" + save_unit);
        if (unitDataEntry != null)
        {
            Debug.LogWarning("save có tồn tại" + unitDataEntry.unitTag);

            GameObject crtSpawBtn = Instantiate(cloneSpawnBtn, transform); // Spawn prefab
            SpawnPlayer sp = crtSpawBtn.GetComponent<SpawnPlayer>();
            sp.setUpButton(unitDataEntry.prefab, unitDataEntry.prefabSprite, mapSize, unitDataEntry.unitPrice, unitDataEntry.cdTimerUnit, mod);

            crtSpawBtn.name = unitDataEntry.unitTag + "_Spawner";
            Debug.LogWarning("save đã spawn" + crtSpawBtn.name);

            // Lưu thông tin nút vào spawnBtnLocation
            Vector2 btnPosition = crtSpawBtn.GetComponent<RectTransform>().anchoredPosition; // Lấy vị trí của nút
            SpawnButtonInfo buttonInfo = new SpawnButtonInfo(crtSpawBtn, btnPosition);
            spawnBtnInfoList.Add(buttonInfo);

        }
        else
        {
            Debug.LogWarning("Không tìm thấy UnitData cho unitTag: " + save_unit);
        }
    }

    // Phương thức để thêm thông tin vào mảng (tùy chọn)
    private SpawnButtonInfo[] AddToArray(SpawnButtonInfo[] array, SpawnButtonInfo newItem)
    {
        SpawnButtonInfo[] newArray = new SpawnButtonInfo[array.Length + 1];
        Array.Copy(array, newArray, array.Length);
        newArray[array.Length] = newItem;
        return newArray;
    }

    private void OnButtonClick(UnitData unitDataEntry)
    {
        // Xử lý logic khi nút được nhấn
    }

    public int Get_UnitListCount()
    {
        return selectedUnitTags.Count;
    }

    public List<string> SelectedUnitTags
    {
        get { return selectedUnitTags; }
    }

    public Sprite Find_UnitAvatar(string unitTagRef)
    {
        int index = unitTagRef.IndexOf('(');
        if (index > 0)
        {
            unitTagRef = unitTagRef.Substring(0, index).Trim();
        }
        UnitData unitDataEntry = unitData.Find(unit => unit.unitTag == unitTagRef);
        return unitDataEntry.prefabSprite;
    }
}