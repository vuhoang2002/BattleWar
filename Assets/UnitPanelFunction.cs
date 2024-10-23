using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanelFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public List<UnitData> unitData;
    public List<string> selectedUnitTags;
    public GameObject UnitPanel;
    public GameObject cloneSpawnBtn;
    public GameObject mapSize;
    private Level_Controller levelWarMod;
    public GameMod mod;

    void Start()
    {
        GetSaveUnit();// lấy unit trong bộ bài
        Create_Spawn_Button();

    }
    void Awake()
    {
        GameObject gameManager = GameObject.Find("GAME_MANAGER");
        levelWarMod = gameManager.GetComponentInChildren<Level_Controller>();
        if (levelWarMod.gameMod == GameMod.War)
        {
            levelWarMod.OnGameModeChanged_War += HandleGameModeChanged_War;
            mod = GameMod.War;
        }
    }

    private void HandleGameModeChanged_War()
    {
        //throw new NotImplementedException();
        mod = GameMod.War;
    }


    // Update is called once per frame
    void GetSaveUnit()
    {
        selectedUnitTags = new List<string>();
        LoadSelectedUnits();
    }

    public void LoadSelectedUnits()
    {
        string path = Application.persistentDataPath + "/savedata.dat";
        //     ("Dữ liệu được lưu ở: " + path);
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
    {// dựa trên các thẻ đã lưu, tạo button
        foreach (var save_unit in selectedUnitTags)
        {
            Create_Current_Button(save_unit);
        }
    }

    private void Create_Current_Button(string save_unit)
    {
        // Tìm UnitData tương ứng với unitTag
        UnitData unitDataEntry = unitData.Find(unit => unit.unitTag == save_unit);
        Debug.LogWarning("save là" + save_unit);
        if (unitDataEntry != null)
        {
            Debug.LogWarning("save có tồn tại" + unitDataEntry.unitTag);

            // Tạo nút từ prefab đã tạo sẵn (nếu có) hoặc tạo mới
            // Tạo GameObject cho nút
            GameObject crtSpawBtn = new GameObject();

            // thiết lập nút
            crtSpawBtn = Instantiate(cloneSpawnBtn, transform); // Spawn prefab
            SpawnPlayer sp = crtSpawBtn.GetComponent<SpawnPlayer>();
            sp.setUpButton(unitDataEntry.prefab, unitDataEntry.prefabSprite, mapSize, unitDataEntry.unitPrice, unitDataEntry.cdTimerUnit, mod);

            crtSpawBtn.name = "Spawn" + unitDataEntry.unitTag;
            Debug.LogWarning("save đã spawn" + crtSpawBtn.name);
            // crtSpawBtn.setUpButton(unitDataEntry.prefab, unitDataEntry.prefabSprite, mapSize,  0);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy UnitData cho unitTag: " + save_unit);
        }
    }

    private void OnButtonClick(UnitData unitDataEntry)
    {
        // Xử lý logic khi nút được nhấn, ví dụ:
        // Có thể thêm logic để spawn hoặc thực hiện hành động khác với unitDataEntry
    }

    public int Get_UnitListCount()
    {
        return selectedUnitTags.Count;
    }
    public List<string> SelectedUnitTags
    {
        get { return selectedUnitTags; }
    }
}