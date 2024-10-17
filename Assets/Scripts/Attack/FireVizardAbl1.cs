using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abl1 : MonoBehaviour
{
    // public string AblName = "FireVizard"; // Tên của khả năng
    // public GameObject fireBall; // Prefab của FireBall
    // public Transform spawnFireBall; // Vị trí để spawn FireBall
    private Animator amt;
    public string amtActiveName = "isAbl1";
    //public float timeToDealDmg=0.2f;

    void Start()
    {
        // Khởi tạo Animator
        amt = GetComponent<Animator>();



        // amtActiveName = "isShot"; // Tên trigger cho animator
    }


    // Phương thức để kích hoạt khả năng 
    public void active_Abl(int basic_Atk, bool arrowDirection)
    {

        if (amt != null) // Kiểm tra Animator không null
        {
            amt.SetTrigger(amtActiveName);
        }
    }
}