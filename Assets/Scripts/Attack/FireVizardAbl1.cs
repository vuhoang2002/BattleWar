using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnit : MonoBehaviour
{
    private Animator amt;
    public AbilityCount abilityCount;
    //public float timeToDealDmg=0.2f;

    void Start()
    {
        // Khởi tạo Animator
        amt = GetComponent<Animator>();
    }
    public void active_Abl_1(int basic_Atk, bool arrowDirection)
    {

        if (amt != null) // Kiểm tra Animator không null
        {
            amt.SetTrigger("isAbl1");
        }
    }
    public void active_Abl_2(int basic_Atk, bool arrowDirection)
    {

        if (amt != null) // Kiểm tra Animator không null
        {
            amt.SetTrigger("isAbl3");
        }
    }
    public void active_Abl_3(int basic_Atk, bool arrowDirection)
    {

        if (amt != null) // Kiểm tra Animator không null
        {
            amt.SetTrigger("isAbl3");
        }
    }

}