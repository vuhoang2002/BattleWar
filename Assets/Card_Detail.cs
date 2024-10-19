using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        Vector3 space2 = new Vector3(transform.position.x, transform.position.y - 1f, 0);
        Vector3 space = new Vector3(2, 0, 0);
        clone1 = Instantiate(unitDataEntry.prefab, space2 - space, Quaternion.identity);
        clone1.GetComponent<PlayerController>().def_Position = space2 - space;
        clone1.AddComponent<PlayerCardControl>();
        clone1.transform.SetParent(playerList.transform);

        if (unitDataEntryBefore != null)
        {// tạo đối thủ
            clone2 = Instantiate(unitDataEntryBefore.prefab, space2 + space, Quaternion.identity);
            clone2.GetComponent<PlayerController>().Change_Prefab_To_Enemy();
            clone2.GetComponent<PlayerController>().def_Position = space2 + space;
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
