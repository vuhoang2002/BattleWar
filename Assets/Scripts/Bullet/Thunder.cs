using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    // Start is called before the first frame update
    Animator amtParnet;
    public int thunerDmg = 8;
    public int extraDmg = 30;
    public GameObject magicsterOrderLayout;
    public GameObject summondExplosion;// triệu hồi vụ nổ cho lightning mage
    void Start()
    {
        amtParnet = GetComponentInParent<Animator>();
        //thunerDmg = GetComponent<Attacks>().abl1_Atk;
    }

    public void ThunderAttack()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void removeBoxCoiler()
    {
        GetComponent<BoxCollider2D>().enabled = false;
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
        Object.Destroy(this.gameObject);
    }
    public void SetDamageThunder(int dmg)
    {
        thunerDmg = dmg;
    }

    public void SummondExplolison()
    {
        GameObject summondEx = Instantiate(summondExplosion, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
        summondEx.SetActive(true);
    }

}
