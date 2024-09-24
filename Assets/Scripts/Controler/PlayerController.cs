
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public bool isChosen = false;
    public float moveSpeed = 1f;
    public float searchRadius = 5f;
    public float attackCooldown = 0.7f;
    public float highPos = -4.5f; // Giá trị Y khi prefab đạt kích thước lớn nhất
    public float lowPos = -2.5f;  // Giá trị Y khi prefab đạt kích thước nhỏ nhất
    public float minScale = 0.85f; // Kích thước nhỏ nhất
    public float maxScale = 1.5f; // Kích thước lớn nhất
    public bool isAtk_Order = false;
    public bool isDef_Order = true;
    public bool isFallBack_Order = false;
    public bool isHold_Order=false;
    public int heal=5;
    private float timerHeal=0f;
    private float time_Heal=2f;
  //  public string unitTag=Knight;
   
   public Vector3 def_Position;// tọa độ phòng thủ
   public Vector3 retreat_Position; // tọa độ rút lui
    private GameObject target;
    private Animator amt;
    

    private GameObject joystickContainer;
    private Joystick joystick;
    private Vector2 movement;
    public Vector3 currentDirection;
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
    public  bool canChosen = true;// đúng thì mới select được char
    public float timeBwFindTarget = 1f;
    public float timerFindTarget = 0f;

    private bool isRightWay = true;// với player, hướng mặc định là true
    private Attacks attackComponent;
    private Rigidbody2D rb;
    // private bool isColliding;
    public bool isAttacking = false;
    private bool staticDirection = true; //enemy là false, cái này là hướng mặc định
    private Vector2 previousPosition;
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

        GameObject battleCanva = GameObject.Find("BattleCanva");

        if (battleCanva != null)
        {
            joystickContainer = battleCanva.transform.Find("JoyStickCanva").gameObject;
            joystick = joystickContainer.transform.Find("Fixed Joystick").GetComponent<Joystick>();
        }
        previousPosition = transform.position;
        attackComponent = GetComponent<Attacks>();
    }

    void Update()
    {
        // moveSpeed = UnityEngine.Random.Range(final_Speed - 0.75f, final_Speed + 0.75f);
        if (isChosen)
        {
            MoveByJoystick();
        }
        else
        {
            if (isAtk_Order)
            {   unit_Over_Healing();
                if (timerFindTarget <= 0)
                {
                    FindClosestEnemy(searchRadius);
                    timerFindTarget = timeBwFindTarget;
                }
                AttackCommandOrder();
                // cái này chỉ di chuyển về phía trước, còn việc tìm kiếm ở update bên trên
            }
            else if (isDef_Order)
            {   
                unit_Over_Healing();
                currentDirection = new Vector3(0, 0, 0);
                // Logic phòng thủ
                // Giữ nguyên vị trí, chỉ tấn công kẻ địch trong phạm vi vị trí đó, nếu kẻ địch rời khỏi phạm vi thì quay về
                if(def_Position!=null){                
                if(Vector3.Distance(transform.position, def_Position)>0.1f){
                    moveToDefPosition();
                }else{
                    Flip_To_True_Direction();
                }
                }else{
                    Debug.Log("ko có defPosition");
                }
                
            }
            else if (isFallBack_Order)
            {
                // lui về sau tường thành+heal
                moveToRetreatPosition();
                if(Check_Is_At_Retreat_Position()){
                    unit_Is_Healing(true,heal);
                }else{
                    unit_Over_Healing();
                }
            }
            else if(isHold_Order){
                //đứng yên
            }
        }

        if (timerFindTarget > 0)
        {
            timerFindTarget -= Time.deltaTime;
        }
        currentY = transform.position.y;
        ScaleMovement();// chỉnh scale char
                        //    UpdateCharacterDirection();// chỉnh hướng char

        SetOrderLayer();// chỉnh Layer char
        //isRunning(currentDirection);
        CheckMovement();
        Flip();
    }

    void FixedUpdate()
    {
      if(isFallBack_Order){
        timerHeal-=Time.deltaTime;
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
            //float directionX = Mathf.Sign(currentDirection.x); // Xác định hướng di chuyển
            //  UpdateScale(directionX);
            amt.SetBool("isRunning", true);
        }
        else
        {
            amt.SetBool("isRunning", false);
        }

        // Cập nhật vị trí trước đó
        previousPosition = currentPosition;
    }

    // Hàm cập nhật scale dựa trên hướng
    void UpdateScale(float directionX)
    {
        float newScaleX = (directionX > 0) ? 1 : -1; // Chỉ định chiều
        transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
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
        // yield return new WaitForSeconds(0.5f);
        if (!isAttacking)
        {
            if (currentDirection.magnitude > 0.1f)
            {
                amt.SetBool("isRunning", true);
            }
            else if (currentDirection.magnitude == 0)
            {
                amt.SetBool("isRunning", false);
            }
            else
            {
                amt.SetBool("isRunning", false);
            }
        }
        else
        {
            amt.SetBool("isRunning", false);
        }
    }
    // wait for running là isRunning nhưng nó có độ delay( dùng khi command_order)
    void WaitForRunning(Vector3 currentDirection2)
    {
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
        }
        else
        {
            amt.SetBool("isRunning", false);
        }
    }

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

     public void Flip_To_True_Direction()// flip về hướng mặc định
    {
        if (staticDirection)
        {// phải
       transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); 
        // Ghi lại thông báo
      
            isRightWay = true;// tức là nhân vật đang quay sang trái, nó có tác dụng trong hàm scale
        } else{
       
             transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x) * scale / Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isRightWay = false;
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

    void FindClosestEnemy(float searchRadius)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < searchRadius && distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }
        target = closestEnemy;
        if (target != null)
        {
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
       // Debug.Log("Target là: "+target+" isAttacking là: "+ isAttacking);
        if (target == null && !isAttacking)
        {// ko tìm thấy ai thì đi về phía trc
            if (!staticDirection)// cái này là cho enemy
            {   //currentDirection = new Vector3(1, 0, 0).normalized; // Di chuyển sang phải
                Vector3 vt3=new Vector3(transform.position.x +1.0f, def_Position.y, 0);
                currentDirection=(vt3 - transform.position).normalized;
                movement = currentDirection * moveSpeed * Time.deltaTime;
                transform.Translate(movement);
            }
            else
            {//player
                //currentDirection = new Vector3(1, 0, 0).normalized; // Di chuyển sang phải
                Vector3 vt3=new Vector3(transform.position.x +1.0f, def_Position.y, 0);
                currentDirection=(vt3 - transform.position).normalized;
                movement = currentDirection * moveSpeed * Time.deltaTime;
                transform.Translate(movement);
            }
             

        // Tính toán hướng di chuyển về bên phải
       
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

    void OnMouseDown()
    {
       
        // Kiểm tra nếu đối tượng có tag là "Player"
        if (canChosen)
        {
            Collider collider = GetComponent<Collider>();
            isChosen = true;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            // Duyệt qua tất cả các gameobject "Player" và thiết lập isChosen = false
            foreach (GameObject player in players)
            {
                if (player.TryGetComponent(out PlayerController playerController))
                {
                    playerController.canChosen = false;
                }
            }
              chosenPlayerSetCam();
            showJoyStickCanva();
        }
    }
    // hoặc
     void OnPointerClick(PointerEventData eventData)
    {
       
        // Kiểm tra nếu đối tượng có thể được chọn
        if (canChosen)
        {
            isChosen = true;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            // Duyệt qua tất cả các gameobject "Player" và thiết lập canChosen = false
            foreach (GameObject player in players)
            {
                if (player.TryGetComponent(out PlayerController playerController))
                {
                    playerController.canChosen = false;
                }
            }
            chosenPlayerSetCam();
            showJoyStickCanva();// show Joystick
        }
    }
    private void chosenPlayerSetCam(){
        // Tìm kiếm camera
    GameObject mainCamera = GameObject.Find("Main Camera"); 
if (mainCamera != null){
   //     Debug.Log("Found Camera");
    CameraControl cam = mainCamera.GetComponent<CameraControl>();
    if (cam != null)
    {
     //   Debug.Log("Found Cam Controller");

        cam.setChosenPlayer(this.gameObject, true);
    }
}
else
{
 //   Debug.LogWarning("Camera not found!");
}
    }

    public void Set_CanChosen(Boolean bl)
    {
        this.canChosen = bl;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Xử lý va chạm
        if (other.gameObject.CompareTag("Enemy") && isAtk_Order)
        {
            isAttacking = true;

       
        }
       // else if (other.gameObject.CompareTag("Player"))
       // {
            // Vô hiệu hóa va chạm giữa các Player
           // Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
        //}
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Xử lý va chạm

        attackComponent.setAttack(false);
        isAttacking = false;
    }
    //public void MoveToPosition(Vector2 to_position){
      //  do{
        //transform.position = Vector2.MoveTowards(transform.position, to_position, moveSpeed * Time.deltaTime);
        //}while(transform.position!=to_position);
       // Flip_To_True_Direction();

    //}
     public void StartMovingToPosition(Vector3 targetPosition)
    {
        StartCoroutine(MoveToPosition(targetPosition));
    }

private IEnumerator MoveToPosition(Vector3 targetPosition)
{
    while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
    {
        currentDirection = (targetPosition - transform.position).normalized;
        movement = currentDirection * moveSpeed * Time.deltaTime;
        
        // Giới hạn tốc độ
        if (movement.magnitude > Vector3.Distance(transform.position, targetPosition))
        {
            movement = (targetPosition - transform.position).normalized * Vector3.Distance(transform.position, targetPosition);
        }
        
        transform.Translate(movement);   
        yield return null; // Chờ cho đến khung hình tiếp theo
    }

    // Đảm bảo đối tượng đến đúng vị trí
    transform.position = targetPosition;
}
     private void showJoyStickCanva(){
          GameObject BattleCanvas=GameObject.Find("BattleCanva");
          Transform joyStickCanvaTransform = BattleCanvas.transform.Find("JoyStickCanva");
          joyStickCanvaTransform.gameObject.SetActive(true);
    }
    public string Get_Current_Order_toString(){
       if(isChosen){
        return "chosen";
       }
        if(isAtk_Order){
                return "atk";
        }else if(isDef_Order){
                            return "def";

        }
        else if(isFallBack_Order){
                            return "back";

        }
        //else if(isAtk_Order){
            
       // }
        
        return "unKnow_Behavius";
    }
    public void Set_BehaviusForPrefab(bool atk, bool def, bool fck){
            // thiết lập trạng thái ngay khi vừa được spawn cho unit
            this.isAtk_Order=atk;
            this.isDef_Order=def;
            this.isFallBack_Order=fck;
    }
    public void moveToDefPosition(){
        if(def_Position!=null){
          //  StartMovingToPosition(def_Position);
             currentDirection = (def_Position - transform.position).normalized;
             movement = currentDirection * moveSpeed * Time.deltaTime;
            transform.Translate(movement);   
             }
           
    }
    public void moveToPosition(Vector3 position){
            currentDirection = (position - transform.position).normalized;
            movement = currentDirection * moveSpeed * Time.deltaTime;
            transform.Translate(movement);       
    }
    public void moveToPosition(Vector2 position){
            Vector3 position3=new Vector3(position.x, position.y, 0);
              currentDirection = (position3 - transform.position).normalized;
             movement = currentDirection * moveSpeed * Time.deltaTime;
            transform.Translate(movement);         
    }
    public void Set_Def_Position(Vector2 def){
        // chuyển đôi vectorw thành vecotr 3
        this.def_Position=new Vector3(def.x, def.y,0);
    }
    public void Set_Retreat_Position(Vector3 set_position){
        // truyền tham số này ngay khi spawn
        this.retreat_Position= set_position;
    }
    public void moveToRetreatPosition(){
        if(retreat_Position!=null){
            moveToPosition(retreat_Position);
          
        }else{
            Debug.Log("Chưa thiết lập retreat_Postion");
           
        }
      
    }
    public bool Check_Is_At_Retreat_Position(){
        if((transform.position.x- retreat_Position.x)<0.1f){
            // thực hiện tạm ẩn nhân vật
            // thực hiện hồi máu cho nhân vật ở castle
            
            
            return true;

        }
        return false;
    }
    public void unit_Is_Healing(bool inTheCastle, int heal){
        if(inTheCastle){// còn có hồi máu ngoài thành
            gameObject.tag="isHealing";
           gameObject.GetComponent<Renderer>().enabled = false; // Ẩn đối tượng
            gameObject.GetComponent<BoxCollider2D>().enabled = false; // Ngừng tương tác vật lý
        }
        if(timerHeal<=0){
        gameObject.GetComponent<Health>().Heal(heal);
        timerHeal=time_Heal;
        }
    }
    public void unit_Over_Healing(){
         if(gameObject.CompareTag("isHealing")){
          if(staticDirection){
          gameObject.tag="Player";
          }else{
            gameObject.tag="Enemy";
          }

           gameObject.GetComponent<Renderer>().enabled = true; // Ẩn đối tượng
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
         }
    }

}