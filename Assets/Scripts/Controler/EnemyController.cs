
using System;
using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public bool isChosen = false;
    public float moveSpeed = 5f;
    public float searchRadius = 5f;
    public float highPos = -4.5f; // Giá trị Y khi prefab đạt kích thước lớn nhất
    public float lowPos = -2.5f;  // Giá trị Y khi prefab đạt kích thước nhỏ nhất
    public float minScale = 0.75f; // Kích thước nhỏ nhất
    public float maxScale = 1.5f; // Kích thước lớn nhất
    public bool isAtk_Order = false;
    public bool isDef_Order = true;
    public bool isFallBack_Order = false;
    private GameObject target;
    private Animator amt;

    private GameObject joystickContainer;
    private Joystick joystick;
    private Vector2 movement;
    private Vector3 currentDirection;
    private float currentScale;
    float currentY;// vị trí y hiện tại
    float scale = 1f;
    float truePossitionY;
    int orderLayer = 0;

    private int minOrderLayer = 0;
    private int maxOrderLayer = 20;

    private Renderer rend;
    float randomTime = 0.5f;
    float final_Speed = 1f;
    private float nextActionTime = 0f;
    private float period = 0.5f;
    private bool canChosen = true;// đúng thì mới select được char
    public float timeBwFindTarget = 1f;
    public float timerFindTarget = 0f;

    private bool isRightWay = false;// với player, hướng mặc định là true
    private Attacks attackComponent;
    private Rigidbody2D rb;
   // private bool isColliding;
    public bool isAttacking=false;
    private bool staticDirection=false;// Enemy thì cái này false
  //  private bool isTest;

    void Start()
    {
        final_Speed = moveSpeed;
        randomTime = UnityEngine.Random.Range(0.0f, randomTime);
        rend = GetComponent<Renderer>();// orderLayout
        amt = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //scale for Player
        currentScale = transform.localScale.x; // scale mặc định
        minScale = currentScale * minScale;
        maxScale = currentScale * maxScale;
    
        Debug.Log("Chạy start");
        GameObject battleCanva = GameObject.Find("BattleCanva");

        if (battleCanva != null)
        {
            joystickContainer = battleCanva.transform.Find("JoyStickCanva").gameObject;
            joystick = joystickContainer.transform.Find("Fixed Joystick").GetComponent<Joystick>();
        }
        attackComponent = GetComponent<Attacks>();

        if(staticDirection){
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }else{
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }

    void Update()
    {  
        if(!staticDirection){
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        // moveSpeed = UnityEngine.Random.Range(final_Speed - 0.75f, final_Speed + 0.75f);
        if (isChosen)
        {
            MoveByJoystick();
        }
        else
        {
            if (isAtk_Order)
            {
                if (timerFindTarget <= 0)
                {
                    FindClosestPlayer(searchRadius);
                    timerFindTarget = timeBwFindTarget;
                }
                AttackCommandOrder();
                // cái này chỉ di chuyển về phía trước, còn việc tìm kiếm ở update bên trên
            }
            else if (isDef_Order)
            {
                currentDirection = new Vector3(0,0,0);
                // Logic phòng thủ
                // Giữ nguyên vị trí, chỉ tấn công kẻ địch trong phạm vi vị trí đó, nếu kẻ địch rời khỏi phạm vi thì quay về
            }
            else if (isFallBack_Order)
            {
                // lui về sau tường thành
            }
        }
        
        if (timerFindTarget > 0)
        {
            timerFindTarget -= Time.deltaTime;
        }
        currentY = transform.position.y;
        ScaleMovement();// chỉnh scale char
        UpdateCharacterDirection();// chỉnh hướng char
        Flip();
        SetOrderLayer();// chỉnh Layer char
        //isRunning(currentDirection);
       
    }

    void FixedUpdate()

    {
    }

    void UpdateCharacterDirection()
    {
        if (isChosen)
        {
            currentDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            isRunning(currentDirection);
        }
        else
        {
            isRunning(currentDirection);
        }
    }

    void isRunning(Vector3 currentDirection)
    {
        Debug.Log("isRunning curent là:" + currentDirection);
        //Flip();
        // yield return new WaitForSeconds(0.5f);
        if(!isAttacking){
        if (currentDirection.magnitude > 0.1f)
        {
            amt.SetBool("isRunning", true);
            Debug.Log("chạy được bật");
        }
        else if (currentDirection.magnitude == 0)
        {
            amt.SetBool("isRunning", false);
            Debug.Log("chạy được tắt");
        }
        else
        {
            amt.SetBool("isRunning", false);
            Debug.Log("chạy được tắt");
        }}else{
             amt.SetBool("isRunning", false);
        }
    }
    // wait for running là isRunning nhưng nó có độ delay( dùng khi command_order)
    void WaitForRunning(Vector3 currentDirection2)
    {
        Debug.Log("WaitForRunning curent là:" + currentDirection2);
        // Flip();
        StartCoroutine(SetRunningState(currentDirection2));
    }
    //hàm chờ
    private IEnumerator SetRunningState(Vector3 currentDirection2)
    {
        yield return new WaitForSeconds(randomTime); // Chờ 0.5 giây
        //amt.SetBool("isRunning", currentDirection.magnitude > 0.1f);
        if (currentDirection2.magnitude > 0.1f)
        {
            amt.SetBool("isRunning", true);
            Debug.Log("chạy được bật");
        }
        else
        {
            amt.SetBool("isRunning", false);
            Debug.Log("chạy được TẮT");
        }
    }

    void Flip()
    {
        Debug.Log("hướng x là:" +currentDirection.x);
        if (currentDirection.x > 0 && !isRightWay) // Di chuyển sang phải
        {
            //Debug.Log("Xoay Phải");
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isRightWay = true;
        }
        else if (currentDirection.x < 0 && isRightWay) // Di chuyển sang trái
        {
            // Debug.Log("Xoay Trái");
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isRightWay = false;// tức là nhân vật đang quay sang trái, nó có tác dụng trong hàm scale
        }
    }

    void MoveByJoystick()
    {
       movement = new Vector2(joystick.Horizontal, joystick.Vertical) * moveSpeed * Time.deltaTime;
                   //currentDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        // Cập nhật currentDirection với tốc độ di chuyển
       // currentDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    void FindClosestPlayer(float searchRadius)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;
        Debug.Log("Search Player");

        foreach (GameObject Player in enemies)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

            if (distanceToPlayer < searchRadius && distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestPlayer = Player;
            }
        }
        target = closestPlayer;
        if (target != null)
        {
            Debug.Log("found Player");
        }
    }

    void MoveTowardTarget()
    {
        if (target != null)
        {
            currentDirection = (target.transform.position - transform.position).normalized;
            movement = currentDirection * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
            Debug.Log("move to Player");
            
        }
    }

    void ScaleMovement()
    {
        // Tính toán tỉ lệ scale dựa trên tọa độ Y
        scale = Mathf.Lerp(minScale, maxScale, Mathf.InverseLerp(lowPos, highPos, currentY));
        // nên ta có hàm này ;v
        if (isRightWay)
        {// nếu nhân vật đang quay sang phải
            transform.localScale = new Vector3(scale, scale, scale);// nếu nhân vật quay sang trái thì cái này vẫn khiến nhân vật quay sang phải
        }
        else if (!isRightWay)
        {
            transform.localScale = new Vector3(-scale, scale, scale);// nếu nhân vật quay sang trái thì cái này vẫn khiến nhân vật quay sang phải
        }
    }

    void SetOrderLayer()
    {
        orderLayer = (int)Mathf.Lerp(minOrderLayer, maxOrderLayer, Mathf.InverseLerp(lowPos, highPos, currentY));
        rend.sortingOrder = orderLayer;
    }

    void AttackCommandOrder()
    {
        if (target == null && !isAttacking)
        {// ko tìm thấy ai thì đi về phía trc
            if(!staticDirection){
            currentDirection = new Vector3(-1, 0, 0); // Di chuyển sang phải
            movement = -Vector3.right * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
            }else{
             currentDirection = new Vector3(1, 0, 0); // Di chuyển sang phải
            movement = Vector3.right * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
            }
        }
        else if (target != null && !isAttacking)
        {// có thì đi về phía mục tiêu
            MoveTowardTarget();
        }
        if (target != null && isAttacking)// có mục tiêu và tiếp cận gần mục tiêu
        {
            attackComponent.CallAttack(target);
        }
    }

    

    public void Set_CanChosen(Boolean bl)
    {
        this.canChosen = bl;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Xử lý va chạm
        if (other.gameObject.CompareTag("Player") && isAtk_Order)
        {
            isAttacking = true;  
          
           
            // Thêm xử lý khi va chạm với Player ở đây
        //    Debug.Log("Player Bắt đầu va chạm với " + other.gameObject.name);
        Debug.Log("On Collision");
        Debug.Log("On Collision và "+ isAttacking);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
           // Vô hiệu hóa va chạm giữa các Player
           Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
       }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Xử lý va chạm
      // Debug.Log("Kết thúc va chạm");
        Debug.Log("Off Collision");
        attackComponent.setAttack(false);
        isAttacking = false;
       
        
    }
    

}