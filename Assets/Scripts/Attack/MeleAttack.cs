using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleAttack : MonoBehaviour
{
    public int damage;
    public int extraDmg;
    public WeightUnit myWeightExtra;
    public bool isActive = false;
    public SpeardDamage speardDamage;
    public byte spreadDamageCount;
    public byte spreadDamageCount_Time;
    public GameObject target;
    public bool isDestroyAfterCollding = false;// áp dụng cho tuyêt chiêu là bản sao

    void Start()
    {
        spreadDamageCount_Time = spreadDamageCount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        target = other.gameObject;
        if (!isActive) return;

        Health health = other.gameObject.GetComponent<Health>();
        if (health != null)
        {
            int totalDamage = damage + (CheckTargetUnitClass_Weight(other.gameObject, myWeightExtra) ? extraDmg : 0);
            health.TakeDamage(totalDamage);

            if (speardDamage == SpeardDamage.NormalAttack)// đòn đánh thương gây 1 lần sát thương
            {
                gameObject.SetActive(false);
                isActive = false; // Đánh dấu là không còn hoạt động
            }
            else if (speardDamage == SpeardDamage.HaveCount)
            {
                if (spreadDamageCount_Time > 1)// số mục tiêu tối đa chịu sát thương lan do thực hiện trc 1 lần hàm nên lần này ta chỉnh là 1
                {
                    spreadDamageCount_Time--;
                }
                else
                {
                    gameObject.SetActive(false);
                    isActive = false;
                    //spreadDamageCount_Time = spreadDamageCount;
                }
            }
            // còn lại là Ability gây st lan cho toàn bộ mục tiêu(pháp sư)
            if (isDestroyAfterCollding)
            {
                if (GetComponent<Animator>() != null)
                {
                    GetComponent<Animator>().SetBool("Destroy", true);
                }
                Destroy(gameObject);
            }
        }
    }

    public void SetUp_MeleAttack(int damage, int extraDmg, WeightUnit weightUnitExtra)
    {
        this.damage = damage;
        this.extraDmg = extraDmg;
        this.myWeightExtra = weightUnitExtra;
    }

    private bool CheckTargetUnitClass_Weight(GameObject target, WeightUnit extraDmgWeight)
    {
        UnitClass targetUnitClass = target.GetComponent<UnitClass>();
        return targetUnitClass != null && extraDmgWeight == targetUnitClass.unitWeight;
    }
    public void OnDestroyMeleAttack()
    {
        Object.Destroy(this.gameObject);
    }
    public void Off_MeleAttack()
    {

        gameObject.SetActive(false);

    }
    public void Active_MeleeAttack()
    {
        spreadDamageCount_Time = spreadDamageCount;
        gameObject.SetActive(true);
        isActive = true;

    }

}