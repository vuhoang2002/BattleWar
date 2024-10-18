using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Card_Detail : MonoBehaviour
{
    // Start is called before the first frame update
    public UnitPanelFunction unitPanelFunction;// thông tin của unit
    public GameObject cardAvatar;
    public TextMeshProUGUI cardName;
    private UnitData unitDataEntry;
    private static UnitData unitDataEntryBefore;
    GameObject clone1;
    GameObject clone2;
    static GameObject clone3;
    void Start()
    {
        // unitData = unitPanelFunction.unitData;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetUp_CardDetail(string unitTagRe)
    {
        unitDataEntry = unitPanelFunction.unitData.Find(unit => unit.unitTag == unitTagRe);

        if (unitDataEntry != null)
        {
            Debug.Log("unitdata bắt đầu");
            SetUp_CardHeader();
            SetUp_Prefab();
        }
    }
    private void SetUp_CardHeader()
    {
        cardAvatar.GetComponent<Image>().sprite = unitDataEntry.prefabSprite;
        cardName.text = unitDataEntry.unitTag;
    }
    private void SetUp_Prefab()
    {
        GameObject playerList = GameObject.Find("PlayerList");
        if (playerList != null)
        {//xóa bảng
            foreach (Transform child in playerList.transform)
            {
                Destroy(child.gameObject);
            }
        }
        clone1 = Instantiate(unitDataEntry.prefab, new Vector3(4.37f, -0.7f, 0f), Quaternion.identity);
        clone1.GetComponent<PlayerController>().def_Position = new Vector3(4.37f, -0.7f, 0f);
        clone1.AddComponent<PlayerCardControl>();
        clone1.transform.SetParent(playerList.transform);

        if (unitDataEntryBefore != null)
        {// tạo đối thủ
            clone2 = Instantiate(unitDataEntryBefore.prefab, new Vector3(8.24f, -0.7f, 0f), Quaternion.identity);
            clone2.GetComponent<PlayerController>().Change_Prefab_To_Enemy();
            clone2.GetComponent<PlayerController>().def_Position = new Vector3(8.24f, -0.7f, 0f);
            clone2.AddComponent<PlayerCardControl>();
            clone2.transform.SetParent(playerList.transform);
            //
            //clone2

        }

        // clone2 = clone1;
        unitDataEntryBefore = unitDataEntry;
    }
    // Thiết lập card spell
    private void SetUpSpellCard(string nameCard)
    {
        switch (nameCard)
        {
            case "FireUpgrade":
                FireUpgradeSetUp();
                break;
            default:

                break;
        }
    }
    private void FireUpgradeSetUp()
    {
        // chời cho đến khi clone1 khác null
    }
}
