using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour

{
    // thông số sát thương và hồi chiêu
    public int abl_Count = 4;// tổng số lượng chiêu thức mà vị tướng đó có, max là 4
    public int basic_Atk = 1;
    public float basic_Cd = 0f;
    public float basic_Cd_Time = 0f;

    //basicAttack và ability
    public int abl1_Atk = 0;
    public float abl1_Cd = 1f;
    public int abl2_Atk = 0;
    public float abl2_Cd = 1f;

    public int abl3_Atk = 0;
    public float abl3_Cd = 1f;

    private float abl1_Cd_Time = 1f;
    private float abl2_Cd_Time = 1f;
    private float abl3_Cd_Time = 1f;

    private float basic_Atk_Range = 1f;

    private float otherDamage = 0f; //sát thương phụ khác như thiêu đốt hay sát thương + thêm cho các đơn vị lính khác
    public float timeToDealDmg = 0.5f;
    private GameObject attackArea = default;
    private bool isAttacking = false;
    //private float timeToAttack = 1f;
    //private float timer = 0f;
    private Animator amt;
    public float attackCooldown = 1f;
    public int damage = 20;
    private string CharacterName = null;
    private Ability myAbility;
    private float distanceTo_Target;
    private Health targetHealth;

    void Start()
    {
        //attackArea = transform.GetChild(0).gameObject;
        amt = GetComponent<Animator>();
        CharacterName = gameObject.name;
        //gắn thời gian chiêu thức
        basic_Cd_Time = 0;
        abl1_Cd_Time = 0;
        abl2_Cd_Time = 0;
        abl3_Cd_Time = 0;
    }

    void FixedUpdate()
    {
        //tính thời gian hồi chiêu của các chiêu thức
        CalculatedCoolDownTiming();

        // StartCoroutine(UpdateAttack());
    }

    void CalculatedCoolDownTiming()
    {
        if (basic_Cd_Time > 0)
        {
            basic_Cd_Time -= Time.deltaTime;
        }
        if (abl1_Cd_Time > 0)
        {
            abl1_Cd_Time -= Time.deltaTime;
        }
        if (abl2_Cd_Time > 0)
        {
            abl2_Cd_Time -= Time.deltaTime;
        }
        if (abl3_Cd_Time > 0)
        {
            abl3_Cd_Time -= Time.deltaTime;
        }
    }

    // các đòn tấn công còn phụ thuộc vào nhân vật hiện tại là gì và chúng sẽ sử dụng animation nào để thực hiện
    //    

    public void CallAttack(GameObject target) // yêu cầu có mục tiêu 
    {
        // Tính toán khoảng cách giữa player và enemy
        distanceTo_Target = Vector3.Distance(transform.position, target.transform.position);

        if (basic_Cd_Time <= 0)
        {
            useBasicAttack(target);
        }
    }

    void useBasicAttack(GameObject target)
    {
        // Kiểm tra xem target có component Health không
        targetHealth = target.GetComponent<Health>();
        isAttacking = true;
        if (targetHealth == null)
        {
           // Debug.LogError("Target does not have a Health component!");
            return;
        }
        // amt.SetBool("isRunning", false);
        amt.SetTrigger("isAttack"); // Kích hoạt trigger animation
      //  Debug.Log("Tấn công");
        ;// nhận st đúng thời điểm vũ khí tấn công trúng mục tiêu
       
        Invoke("BasicAttackActive", timeToDealDmg);

        // Thời gian hồi chiêu
        basic_Cd_Time = basic_Cd;
    }

    private void BasicAttackActive()
    {
        if(targetHealth!=null){//mục tiêu mà chết quá sớm sẽ ko ảnh hừng gì
        targetHealth.TakeDamage(basic_Atk);}
    }

    public void setAttack(bool value)
    {
        isAttacking = value;
    }

    public bool getAttack()
    {
        return isAttacking;
    }
}