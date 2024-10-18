using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro; // Đảm bảo bạn đã sử dụng TextMeshPro

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
    public GameObject damePopUpPrefab;
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
        // damePopUpPrefab = Resources.Load<GameObject>("DamePopUp");
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
        ShowDamagePopup(amount);
        // MÁU CHO CASTLE 
        if (halfCastle != null && dieCastle != null)
        {
            if (health <= halfHealth)
            {
                ShowHalfDead(); // for castle
            }
            if (health <= 0)
            {
                ShowDieCastle(); // for castle
                                 // Tìm BattleCanva
                GameObject battleCanva = GameObject.Find("BattleCanva");

                if (battleCanva != null)
                {
                    if (gameObject.CompareTag("PlayerCastle"))
                    {
                        // Tìm LoseUI trong BattleCanva
                        GameObject loseUI = battleCanva.transform.Find("LoseUI")?.gameObject;

                        if (loseUI != null)
                        {
                            loseUI.SetActive(true);
                            Time.timeScale = 0f;
                        }
                        else
                        {
                            // Debug.LogWarning("LoseUI not found in BattleCanva!");
                        }
                    }
                    else if (gameObject.CompareTag("EnemyCastle"))
                    {
                        // Tìm VictoryUI
                        GameObject victoryUI = battleCanva.transform.Find("VictoryUI")?.gameObject;

                        if (victoryUI != null)
                        {
                            victoryUI.SetActive(true);
                            Time.timeScale = 0f;
                        }
                        else
                        {
                            // Debug.LogWarning("VictoryUI not found!");
                        }
                    }

                    // Đánh dấu lâu đài là đã chết
                    gameObject.tag = "Dead";
                }
                else
                {
                    // Debug.LogWarning("BattleCanva not found!");
                }
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
        if (gameObject.GetComponent<SummondToken>() != null)
        {
            //nếu đối tượng là 1 vật thể triệu hồi
            killSelf();
            return;
        }
        if (GetComponent<PlayerCardControl>().isShowOnCard)
        {
            killSelf();
            return;
        }
        if (gameObject.CompareTag("Player"))
        {
            GameObject list = GameObject.Find("PUnit_List");
            UnitListManager unitListManager = list.GetComponent<UnitListManager>();
            id = GetComponent<PlayerController>().id;
            unitListManager.RemoveUnitFromTagList(gameObject.name, gameObject, id);
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            GameObject list = GameObject.Find("EUnit_List");
            EnemyBehavius enemyBehavius = list.GetComponent<EnemyBehavius>();
            id = GetComponent<PlayerController>().id;
            enemyBehavius.RemoveUnitFromTagList(gameObject.name, gameObject, id);
        }
        gameObject.tag = "Dead";
        currentHealthBar.enabled = false;
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
            GetComponent<Renderer>().sortingOrder = GetComponent<PlayerController>().GetOrderLayer();
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

    // Kiểm tra prefab không null
    public void ShowDamagePopup(int damage)
    {
        if (damePopUpPrefab != null)
        {
            // Tạo instance của damage pop-up
            GameObject instance = Instantiate(damePopUpPrefab, transform.position, Quaternion.identity);

            // Đặt instance là con của đối tượng hiện tại
            instance.transform.SetParent(transform);

            // Lấy component TextMeshPro từ instance
            TextMeshPro textComponent = instance.GetComponentInChildren<TextMeshPro>();

            if (textComponent != null)
            {
                textComponent.text = damage.ToString();
            }
            else
            {
                Debug.LogWarning("TextMeshPro component not found in the pop-up instance!");
            }

            // Bắt đầu animation (nếu cần)
            // Animator animator = instance.GetComponent<Animator>();

            //  if (animator != null)
            // {
            //   animator.SetTrigger("PlayAnimation"); // Trigger đã tạo trong Animator
            // }
        }
        else
        {
            //.LogWarning("Damage pop-up prefab is not assigned!");
        }
    }
    public void Heal_Full_Hp()
    {
        health = MAX_HEALTH;
    }
}