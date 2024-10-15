using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight_attack : MonoBehaviour
{
    private GameObject attackArea = default;
    private bool isAttacking = false; // Đổi tên từ isAtk sang isAttacking
    private float timeToAttack = 1f;
    private float timer = 0f;
    private Animator amt;
    private float attackCooldown = 1f;
    private float cooldownTimer = 0f;
    public int damage = 20;
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        amt = GetComponent<Animator>();
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0) 
        //{
        //      ("Attack");
        //     Attack();
        //     amt.SetBool("isAtk", true); // Set the boolean parameter to true
        //     cooldownTimer = attackCooldown; 
        //}

        if (isAttacking)
        {
            timer += Time.deltaTime;
            if (timer >= timeToAttack)
            {
                timer = 0;
                isAttacking = false;
                attackArea.SetActive(isAttacking);
                amt.SetBool("isAtk", false); // Set the boolean parameter to false
            }
        }
    }

    public void Attack(GameObject target)
    {
        amt.SetBool("isAtk", true); // Set the boolean parameter to true
        cooldownTimer = attackCooldown;

        // Tìm và lấy component Health của đối tượng bị tấn công
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            // Gọi hàm Damage() trên component Health của đối tượng bị tấn công
            targetHealth.TakeDamage(damage);
        }
    }
}