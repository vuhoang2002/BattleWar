using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    // thông số sát thương và hồi chiêu
    public int abl_Count = 4;// tổng số lượng chiêu thức mà vị tướng đó có, max là 4
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

    private float otherDamage = 0f; //sát thương phụ khác như thiêu đốt hay sát thương + thêm cho các đơn vị lính khác
    public float timeToDealDmg = 0.5f;
    private GameObject attackArea = default;
    private bool isAttacking = false;
    //private float timeToAttack = 1f;
    //private float timer = 0f;
    private Animator amt;
    public float attackCooldown = 1f;
    //public int damage = 20;
    private string CharacterName = null;
    private Ability myAbility;
    private float distanceTo_Target;
    private Health targetHealth;
    private PlayerController playerController;
    private EnemyController enemyController;
    // bool isChosen=false;
    bool isRightWay = true;
    bool isPlayer = true;
    public Abl1 abl1;
    public int extraDmg = 5;
    // sát thương thực tế= sát thương cơ bản +(sát thương thêm <nếu mục tiêu cùng loại vs targetWeight>)
    private bool isDealExtraDamage;
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
        enemyController = GetComponent<EnemyController>();
        if (playerController == null)
        {
            isPlayer = false;
        }
        abl1 = GetComponent<Abl1>();

        // timeToDealDmg*=Time.deltaTime;
        //this.isChosen=playerController.isChosen;

    }

    void FixedUpdate()
    {
        //tính thời gian hồi chiêu của các chiêu thức
        CalculatedCoolDownTiming();

        // StartCoroutine(UpdateAttack());
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

    // các đòn tấn công còn phụ thuộc vào nhân vật hiện tại là gì và chúng sẽ sử dụng animation nào để thực hiện
    //    

    public void CallAttack(GameObject target) // yêu cầu có mục tiêu 
    {
        // Tính toán khoảng cách giữa player và enemy
        // distanceTo_Target = Vector3.Distance(transform.position, target.transform.position);

        // ("Weight mục tiêu là " + target.GetComponent<UnitClass>().unitWeight + "Của tôi là: " + GetComponent<UnitClass>().extraDMGWeight);
        if (CheckTargetUnitClass_Weight(target))
        {
            isDealExtraDamage = true;
        }
        else
        {
            isDealExtraDamage = false;
        }
        // tấn công 
        if (abl1_Cd_Time <= 0 && abl1 != null && isAbl1)
        {
            Abl1Attack();
            abl1_Cd_Time = abl1_Cd;
        }
        else if (basic_Cd_Time <= 0 && GetComponent<Shot>() != null)
        {
            amt.SetTrigger("isShot");

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
        // Kiểm tra xem target có component Health không

        // amt.SetBool("isRunning", false);
        //CheckTargetUnitClass_Weight();
        if (GetComponent<Shot>() == null)
        {
            amt.SetTrigger("isAttack");
            // Invoke("BasicAttackActive", timeToDealDmg); // nhận st đúng thời điểm vũ khí tấn công trúng mục tiêu
        }
        else
        {
            //isRightWay=playerController.isRightWay;
            amt.SetTrigger("isShot");

        }
        // Thời gian hồi chiêu
        basic_Cd_Time = basic_Cd;
        if (target != null)
        {
            targetHealth = target.GetComponent<Health>();

        }
        isAttacking = true;
    }

    private void BasicAttackActive()
    {
        if (targetHealth != null)
        {//mục tiêu mà chết quá sớm sẽ ko ảnh hừng gì

            int damageToDeal = isDealExtraDamage ? (basic_Atk + extraDmg) : basic_Atk;
            targetHealth.TakeDamage(damageToDeal);
        }
    }

    public void setAttack(bool value)
    {
        isAttacking = value;
    }

    public bool getAttack()
    {
        return isAttacking;
    }

    public void AttackByButton()
    {
        if (basic_Cd_Time <= 0)
        {
            if (GetComponent<Shot>() == null)
            {
                amt.SetTrigger("isAttack");
                // StartCoroutine(MeleeAttack(0.2f));
            }
            else
            {
                amt.SetTrigger("isShot");
                //StartCoroutine(ShotAttack());
            }
            basic_Cd_Time = basic_Cd;
        }
    }

    public void GetAttack_byBtn(GameObject myTarget)
    {
        myTarget.GetComponent<Health>().TakeDamage(basic_Atk);
    }

    public void ShotAttack()
    {
        //yield return new WaitForSeconds(timeToDealDmg);
        //spawn ra cung tên
        if (isPlayer || GetComponent<PlayerController>() != null)
        {
            isRightWay = playerController.isRightWay;
        }
        else if (!isPlayer || GetComponent<EnemyController>() != null)
        {
            isRightWay = enemyController.isRightWay;
        }
        // int damageToDeal = isDealExtraDamage ? (basic_Atk + extraDmg) : basic_Atk;
        GetComponent<Shot>().Spawn_Arrow(basic_Atk, isRightWay);//extra được thiết lập ở shot r

        //("Collider đã được tắt!");
    }

    private IEnumerator MeleeAttack(float duration)
    {//duration là thơi gian colider tồn tại

        GameObject attackArea = transform.Find("AtkArea").gameObject;

        yield return new WaitForSeconds(timeToDealDmg);
        //attackArea.SetActive(true); // Tắt collider
        GetComponent<PolygonCollider2D>().enabled = true;
        yield return new WaitForSeconds(duration); // Chờ trong khoảng thời gian đã chỉ định
        //attackArea.SetActive(false);
        GetComponent<PolygonCollider2D>().enabled = false;

    }

    private void Abl1Attack()
    {
        //  yield return new WaitForSeconds(0f);
        //spawn ra cung tên
        if (isPlayer || GetComponent<PlayerController>() != null)
        {
            isRightWay = playerController.isRightWay;
        }
        else if (!isPlayer || GetComponent<EnemyController>() != null)
        {
            isRightWay = enemyController.isRightWay;
        }
        GetComponent<Abl1>().active_Abl(abl1_Atk, isRightWay);

        //("Collider đã được tắt!");
    }
    bool CheckTargetUnitClass_Weight(GameObject target)
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
}