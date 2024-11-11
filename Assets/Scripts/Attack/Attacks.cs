using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{

    // public Ability abl;
    public AbilityCount abilityCount;
    public int basic_Atk = 1;
    public float basic_Cd = 1f;
    public float basic_Cd_Time = 0f;
    //basicAttack và ability
    public int abl1_Atk = 1;
    public float abl1_Cd = 1f;
    public int abl2_Atk = 0;
    public float abl2_Cd = 1f;
    public int abl3_Atk = 0;
    public float abl3_Cd = 1f;
    public float abl1_Cd_Time = 1f;
    private float abl2_Cd_Time = 1f;
    private float abl3_Cd_Time = 1f;
    public bool isAbl1 = false;
    // private float basic_Atk_Range = 1f;
    // private float otherDamage = 0f; //sát thương phụ khác như thiêu đốt hay sát thương + thêm cho các đơn vị lính khác
    public float timeToDealDmg = 0.5f;// ko xài nx
    private GameObject attackArea = default; // ko xài nx
    private bool isAttacking = false;// ko xài nx
    private Animator amt;
    public float attackCooldown = 1f;
    private string CharacterName = null;//un use
    private Ability myAbility; // un use
    private float distanceTo_Target;// un use
    private Health targetHealth;
    private PlayerController playerController;

    bool isRightWay = true;
    bool isPlayer = true;
    public AbilityUnit abiliity; //  un use
    public int extraDmg = 5;
    // sát thương thực tế= sát thương cơ bản +(sát thương thêm <nếu mục tiêu cùng loại vs targetWeight>)
    private bool isDealExtraDamage;
    public GameObject MeleAttack;
    public WeightUnit extraWeight;
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
        playerController = GetComponent<PlayerController>();

        if (playerController == null)
        {
            isPlayer = false;
        }
        abiliity = GetComponent<AbilityUnit>();
        UnitClass extraDmgWeight = GetComponent<UnitClass>();
        extraWeight = extraDmgWeight.extraDMGWeight;
        if (MeleAttack != null)
        {
            MeleAttack.GetComponent<MeleAttack>().SetUp_MeleAttack(basic_Atk, extraDmg, extraWeight);
        }
    }

    void FixedUpdate()
    {
        CalculatedCoolDownTiming();
    }

    void Update()
    {
        //   this.isChosen=playerController.isChosen;
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
    public void CallAttack(GameObject target) // yêu cầu có mục tiêu 
    {
        if (CheckTargetUnitClass_Weight(target))
        {
            isDealExtraDamage = true;
        }
        else
        {
            isDealExtraDamage = false;
        }
        // tấn công 
        if (abl1_Cd_Time <= 0 && abiliity != null && isAbl1)
        {
            Abl1Attack();
            abl1_Cd_Time = abl1_Cd;
        }
        else if (basic_Cd_Time <= 0 && GetComponent<Shot>() != null)// dành cho đánh xa
        {
            amt.SetTrigger("isShot");
            basic_Cd_Time = basic_Cd;
            // đánh xa vẫn chạm isAbl1=true;
        }
        else if (basic_Cd_Time <= 0 && !isAbl1)
        {
            useBasicAttack(target);
        }
        else
        {
        }
    }

    void useBasicAttack(GameObject target)
    {
        if (GetComponent<Shot>() == null)
        {
            amt.SetTrigger("isAttack");
        }
        else
        {
            amt.SetTrigger("isShot");
        }
        basic_Cd_Time = basic_Cd;
        if (target != null)
        {
            targetHealth = target.GetComponent<Health>();
        }
        isAttacking = true;
    }

    private void BasicAttackActive()
    {

        //mục tiêu mà chết quá sớm sẽ ko ảnh hưởng gì
        // int damageToDeal = isDealExtraDamage ? (basic_Atk + extraDmg) : basic_Atk;
        //targetHealth.TakeDamage(damageToDeal);
        MeleAttack.GetComponent<MeleAttack>().Active_MeleeAttack();


    }

    public void setAttack(bool value)
    {
        isAttacking = value;
    }

    public bool getAttack()
    {
        return isAttacking;
    }


    public void GetAttack_byBtn(GameObject myTarget)
    {
        myTarget.GetComponent<Health>().TakeDamage(basic_Atk);
    }

    public void ShotAttack()
    {
        if (isPlayer || GetComponent<PlayerController>() != null)
        {
            isRightWay = playerController.isRightWay;
        }
        GetComponent<Shot>().Spawn_Arrow(basic_Atk, isRightWay);//extra được thiết lập ở shot r
    }

    private IEnumerator MeleeAttack(float duration)
    {
        GameObject attackArea = transform.Find("AtkArea").gameObject;
        yield return new WaitForSeconds(timeToDealDmg);
        GetComponent<PolygonCollider2D>().enabled = true;
        yield return new WaitForSeconds(duration); // Chờ trong khoảng thời gian đã chỉ định
        GetComponent<PolygonCollider2D>().enabled = false;

    }

    private void Abl1Attack()
    {
        if (isPlayer || GetComponent<PlayerController>() != null)
        {
            isRightWay = playerController.isRightWay;
        }
        GetComponent<AbilityUnit>().active_Abl_1(abl1_Atk, isRightWay);
    }
    public bool CheckTargetUnitClass_Weight(GameObject target)
    {
        UnitClass extraDmgWeight = GetComponent<UnitClass>();
        if ((extraDmgWeight.extraDMGWeight == target.GetComponent<UnitClass>().unitWeight))
        {
            return true;
        }
        return false;
    }
    public bool Get_IsDealExtraDmg()
    {
        return isDealExtraDamage;
    }
    public float SpaceToMyTarget()
    {
        float space = 0f;
        return space;
    }
    public float GetAtk1_Cd()
    {
        return abl1_Cd_Time;
    }
    public void SetAtk1_CdFullTime()
    {
        abl1_Cd_Time = abl1_Cd;
    }
    public void OffMeleAttack()
    {
        // MeleAttack.GetComponent<MeleAttack>().isActive = false;
        MeleAttack.GetComponent<MeleAttack>().Off_MeleAttack();
    }
    public void BasicAttackByBtn()
    {
        if (basic_Cd_Time <= 0)
        {
            if (GetComponent<Shot>() == null)
            {
                amt.SetTrigger("isAttack");

            }
            else
            {
                amt.SetTrigger("isShot");
                //StartCoroutine(ShotAttack());
            }
            basic_Cd_Time = basic_Cd;
        }
    }
    public void Abl1_AttackBtn()
    {
        if (abl1_Cd_Time <= 0)
        {

            amt.SetTrigger("isAbl1");


            abl1_Cd_Time = abl1_Cd;
        }
    }

    public void Abl2_AttackByBTn()
    {
        if (abl2_Cd_Time <= 0)
        {
            amt.SetTrigger("isAbl2");
            abl2_Cd_Time = abl2_Cd;
        }
    }
    public void Abl3_AttackByBTn()
    {
        if (abl3_Cd_Time <= 0)
        {
            amt.SetTrigger("isAbl3");
            abl3_Cd_Time = abl3_Cd;
        }
    }



}