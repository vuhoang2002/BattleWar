using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private int MAX_HEALTH = 100;

    private Animator animator;
    public GameObject deadthObject;
    public Sprite halfCastle; // castle
    public Sprite dieCastle; // for castle
    private AnimationClip deathClip;
    private float deathDuration = 1f;
    private int halfHealth;
    private SpriteRenderer spriteRenderer;
    private bool isDestroy = false;
    public SpriteRenderer currentHealthBar;

    float currentScaleX = 1.3f;
    string id;

    void Start()
    {
        MAX_HEALTH = health;
        animator = GetComponent<Animator>();
        halfHealth = health / 2;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Sửa kiểm tra null
        if (currentHealthBar == null)
        {
            currentHealthBar = transform.Find("CurentBar")?.GetComponent<SpriteRenderer>();
        }

        if (currentHealthBar != null)
        {
            currentScaleX = currentHealthBar.transform.localScale.x;
        }
       
    }

    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative damage");
        }
        this.health -= amount;

        if (halfCastle != null && dieCastle != null)
        {
            if (health <= halfHealth)
            {
                ShowHalfDead(); // for castle
            }
            if (health <= 0)
            {
                ShowDieCastle(); // for castle
                gameObject.tag = "Dead";
            }
        }
        else if (health <= 0)
        {
            health = 0;
            Die();
        }
        UpdateHealthBar();
    }

    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        // Ensure health doesn't exceed max health
        health = Mathf.Min(health + amount, MAX_HEALTH);
        UpdateHealthBar();
    }

    public void Die()
    {
       
        if (gameObject.CompareTag("Player"))
        {
            GameObject list = GameObject.Find("PUnit_List");
            UnitListManager unitListManager = list.GetComponent<UnitListManager>();
              id=GetComponent<PlayerController>().id;
            unitListManager.RemoveUnitFromTagList(gameObject.name, gameObject, id);
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            GameObject list = GameObject.Find("EUnit_List");
            EnemyBehavius enemyBehavius = list.GetComponent<EnemyBehavius>();
              id=GetComponent<EnemyController>().id;
            enemyBehavius.RemoveUnitFromTagList(gameObject.name, gameObject, id);
        }
        gameObject.tag = "Dead";
        currentHealthBar.enabled=false;
    }

    public void killSelf()
    {
        if (deadthObject != null)
        {
            deathClip = GetComponent<Animator>().runtimeAnimatorController.animationClips
                .FirstOrDefault(clip => clip.name == deadthObject.name);

            // Lấy thời lượng của animation "Knight_Dead"
            deathDuration = deathClip.length;
        }
        else
        {
            Debug.LogWarning("Chưa có dead Object");
        }

        animator.SetBool("isDead", true);
        Invoke("DeleteSelf", deathDuration);
    }

    private void DeleteSelf()
    {
        if (deadthObject != null)
        {
            GameObject instantiatedObject = Instantiate(deadthObject, transform.position, Quaternion.identity);
            instantiatedObject.transform.localScale = transform.localScale; // Đảm bảo deadthObject quay cùng hướng
        }
        Destroy(gameObject);
    }

    public void SetHealth(int healthAtThisMoment)
    {
        this.health = healthAtThisMoment;
    }

    public int getCurrentHealth()
    {
        return health;
    }

    private void ShowHalfDead()
    {
        spriteRenderer.sprite = halfCastle;
    }

    private void ShowDieCastle()
    {
        spriteRenderer.sprite = dieCastle;
    }

    private void UpdateHealthBar()
    {
        if (currentHealthBar != null)
        {
            float healthPercentage = (float)this.health / MAX_HEALTH;
            currentHealthBar.transform.localScale = new Vector3(healthPercentage * currentScaleX, 1f, 1f);
        }
    }
}