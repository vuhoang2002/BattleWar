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
    public TextMeshProUGUI unitClassDetail;
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI atk;
    public TextMeshProUGUI coid_and_cd;
    Vector3 space2;
    Vector3 space;
    GameObject playerList;

    void Start()
    {

        playerList = GameObject.Find("PlayerList");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetUp_CardDetail(string unitTagRe, string title)
    {
        unitDataEntry = unitPanelFunction.unitData.Find(unit => unit.unitTag == unitTagRe);

        if (unitDataEntry != null)
        {
            Debug.Log("unitdata bắt đầu");
            SetUp_CardHeader();
            SetUp_Prefab();
            SetUp_Infomation_Of_Deatail_Unit(title);
        }
    }
    private void SetUp_CardHeader()
    {
        cardAvatar.GetComponent<Image>().sprite = unitDataEntry.prefabSprite;
        cardName.text = unitDataEntry.unitTag;
    }
    private void SetUp_Prefab()
    {

        if (playerList != null)
        {//xóa bảng
            foreach (Transform child in playerList.transform)
            {
                Destroy(child.gameObject);
            }
        }
        space2 = new Vector3(transform.position.x, transform.position.y - 1f, 0);
        space = new Vector3(2, 0, 0);
        if (unitDataEntry.isUnit == CardType.Unit)
        {

            clone1 = Instantiate(unitDataEntry.prefab, space2 - space, Quaternion.identity);
            clone1.GetComponent<PlayerController>().def_Position = space2 - space;
            clone1.AddComponent<PlayerCardControl>();
            clone1.transform.SetParent(playerList.transform);
        }
        else if (unitDataEntry.isUnit == CardType.Enhancement)
        {
            SetUp_Enhancement(unitDataEntry.unitTag, true);
        }

        if (unitDataEntryBefore != null)
        {// tạo đối thủ
            if (unitDataEntryBefore.isUnit == CardType.Unit)
            {
                clone2 = Instantiate(unitDataEntryBefore.prefab, space2 + space, Quaternion.identity);
                clone2.GetComponent<PlayerController>().Change_Prefab_To_Enemy();
                clone2.GetComponent<PlayerController>().def_Position = space2 + space;
                clone2.AddComponent<PlayerCardControl>();
                clone2.transform.SetParent(playerList.transform);
                //
                //clone2
            }
            else if (unitDataEntryBefore.isUnit == CardType.Enhancement)
            {
                SetUp_Enhancement(unitDataEntryBefore.unitTag, false);
            }

        }

        // clone2 = clone1;
        unitDataEntryBefore = unitDataEntry;
    }

    private void SetUp_Infomation_Of_Deatail_Unit(string title)
    {
        if (unitDataEntry.isUnit == CardType.Unit)
        {
            Attacks attacks = unitDataEntry.prefab.GetComponent<Attacks>();
            UnitClass unitClass = unitDataEntry.prefab.GetComponent<UnitClass>();
            unitClassDetail.text = unitClass.unitWeight + "/" + unitClass.unitRace;
            textTitle.text = title;
            hp.text = unitDataEntry.prefab.GetComponent<Health>().health.ToString();
            atk.text = attacks.basic_Atk.ToString() + "+(" + attacks.extraDmg + changWeightToOneChar(unitClass.extraDMGWeight) + ")";
        }
        else if (unitDataEntry.isUnit == CardType.Unit)
        {

        }
        coid_and_cd.text = unitDataEntry.unitPrice.ToString() + "/" + unitDataEntry.cdTimerUnit.ToString() + "s";

    }

    private string changWeightToOneChar(WeightUnit weightUnit)
    {
        string weightChar = "Error";
        switch (weightUnit)
        {
            case WeightUnit.Light:
                weightChar = "L";
                break;
            case WeightUnit.Heavy:
                weightChar = "H";
                break;
            case WeightUnit.Rock:
                weightChar = "R";
                break;
            default:
                weightChar = "Error";
                break;
        }


        return weightChar;
    }
    private void FireUpgradeSetUp(bool player)
    {
        // chời cho đến khi clone1 khác null

        clone3 = Instantiate(unitDataEntry.prefab, space2 - space, Quaternion.identity);
        clone3.GetComponent<PlayerController>().def_Position = space2 - space;
        clone3.AddComponent<PlayerCardControl>();
        clone3.transform.SetParent(playerList.transform);
        clone3.GetComponent<FireVizardController>().OnUpgrade();
        clone1 = clone3;
        unitClassDetail.text = "Enchange";
        textTitle.text = "Nâng cấp đòn cầu lửa và đòn đánh thường của FireVizard";
        hp.text = "+0";
        atk.text = "+5Dmg";
    }
    private void SetUp_Enhancement(string unitTagEnchange, bool plauer)
    {
        if (unitTagEnchange == "FireUpgrade")
        {
            FireUpgradeSetUp(plauer);
        }
    }
}
