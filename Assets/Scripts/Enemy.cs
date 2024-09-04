using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    public float speed = 1f;
    private Animator animator;
    private bool isColliding = false;
    private float previousPlayerPositionX;
    private float attackRange = 5f; // Khoảng cách tấn công
    private float attackCooldown = 2f; // Thời gian làm mát tấn công (2 giây)
    private float lastAttackTime = 0f;
    private knight_attack knightAttack;
    private bool isPlayerAlive = true; // Biến theo dõi trạng thái của Player

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player GameObject not found!");
        }
        previousPlayerPositionX = player.transform.position.x;
        knightAttack = GetComponent<knight_attack>();
    }

    void Update()
    {
        if (player != null && isPlayerAlive)
        {
            if (!isColliding)
            {
                Swarm();
            }
            else
            {
                // Dừng lại khi va chạm với Player
                animator.SetBool("isRunning", false);
                TryAttack();
            }

            // Kiểm tra nếu Player đã chuyển tag thành "Dead"
            if (player.CompareTag("Dead"))
            {
                OnPlayerDestroyed();
            }
        }
        else
        {
            // Nếu không có Player, Enemy dừng lại
            animator.SetBool("isRunning", false);
            FindNewPlayer();
        }
        
    // Tính toán giá trị scale mới dựa trên y

    }

    private void Swarm()
    {
        // Kiểm tra xem Player đang di chuyển về bên trái hay bên phải
        if (player.transform.position.x < previousPlayerPositionX)
        {
            // Player đang di chuyển về bên trái
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else
        {
            // Player đang di chuyển về bên phải
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        animator.SetBool("isRunning", true);

        // Lưu trữ vị trí x của Player trước đó
        previousPlayerPositionX = player.transform.position.x;
    }

    private void TryAttack()
    {
        // Kiểm tra nếu Player ở trong phạm vi tấn công và đủ thời gian làm mát
        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange
            && Time.time - lastAttackTime >= attackCooldown
            && isPlayerAlive)
        {
            // Thực hiện tấn công
            knightAttack.Attack(player);
            lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
            Debug.Log("Enemy Attacked Player");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = true;
            // Thêm xử lý khi va chạm với Player ở đây
            Debug.Log("Enemy bắt đầu va chạm với " + collision.gameObject.name);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Vô hiệu hóa va chạm giữa các enemy
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isColliding = false;
            Debug.Log("Kết thúc va chạm với " + collision.gameObject.name);
            animator.SetBool("isAtk", false);
            FindNewPlayer(); // Tìm Player mới khi kết thúc va chạm
        }
    }

    public void OnPlayerDestroyed()
    {
        isPlayerAlive = false;
        animator.SetBool("isAtk", false); // Đặt lại isAtk thành false
        Debug.Log("Enemy đã tiêu diệt player");
        FindNewPlayer(); // Tìm Player mới sau khi tiêu diệt
    }

    private void FindNewPlayer()
    {
        // Tìm kiếm Player mới
        GameObject newPlayer = GameObject.FindGameObjectWithTag("Player");
        if (newPlayer != null)
        {
            player = newPlayer;
            previousPlayerPositionX = player.transform.position.x;
            isPlayerAlive = true;
            Debug.Log("Enemy đã tìm thấy Player mới: " + player.name);
        }
        else
        {
            // Không tìm thấy Player mới, Enemy sẽ dừng hoạt động
            Debug.Log("Không tìm thấy Player mới");
            animator.SetBool("isRunning", false);
        }
    }
}
