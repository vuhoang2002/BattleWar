using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummondToken : MonoBehaviour
{
    public GameObject masterOfToken;
    private string numberType = "O";
    PlayerController pl;
    PlayerController plMaster;
    float randomValue;


    // Start is called before the first frame update
    void Start()
    {
        pl = GetComponent<PlayerController>();
        plMaster = masterOfToken.GetComponent<PlayerController>();
        pl.Set_Retreat_Position(plMaster.retreat_Position);
        SetNumberType();
        if (masterOfToken.CompareTag("Enemy"))
        {
            GetComponent<PlayerController>().Change_Prefab_To_Enemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        pl.Set_DefPosition(plMaster.def_Position + new Vector3(randomValue, 0, 0));
        pl.SetBehavius(plMaster.isAtk_Order, plMaster.isDef_Order, plMaster.isFallBack_Order, plMaster.isHold_Order);
        if (pl.isHold_Order)
        {
            pl.hold_Position = plMaster.hold_Position + new Vector3(randomValue, 0, 0);
        }
    }
    public void iAmDeadMaster()
    {
        masterOfToken.GetComponent<Skl_Mage_Controller>().summondCount--;
    }
    public void SetNumberType()
    {
        //numberType = Ab;
        if (masterOfToken.GetComponent<Skl_Mage_Controller>().summondCount == 1)
        {
            numberType = "A";
            randomValue = 0.5f;

            //pl.SetID(masterOfToken.GetComponent<PlayerController>().id + numberType);
        }
        else if (masterOfToken.GetComponent<Skl_Mage_Controller>().summondCount == 2)
        {
            numberType = "B";
            randomValue = -0.5f;
            //  pl.SetID(masterOfToken.GetComponent<PlayerController>().id + numberType);
        }
        pl.SetID(masterOfToken.GetComponent<PlayerController>().id + numberType);
    }
}
