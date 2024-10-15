using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    // Start is called before the first frame update
    Animator amtParnet;
    public int thunerDmg = 8;
    public int extraDmg = 30;
    void Start()
    {
        amtParnet = GetComponentInParent<Animator>();
        //thunerDmg = GetComponent<Attacks>().abl1_Atk;
    }

    public void ThunderAttack()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            if (other.GetComponent<UnitClass>().unitWeight != WeightUnit.Rock)
            {
                targetHealth.TakeDamage(thunerDmg);
            }
            else
            {
                targetHealth.TakeDamage(thunerDmg + extraDmg);
            }
            GetComponent<BoxCollider2D>().enabled = false;
        }
        // ("Va chạm với " + other);
    }
    void OnDestroyThis()
    {
        Transform parentTransform = transform.parent;
        Object.Destroy(parentTransform.gameObject);
    }
    public void SetDamageThunder(int dmg)
    {
        thunerDmg = dmg;
    }
}
