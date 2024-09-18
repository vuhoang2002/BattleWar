
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
    public Sprite halfCastle;//castle
    public Sprite dieCastle;// for castke
    private AnimationClip deathClip;
    private float deathDuration;
    private int halfHealth;
    SpriteRenderer spriteRenderer ;

    void Start()
    {
        animator = GetComponent<Animator>();
        halfHealth=health/2;
         spriteRenderer = GetComponent<SpriteRenderer>();
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

     if(halfCastle!=null && dieCastle!=null ){
            if(health<=halfHealth){
                ShowHalfDead(); //for castle
            }
            if(health<=0){
                //thua
                ShowDieCastle();// for castle
                 gameObject.tag = "Dead";


            }
        }else if (health <= 0)
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
      //  Debug.Log("Death");
       // gameObject.tag = "Dead";
       if (deadthObject != null)
{
    animator.SetBool("isDead", true);
      deathClip = GetComponent<Animator>().runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == deadthObject.name);
            

        // Lấy thời lượng của animation "Knight_Dead"
        deathDuration = deathClip.length;
}
else
{
    Debug.LogWarning("Chưa có dead Object");
}

      

        // In thời lượng animation ra console
       // Debug.Log("Death animation duration: " + deathDuration + " seconds");

        // Lập lịch để xóa GameObject hiện tại sau khi animation "Knight_Dead" kết thúc
        Invoke("DeleteSelf", deathDuration);

        // Instantiate the "deadthObject" at the same position và quay cùng hướng
    }

    private void DeleteSelf()
    {
       
        if(deadthObject!=null){
        GameObject instantiatedObject = Instantiate(deadthObject, transform.position, Quaternion.identity);
        instantiatedObject.transform.localScale = transform.localScale; // Đảm bảo deadthObject quay cùng hướng
         Destroy(gameObject);
    }else{
         Destroy(gameObject);
    }
    }

    public void SetHealth(int healthAtThisMoment)
    {
        this.health = healthAtThisMoment;
    }

    public int getCurrentHealth()
    {
        return health;
    }
    private void ShowHalfDead(){
        spriteRenderer.sprite = halfCastle;
    }
    private void  ShowDieCastle(){
          spriteRenderer.sprite = dieCastle;
    }
}