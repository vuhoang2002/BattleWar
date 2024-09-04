using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject enemy;
    public float speed = 1f;
    private Animator animator;
    private bool isColliding = false;
    private float previousEnemyPositionX;
    private float attackRange = 5f; // Khoảng cách tấn công
    private float attackCooldown = 2f; // Thời gian làm mát tấn công (2 giây)
    private float lastAttackTime = 0f;
    private knight_attack knightAttack;
    private bool isEnemyAlive = true; // Biến theo dõi trạng thái của Enemy

    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy == null)
        {
            Debug.LogError("Enemy GameObject not found!");
        }
        previousEnemyPositionX = enemy.transform.position.x;
        knightAttack = GetComponent<knight_attack>();
    }

    void Update()
    {
        if (enemy != null && isEnemyAlive)
        {
            if (!isColliding)
            {
                Chase();
            }
            else
            {
                // Dừng lại khi va chạm với Enemy
                animator.SetBool("isRunning", false);
                TryAttack();
            }
        }
        else
        {
            // Nếu Enemy đã bị destroy, không cần tấn công nữa
            animator.SetBool("isRunning", false);
        }

     
           
    
        
    }

    private void Chase()
    {
        // Kiểm tra xem Enemy đang di chuyển về bên trái hay bên phải
        if (enemy.transform.position.x < previousEnemyPositionX)
        {
            // Enemy đang di chuyển về bên trái
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else
        {
            // Enemy đang di chuyển về bên phải
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }

        transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, speed * Time.deltaTime);
        animator.SetBool("isRunning", true);

        // Lưu trữ vị trí x của Enemy trước đó
        previousEnemyPositionX = enemy.transform.position.x;
    }

    private void TryAttack()
    {
        // Kiểm tra nếu Enemy ở trong phạm vi tấn công và đủ thời gian làm mát
        if (Vector2.Distance(transform.position, enemy.transform.position) <= attackRange
            && Time.time - lastAttackTime >= attackCooldown)
        {
            // Thực hiện tấn công
            knightAttack.Attack(enemy);
            lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
            Debug.Log("Player Attacked Enemy");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isColliding = true;
            // Thêm xử lý khi va chạm với Enemy ở đây
            Debug.Log("Player Bắt đầu va chạm với " + collision.gameObject.name);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Vô hiệu hóa va chạm giữa các Player
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isColliding = false;
            Debug.Log("Kết thúc va chạm với " + collision.gameObject.name);
            animator.SetBool("isAtk", false);
        }
    }

    public void OnEnemyDestroyed()
    {
        isEnemyAlive = false;
        Debug.Log("Player Đã Tiêu Diệt Enemy");
        GameObject newEnemy = GameObject.FindGameObjectWithTag("Enemy");
        if (newEnemy != null)
        {
            enemy = newEnemy;
            previousEnemyPositionX = enemy.transform.position.x;
            isEnemyAlive = true; // Reset lại biến theo dõi trạng thái của Enemy
        }
        else
        {
            // Không tìm thấy Enemy mới, Player sẽ dừng hoạt động
            animator.SetBool("isRunning", false);
        }
    }
}
