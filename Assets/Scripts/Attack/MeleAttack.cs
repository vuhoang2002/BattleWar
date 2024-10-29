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


    void OnTriggerEnter2D(Collider2D other)
    {
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
                if (spreadDamageCount > 0)// số mục tiêu tối đa chịu sát thương lan
                {
                    spreadDamageCount--;
                }
                else
                {
                    gameObject.SetActive(false);
                    isActive = false;
                }
            }
            // còn lại là Ability gây st lan cho toàn bộ mục tiêu(pháp sư)
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
        gameObject.SetActive(true);
        isActive = true;
    }

}