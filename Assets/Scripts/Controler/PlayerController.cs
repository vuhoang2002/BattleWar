//using System.Threading.Tasks.Dataflow;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour, IPointerClickHandler
{
    public bool isChosen = false;
    public bool isSelect = false;
    public bool isRanger = false;
    public string id; //id cho player
    public float moveSpeed = 1f;
    public float searchRadius = 5f;
    private float searchRadius_Def = 8f;
    public float highPos = -2.5f; // Giá trị Y khi prefab đạt kích thước lớn nhất
    public float lowPos = -4.5f;  // Giá trị Y khi prefab đạt kích thước nhỏ nhất
    public float minScale = 0.85f; // Kích thước nhỏ nhất
    public float maxScale = 1.5f; // Kích thước lớn nhất
    public UnitOrder order;
    public bool isAtk_Order = false;
    public bool isDef_Order = true;
    public bool isFallBack_Order = false;
    public bool isHold_Order = false;
    public int heal = 5;
    private float timerHeal = 0f;
    private float time_Heal = 2f;
    //  public string unitTag=Knight;

    public Vector3 def_Position;// tọa độ phòng thủ
    public Vector3 retreat_Position; // tọa độ rút lui
    public Vector3 hold_Position; // tọa độ hold
    public GameObject target;
    private Animator amt;
    //   public bool isAtkBtn = false;
    public Transform maxY;
    public Transform minY;

    private GameObject joystickContainer;
    private Joystick joystick;
    private Vector2 movement;
    public Vector3 currentDirection;
    private float currentScale;
    float currentY;// vị trí y hiện tại
    float scale = 1f;
    float truePositionY;
    int orderLayer = 0;
    private int minOrderLayer = 0;
    private int maxOrderLayer = 20;

    private Renderer rend;
    float randomTime = 0.5f;
    float final_Speed = 1f;
    private float nextActionTime = 0f;
    private float period = 0.5f;
    public bool canChosen = true;// đúng thì mới select được char
    public float timeBwFindTarget = 1f;
    public float timer_FindTarget = 0f;

    public bool isRightWay = true;//   Huosng di chuyển hiện tại, dùng nó để bắn cung
    private Attacks attackComponent;
    private Rigidbody2D rb;
    // private bool isColliding;
    public bool isColliding = false;
    public bool isDef_Hold = true;
    public bool isDef_Force = false;
    private bool defaultDirection = true; //enemy là false, cái này là hướng mặc định
    private Vector2 previousPosition;
    //  private bool isTest;
    public static GameObject chosenPlayer;
    public static GameObject selectPlayer;
    public GameObject player;
    //
    public bool thisIsPlayer = true;
    public string findTarget = "Enemy";
    public string aliesTarget = "Player";
    public string findTargetCastle = "EnemyCastle";
    private bool isShowOnCard = false;
    public delegate void SelectionChangedEventHandler(bool isSelected);
    public event SelectionChangedEventHandler OnSelectionChanged;
    public GameObject cursorSelect;


    void Start()
    {
        final_Speed = moveSpeed; // ko xài nx
        randomTime = UnityEngine.Random.Range(0.0f, randomTime);
        rend = GetComponent<Renderer>();// orderLayout
        amt = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //scale for Player
        currentScale = transform.localScale.x; // scale mặc định
        minScale = currentScale * minScale;
        maxScale = currentScale * maxScale;
        GameObject battleCanva = GameObject.Find("BattleCanva");
        if (battleCanva != null)
        {
            joystickContainer = battleCanva.transform.Find("JoyStickCanva").gameObject;
            joystick = joystickContainer.transform.Find("Fixed Joystick").GetComponent<Joystick>();
        }
        previousPosition = transform.position;
        attackComponent = GetComponent<Attacks>();
        if (gameObject.CompareTag("Player"))
        {//các thông số phân biệt player vs enemy
            findTarget = "Enemy";
            aliesTarget = "Player";
            defaultDirection = true;
            thisIsPlayer = true;
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            ChangePlayerToEnemy();
        }
        // chỉnh hightPos và lowPos
        if (maxY != null && minY != null)
        {
            highPos = minY.position.y;// chỗ này đặt sai tên thôi kệ z :v
            lowPos = maxY.position.y;
            //def_Position = new Vector3(0, (highPos + lowPos) / 2, 0);
        }
        OnSelectionChanged += HandleSelectionChanged;
        //Change_Prefab_To_Enemy();

    }



    void Update()
    {
        if (isChosen)
        {
            MoveByJoystick();
        }
        else
        {
            if (isAtk_Order)
            {
                unit_Over_Healing();
                if (timer_FindTarget <= 0)
                {
                    FindClosestEnemy(searchRadius);
                    timer_FindTarget = timeBwFindTarget;
                }
                AttackCommandOrder();
                // cái này chỉ di chuyển về phía trước, còn việc tìm kiếm ở update bên trên
            }
            else if (isDef_Order)
            {
                unit_Over_Healing();

                if (transform.position.x < def_Position.x)
                {//đảm bảo việc vừa spawn thì vẫn đánh
                    isDef_Force = false;
                }
                else if (Vector3.Distance(transform.position, def_Position) > searchRadius_Def)
                {
                    isDef_Force = true; //ép buộc lui về
                }
                currentDirection = new Vector3(0, 0, 0);
                // Logic phòng thủ
                // Giữ nguyên vị trí, chỉ tấn công kẻ địch trong phạm vi vị trí đó, nếu kẻ địch rời khỏi phạm vi thì quay về
                if (def_Position != null && isDef_Hold)
                {
                    if (Vector3.Distance(transform.position, def_Position) > 0.1f)
                    {
                        moveToDefPosition();
                    }
                    else
                    {
                        Flip_To_Default_Direction();
                        isDef_Hold = false;// cái này false đc phép tấn công
                        isDef_Force = false;
                    }
                }
                // hàm tìm kiếm kẻ địch
                if (timer_FindTarget <= 0)
                {
                    if (transform.position.x < def_Position.x)
                    {
                        FindClosestEnemy(searchRadius_Def);
                    }
                    else
                    {
                        FindClosestEnemy_ByFindPositon(searchRadius_Def, def_Position);
                    }
                    timer_FindTarget = timeBwFindTarget;
                }

                if (!isDef_Hold && !isDef_Force)
                {//được phép tấn công
                    AttackCommandOrder();

                }

                if (target == null)
                {
                    isDef_Hold = true;// 
                }
                else if (target != null)
                {// có mục tiêu và không cưỡng chế rút lui
                    isDef_Hold = false;//dược phép tấn công
                }
            }
            else if (isFallBack_Order)
            {
                // lui về sau tường thành+heal
                moveToRetreatPosition();
                if (Check_Is_At_Retreat_Position())
                {
                    unit_Is_Healing(true, heal);
                }
                else
                {
                    unit_Over_Healing();
                }
            }
            else if (isHold_Order)
            {

                //đứng yên
                if (Vector3.Distance(transform.position, hold_Position) > searchRadius_Def)
                {
                    isDef_Force = true; //ép buộc lui về
                }
                currentDirection = new Vector3(0, 0, 0);
                // Logic phòng thủ
                // Giữ nguyên vị trí, chỉ tấn công kẻ địch trong phạm vi vị trí đó, nếu kẻ địch rời khỏi phạm vi thì quay về
                if (def_Position != null && isDef_Hold)
                {
                    if (Vector3.Distance(transform.position, hold_Position) > 0.1f)
                    {
                        moveToPosition(hold_Position);
                    }
                    else
                    {
                        Flip_To_Default_Direction();
                        isDef_Hold = false;// cái này false đc phép tấn công
                        isDef_Force = false;
                    }
                }
                // hàm tìm kiếm kẻ địch
                if (timer_FindTarget <= 0)
                {
                    FindClosestEnemy_ByFindPositon(2f, hold_Position);
                    timer_FindTarget = timeBwFindTarget;
                }

                if (!isDef_Hold && !isDef_Force)
                {//được phép tấn công
                    AttackCommandOrder();
                }

                if (target == null)
                {
                    isDef_Hold = true;// 
                }
                else if (target != null)
                {// có mục tiêu và không cưỡng chế rút lui
                    isDef_Hold = false;//dược phép tấn công
                }
            }
        }

        if (timer_FindTarget > 0)
        {
            timer_FindTarget -= Time.deltaTime;
        }

        currentY = transform.position.y;
        ScaleMovement();// chỉnh scale char
        SetOrderLayer();// chỉnh Layer char
        //isRunning(currentDirection);
        CheckMovement();
        Flip();
        if (!isChosen)
        {
            ActiveChild();
        }
    }

    private void ChangePlayerToEnemy()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            findTarget = "Player";
            aliesTarget = "Enemy";
            findTargetCastle = "PlayerCastle";
            defaultDirection = false;
            thisIsPlayer = false;
        }
    }
    private void checkMyOrder()
    {
        switch (order)
        {
            case UnitOrder.Attack:

                break;
            default:

                break;
        }
    }

    void ActiveChild()
    {
        transform.Find("AtkArea").gameObject.SetActive(true);
        //        transform.Find("ChosenArea").gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        if (isFallBack_Order)
        {
            timerHeal -= Time.deltaTime;
        }
    }

    void CheckMovement()
    {
        Vector2 currentPosition = transform.position;
        // Tính toán sự thay đổi tọa độ
        currentDirection = currentPosition - previousPosition;

        // Kiểm tra sự thay đổi về tọa độ
        if (currentPosition != previousPosition)
        {
            amt.SetBool("isRunning", true);
        }
        else
        {
            amt.SetBool("isRunning", false);
        }
        // Cập nhật vị trí trước đó
        previousPosition = currentPosition;
    }
    // wait for running là isRunning nhưng nó có độ delay( dùng khi command_order)
    void Flip()
    {
        if (currentDirection.x > 0 && !isRightWay) // Di chuyển sang phải
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isRightWay = true;
        }
        else if (currentDirection.x < 0 && isRightWay) // Di chuyển sang trái
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isRightWay = false;// tức là nhân vật đang quay sang trái, nó có tác dụng trong hàm scale
        }
    }

    public void Flip_To_Default_Direction()// flip về hướng mặc định
    {
        if (defaultDirection)
        {// phải
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isRightWay = true;// tức là nhân vật đang quay sang trái, nó có tác dụng trong hàm scale
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isRightWay = false;
        }
    }

    void MoveByJoystick()
    {
        movement = new Vector2(joystick.Horizontal, joystick.Vertical).normalized * moveSpeed * Time.deltaTime;
        Check_MinMax_Position();
        transform.Translate(movement);
    }

    void Check_MinMax_Position()
    {
        if (movement.y > 0 && Mathf.Abs(highPos - transform.position.y) < 0.01f)
        {
            movement = new Vector2(joystick.Horizontal, 0).normalized * moveSpeed * Time.deltaTime;
        }
        else if (movement.y < 0 && Mathf.Abs(lowPos - transform.position.y) < 0.01f)
        {
            movement = new Vector2(joystick.Horizontal, 0).normalized * moveSpeed * Time.deltaTime;
        }
    }

    void FindClosestEnemy(float searchRadius)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(findTarget);
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (GameObject target in players) // targets
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget < searchRadius && distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestTarget = target;
            }
        }
        target = closestTarget;
        if (target != null)
        {
            Debug.DrawRay(gameObject.transform.position, target.transform.position - gameObject.transform.position, Color.red, 1f);
        }
    }

    void FindClosestEnemy_ByFindPositon(float searchRadius, Vector3 find_Position)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(findTarget);
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(find_Position, enemy.transform.position);

            if (distanceToEnemy < searchRadius && distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }
        target = closestEnemy;
        if (target != null)
        {
            Debug.DrawRay(find_Position, target.transform.position - gameObject.transform.position, Color.blue, 1f);
        }
    }

    void MoveTowardTarget()
    {
        if (target != null)
        {
            currentDirection = (target.transform.position - transform.position).normalized;
            movement = currentDirection * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    void MoveTowardTarget_Y_Axis()
    {
        float y_Dif = transform.position.y - target.transform.position.y;
        if ((Mathf.Abs(y_Dif)) > 0.2f)
        {
            currentDirection = new Vector3(0, target.transform.position.y - transform.position.y, 0);
            movement = currentDirection * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
        else
        {
            if (!isColliding)
            {
                Flip_To_Target_Direction();
            }
        }
    }

    void Flip_To_Target_Direction()
    {
        if (target.transform.position.x > transform.position.x) // Di chuyển sang phải
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isRightWay = true;
        }
        else if (target.transform.position.x < transform.position.x) // Di chuyển sang trái
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isRightWay = false;// tức là nhân vật đang quay sang trái, nó có tác dụng trong hàm scale
        }
    }

    void ScaleMovement()
    {
        // Tính toán tỉ lệ scale dựa trên tọa độ Y
        scale = Mathf.Lerp(minScale, maxScale, Mathf.InverseLerp(highPos, lowPos, currentY));
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
        orderLayer = (int)Mathf.Lerp(minOrderLayer, maxOrderLayer, Mathf.InverseLerp(highPos, lowPos, currentY));
        rend.sortingOrder = orderLayer;
    }
    public int GetOrderLayer()
    {
        return orderLayer;
    }

    void AttackCommandOrder()
    {

        if (target != null && isColliding)// có mục tiêu và tiếp cận gần mục tiêu
        {
            attackComponent.CallAttack(target);
        }
        else if (target == null && isColliding)
        {
            // tấn công mục tiêu va chạm đc thực hiện bởi collider
        }
        else
        {
            if (target == null && isAtk_Order)
            {// ko tìm thấy ai thì đi về phía trc
                if (!defaultDirection)// cái này là cho enemy
                {
                    Vector3 vt3 = new Vector3(transform.position.x - 1.0f, def_Position.y, 0);
                    currentDirection = (vt3 - transform.position).normalized;
                    moveToPosition();
                }
                else
                {//player
                    Vector3 vt3 = new Vector3(transform.position.x + 1.0f, def_Position.y, 0);
                    currentDirection = (vt3 - transform.position).normalized;
                    moveToPosition();
                }

            }
            else if (target != null && !isColliding)
            {// có thì đi về phía mục tiêu
                if (isRanger)
                {
                    MoveTowardTarget_Y_Axis();
                }
                else
                {
                    MoveTowardTarget();
                }
            }
        }
    }

    // void OnMouseDown()
    // {
    //     // Kiểm tra nếu đối tượng có tag là "Player"
    //     if (gameObject.CompareTag("Player"))
    //     {
    //         if (canChosen)
    //         {
    //             Collider collider = GetComponent<Collider>();
    //             //isChosen = true;
    //             isSelect = true;
    //             MoveCamToSelectUnit();
    //             Show_OrderCanva();
    //         }
    //     }
    // }
    // public void OnSelect()
    // {
    //     if (canChosen)
    //     {
    //         Collider collider = GetComponent<Collider>();
    //         //isChosen = true;
    //         isSelect = true;

    //         MoveCamToSelectUnit();
    //         Show_OrderCanva();
    //     }
    // }
    public void MoveCamToSelectUnit()
    {
        // Tìm kiếm camera
        GameObject mainCamera = GameObject.Find("Main Camera");

        if (mainCamera != null)
        {
            CameraControl cam = mainCamera.GetComponent<CameraControl>();
            if (cam != null)
            {
                //cam.setChosenPlayer(this.gameObject, true);
                cam.MoveCameraToPosition(transform.position);
            }
        }
    }

    public void Set_CanChosen(Boolean bl)
    {
        this.canChosen = bl;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        BoxCollider2D boxCollider = other.GetComponent<BoxCollider2D>();
        Collider2D thisCollider = GetComponent<Collider2D>();
        if (!isChosen && (other.gameObject.CompareTag(findTarget) || (other.gameObject.CompareTag(findTargetCastle))) && boxCollider != null)
        {
            isColliding = true;
            target = other.gameObject;
            if (GetComponent<Shot>() != null)
            {
                attackComponent.CallAttack(other.gameObject);
            }
        }
        if (other.gameObject.CompareTag(findTargetCastle) && !isChosen)
        {
            attackComponent.CallAttack(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(findTarget) && isChosen)
        {
            //attackComponent.GetAttack_byBtn(other.gameObject);// nhận st
            //("tấn công");
            //  GetComponent<PolygonCollider2D>().enabled=false;// ngăn chặn tấn công đa mục tiêu
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        attackComponent.setAttack(false);
        isColliding = false;
    }
    public void Show_OrderCanva()
    {
        //hiện tất cả cái gì cần hiện
        //code mới:
        GameObject BattleCanvas = GameObject.Find("BattleCanva");
        Transform canva = BattleCanvas.transform.Find("OrderCanva");
        Transform canvaChil = canva.Find("PanelOrder_UnitType");
        canvaChil.gameObject.SetActive(true);
        canvaChil = canva.Find("PanelOrder_OneUnit");
        canvaChil.gameObject.SetActive(true);
        canvaChil = canva.Find("SelectUnit_Btn");
        canvaChil.gameObject.SetActive(true);
        chosenPlayer = this.gameObject;
        //joyStickCanvaTransform = joyStickCanvaTransform.Find("PanelOrder_UnitType");
    }

    public string Get_Current_Order_toString()
    {
        if (isChosen)
        {
            return "chosen";
        }
        if (isAtk_Order)
        {
            return "atk";
        }
        else if (isDef_Order)
        {
            return "def";
        }
        else if (isFallBack_Order)
        {
            return "back";
        }
        //else if(isAtk_Order){
        // }

        return "unKnow_Behavius";
    }

    public void Set_BehaviusForPrefab(bool atk, bool def, bool fck)
    {
        // thiết lập trạng thái ngay khi vừa được spawn cho unit
        this.isAtk_Order = atk;
        this.isDef_Order = def;
        this.isFallBack_Order = fck;
    }
    public void moveToDefPosition()
    {
        if (def_Position != null)
        {
            //  StartMovingToPosition(def_Position);
            currentDirection = (def_Position - transform.position).normalized;
            movement = currentDirection * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    public void moveToPosition(Vector3 position)
    {
        currentDirection = (position - transform.position).normalized;
        movement = currentDirection * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    public void moveToPosition(Vector2 position)
    {
        Vector3 position3 = new Vector3(position.x, position.y, 0);
        currentDirection = (position3 - transform.position).normalized;
        movement = currentDirection * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
    public void moveToPosition()
    {   //currentDirection phải được tính toán trước
        movement = currentDirection * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
    public void Set_Def_Position(Vector2 def)
    {
        // chuyển đôi vectorw thành vecotr 3
        this.def_Position = new Vector3(def.x, def.y, 0);
    }

    public void Set_Retreat_Position(Vector3 set_position)
    {
        // truyền tham số này ngay khi spawn
        this.retreat_Position = set_position;
    }

    public void moveToRetreatPosition()
    {
        if (retreat_Position != null)
        {
            moveToPosition(retreat_Position);
        }
    }

    public bool Check_Is_At_Retreat_Position()
    {
        if ((transform.position.x - retreat_Position.x) < 0.1f)
        {
            // thực hiện tạm ẩn nhân vật
            // thực hiện hồi máu cho nhân vật ở castle

            return true;
        }
        return false;
    }

    public void unit_Is_Healing(bool inTheCastle, int heal)
    {
        if (inTheCastle)
        {// còn có hồi máu ngoài thành
            gameObject.tag = "isHealing";
            gameObject.GetComponent<Renderer>().enabled = false; // Ẩn đối tượng
            gameObject.GetComponent<BoxCollider2D>().enabled = false; // Ngừng tương tác vật lý
            Transform currentBarTransform = gameObject.transform.Find("CurrentBar");
            if (currentBarTransform != null)
            {
                // Lấy Renderer và tắt nó
                Renderer renderer = currentBarTransform.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }
        if (timerHeal <= 0)
        {
            gameObject.GetComponent<Health>().Heal(heal);
            timerHeal = time_Heal;
        }
    }

    public void unit_Over_Healing()
    {
        if (gameObject.CompareTag("isHealing"))
        {
            if (defaultDirection)
            {
                gameObject.tag = "Player";
            }
            else
            {
                gameObject.tag = "Enemy";
            }
            gameObject.GetComponent<Renderer>().enabled = true; // Ẩn đối tượng
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            // Tìm đối tượng con
            Transform currentBarTransform = gameObject.transform.Find("CurrentBar");
            if (currentBarTransform != null)
            {
                // Lấy Renderer và tắt nó
                Renderer renderer = currentBarTransform.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }
        }
    }
    public GameObject GetChosenPlayer()
    {
        // playerHasBeenChosen=this.gameObject;
        return chosenPlayer;
    }
    public void SetChosenPlayer(GameObject gameObject)
    {
        // playerHasBeenChosen=this.gameObject;
        chosenPlayer = gameObject;
    }

    public void SetID(string id)
    {
        this.id = id;
    }
    public void SetBehavius(bool atk, bool def, bool fck, bool hold)
    {
        // thiết lập trạng thái ngay khi vừa được spawn cho unit
        isAtk_Order = atk;
        this.isDef_Order = def;
        this.isFallBack_Order = fck;
        this.isHold_Order = hold;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //throw new NotImplementedException();
        Debug.Log("hihi");
    }
    // thay đổi layer và tag của đối tượng thành enemy
    public void Change_Prefab_To_Enemy()
    {
        // thay đổi layer và màu sắc thanh máu
        ChangeObjectLayer changeObjectLayer = new ChangeObjectLayer();
        gameObject.tag = "Enemy";
        changeObjectLayer.ChangeLayer(this.gameObject, "EnemyLayer");
        changeObjectLayer.ChangeLayer(gameObject.transform.Find("AtkArea").gameObject, "EnemyAtkArea");
        Transform[] children = GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.CompareTag("RangerBullet"))
            {
                changeObjectLayer.ChangeLayer(child.gameObject, "EnemyBullet");
            }
            if (child.name.CompareTo("HealthBar") == 0)
            {
                SpriteRenderer childSpriteRenderer = child.GetChild(1).GetComponent<SpriteRenderer>();
                childSpriteRenderer.color = Color.red;
            }
        }
        ChangePlayerToEnemy();
    }
    public void Set_DefPosition(Vector3 def_Pos)
    {
        def_Position = def_Pos;
    }

    public void Set_RetreatPosition(Vector3 retreat_Pos)
    {
        retreat_Position = retreat_Pos;
    }
    public void Set_CurrentOrder(UnitOrder unitOrder)
    {
        switch (unitOrder)
        {
            case UnitOrder.Attack:
                SetBehavius(true, false, false, false);
                break;
            case UnitOrder.Defend:
                SetBehavius(false, true, false, false);
                break;
            case UnitOrder.Retreat:
                SetBehavius(false, false, true, false);
                break;
            case UnitOrder.Hold:
                SetBehavius(false, false, false, true);
                break;
            default:

                break;
        }
    }


    // hiển thị unit lên trên
    public GameObject Get_SelectPlayer()
    {
        //selectPlayer = this.gameObject;
        return selectPlayer;
    }
    public GameObject Get_SelectPlayerIsThis()
    {
        selectPlayer = this.gameObject;
        return selectPlayer;
    }
    public void Set_SelectPlayer(GameObject gameObject)
    {
        if (gameObject.GetComponent<PlayerController>() != null)
        {
            selectPlayer = gameObject;
        }
    }
    public void Set_isSelect(bool flag)
    {
        if (isSelect != flag)
        {
            isSelect = flag;
            OnSelectionChanged?.Invoke(isSelect);// chỉ gọi nếu có sự thay đổi
        }

        //OnSelectionChanged?.Invoke(isSelect);
    }
    //tạo sự kiện nếu có sự thay đổi về isSelect
    // nếu isSelect true và nếu isSelect là false
    private void HandleSelectionChanged(bool isSelected)
    {
        Debug.Log("Đăng kí sự kiện select");
        Transform[] children = GetComponentsInChildren<Transform>();
        //throw new NotImplementedException();
        if (isSelect)
        {

            foreach (Transform child in children)
            {

                if (child.name.CompareTo("HealthBar") == 0)
                {
                    SpriteRenderer childSpriteRenderer = child.GetChild(1).GetComponent<SpriteRenderer>();
                    childSpriteRenderer.color = Color.yellow;
                    GameObject curSorSelect_Ins = Instantiate(this.cursorSelect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
                    curSorSelect_Ins.transform.SetParent(this.transform);
                }
            }
        }
        else
        {
            foreach (Transform child in children)
            {

                if (child.name.CompareTo("HealthBar") == 0)
                {
                    SpriteRenderer childSpriteRenderer = child.GetChild(1).GetComponent<SpriteRenderer>();
                    if (gameObject.CompareTag("Player"))
                    {
                        childSpriteRenderer.color = Color.green;
                    }
                    else if (gameObject.CompareTag("Enemy"))
                    {
                        childSpriteRenderer.color = Color.red;
                    }
                }
                if (child.name.CompareTo("SelectCursor(Clone)") == 0)
                {
                    Destroy(child.gameObject);
                }
            }

        }
    }

}