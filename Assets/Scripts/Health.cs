using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Thêm dòng này

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private int MAX_HEALTH = 100;

    private Animator animator;
    public GameObject deadthObject;
    private AnimationClip deathClip;
    private float deathDuration;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Damage(10);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Heal(10);
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
        }
        this.health -= amount;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }
        // Ensure health doesn't exceed max health
        if (health + amount > MAX_HEALTH)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }
    }

    public void Die()
    {
        Debug.Log("Death");
        gameObject.tag = "Dead";

        animator.SetBool("isDead", true);

        deathClip = GetComponent<Animator>().runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == deadthObject.name);

        // Lấy thời lượng của animation "Knight_Dead"
        deathDuration = deathClip.length;

        // In thời lượng animation ra console
        Debug.Log("Death animation duration: " + deathDuration + " seconds");

        // Lập lịch để xóa GameObject hiện tại sau khi animation "Knight_Dead" kết thúc
        Invoke("DeleteSelf", deathDuration);

        // Instantiate the "deadthObject" at the same position và quay cùng hướng
    }

    private void DeleteSelf()
    {
        Destroy(gameObject);

        GameObject instantiatedObject = Instantiate(deadthObject, transform.position, Quaternion.identity);
        instantiatedObject.transform.localScale = transform.localScale; // Đảm bảo deadthObject quay cùng hướng
    }

    public void SetHealth(int healthAtThisMoment)
    {
        this.health = healthAtThisMoment;
    }

    public int getCurrentHealth()
    {
        return health;
    }
}