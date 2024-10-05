
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    public bool isChosen = false;
    public string id; //id cho player
    public float moveSpeed = 1f;
    public float searchRadius = 5f;
    private float searchRadius_Def=8f;
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
   public Vector3 hold_Position;
    public GameObject target;
    private Animator amt;
    public bool isAtkBtn=false;
    
    

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

    public bool isRightWay = true;// với player, hướng mặc định là true. Huosng di chuyển hiện tại, dùng nó để bắn cung
    private Attacks attackComponent;
    private Rigidbody2D rb;
    // private bool isColliding;
    public bool isAttacking = false;
    public bool isDef_Hold=true;
       public bool isDef_Force=false;
    public bool staticDirection = false; //enemy là false, cái này là hướng mặc định
    private Vector2 previousPosition;
    //  private bool isTest;
    public static GameObject playerHasBeenChosen;
    //
    public bool thisIsPlayer=false;
    public string findTarget="Enemy";
    public string aliesTarget="Player";
    public string findTargetCastle="EnemyCastle";
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
        if(thisIsPlayer ||  gameObject.CompareTag("Player")){//các thông số phân biệt player vs enemy
            findTarget="Enemy";
            aliesTarget="Player";
            staticDirection=true;
        }else if(!thisIsPlayer || gameObject.CompareTag("Enemy")  ){
                findTarget="Player";
                aliesTarget="Enemy";
                findTargetCastle="PlayerCastle";
                staticDirection=false;
        }
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
               
                 if(transform.position.x> def_Position.x){//đảm bảo việc vừa spawn thì vẫn đánh// enemy thì lón hơn
                    isDef_Force=false;
                } else if(Vector3.Distance(transform.position, def_Position)>searchRadius_Def){
                    isDef_Force=true; //ép buộc lui về
                }

                currentDirection = new Vector3(0, 0, 0);
                // Logic phòng thủ
                // Giữ nguyên vị trí, chỉ tấn công kẻ địch trong phạm vi vị trí đó, nếu kẻ địch rời khỏi phạm vi thì quay về
                if(def_Position!=null && isDef_Hold ){                
                if(Vector3.Distance(transform.position, def_Position)>0.1f){
                    moveToDefPosition();
                }else{
                    Flip_To_True_Direction();
                    isDef_Hold=false;// cái này false đc phép tấn công
                    isDef_Force=false;
                }
                }


                if (timerFindTarget <= 0)
                {   if(transform.position.x> def_Position.x){
                            FindClosestEnemy(searchRadius_Def);
                }else{
                    FindClosestEnemy_ByFindPositon(searchRadius_Def, def_Position);
                }
                    timerFindTarget = timeBwFindTarget;
                }

                if(!isDef_Hold && !isDef_Force){//được phép tấn công
                AttackCommandOrder();
                }

                if(target==null){
                    isDef_Hold=true;// 
                }
                else if(target!=null ){// có mục tiêu và không cưỡng chế rút lui
                    isDef_Hold=false;//dược phép tấn công
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
               // hold_Position=transform.position;
                if(Vector3.Distance(transform.position, hold_Position)>searchRadius_Def){
                    isDef_Force=true; //ép buộc lui về
                }
                currentDirection = new Vector3(0, 0, 0);
                // Logic phòng thủ
                // Giữ nguyên vị trí, chỉ tấn công kẻ địch trong phạm vi vị trí đó, nếu kẻ địch rời khỏi phạm vi thì quay về
                if(def_Position!=null && isDef_Hold ){                
                if(Vector3.Distance(transform.position, hold_Position)>0.1f){
                    moveToPosition(hold_Position);
                }else{
                    Flip_To_True_Direction();
                    isDef_Hold=false;// cái này false đc phép tấn công
                    isDef_Force=false;
                }
                }
                // hàm tìm kiếm kẻ địch
                if (timerFindTarget <= 0)
                {   
                    FindClosestEnemy_ByFindPositon(searchRadius_Def, hold_Position);
                    timerFindTarget = timeBwFindTarget;
                }

                if(!isDef_Hold && !isDef_Force){//được phép tấn công
                AttackCommandOrder();
                }

                if(target==null){
                    isDef_Hold=true;// 
                }
                else if(target!=null ){// có mục tiêu và không cưỡng chế rút lui
                    isDef_Hold=false;//dược phép tấn công
                }
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
        if(!isChosen){
            ActiveChild();
        }
    }
    void ActiveChild(){
        transform.Find("AttackArea").gameObject.SetActive(true);
       // transform.Find("ChosenArea").gameObject.SetActive(true);

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
    GameObject[] players = GameObject.FindGameObjectsWithTag(findTarget);
    GameObject[] playerTowers = GameObject.FindGameObjectsWithTag(findTargetCastle);

    // Kết hợp hai mảng thành một danh sách
    GameObject[] targets = new GameObject[players.Length + playerTowers.Length];
    players.CopyTo(targets, 0);
    playerTowers.CopyTo(targets, players.Length);

    float closestDistance = Mathf.Infinity;
    GameObject closestTarget = null;

    foreach (GameObject target in targets)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget < searchRadius && distanceToTarget < closestDistance)
        {
            closestDistance = distanceToTarget;
            closestTarget = target;
        }
    }
    target = closestTarget;
     if(target!=null){
    Debug.DrawRay(gameObject.transform.position,target.transform.position-gameObject.transform.position, Color.blue, 1f);
    }
  //  if(closestDistance<=1f){
  //      isAttacking=true;
  //  }
  //  Debug.Log("Target is "+ target);
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
         //target = closestTarget;
     if(target!=null){
    Debug.DrawRay(find_Position,target.transform.position-gameObject.transform.position, Color.blue, 1f);
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
        if (target == null && !isAttacking && isAtk_Order)
        {// ko tìm thấy ai thì đi về phía trc
            if (!staticDirection)// cái này là cho enemy
            {   //currentDirection = new Vector3(1, 0, 0).normalized; // Di chuyển sang phải
                Vector3 vt3=new Vector3(transform.position.x -1.0f, def_Position.y, 0);
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
        // Tính toán hướng di chuyển về bên phả 
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
      //  Debug.Log("Va chạm với "+ other);
        // Xử lý va chạm
       
        BoxCollider2D boxCollider = other.GetComponent<BoxCollider2D>();
          Collider2D thisCollider = GetComponent<Collider2D>();
         if (other.gameObject.CompareTag(aliesTarget) || other.gameObject.CompareTag(findTarget) )
    {
        // Vô hiệu hóa va chạm với đối tượng nà
            Physics2D.IgnoreCollision(boxCollider, thisCollider, true);
    }

        if ((other.gameObject.CompareTag(findTarget)|| other.gameObject.CompareTag(findTargetCastle))  && boxCollider!=null )
        {
            isAttacking = true;

            if(GetComponent<Shot>()!=null){//dành cho xạ thủ
                Collider2D[] playerColliders = GetComponents<Collider2D>();
               foreach (var collider in playerColliders)
            {
                // Kiểm tra nếu collider là BoxCollider2D
             if (collider is PolygonCollider2D || collider is EdgeCollider2D)
                {
                  
                     attackComponent.CallAttack(other.gameObject);    
                       return;
                }
                //attackComponent.CallAttack(other.gameObject);
            }

        }
        }
        if(other.gameObject.CompareTag(findTargetCastle)){
           // Debug.Log("va chạm vs tower");
            attackComponent.CallAttack(other.gameObject);
        }


    }
    private void OnTriggerEnter2D(Collider2D other) {
          // Debug.Log("Va chạm với "+ other);
        if (other.gameObject.CompareTag(findTarget) && isChosen )
        {
            attackComponent.GetAttack_byBtn(other.gameObject);// nhận st
            Debug.Log("tấn công");
            GetComponent<PolygonCollider2D>().enabled=false;// ngăn chặn tấn công đa mục tiêu
        }
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
        //hiện tất cả cái gì cần hiện
          GameObject BattleCanvas=GameObject.Find("BattleCanva");
          Transform joyStickCanvaTransform = BattleCanvas.transform.Find("JoyStickCanva");
          joyStickCanvaTransform.gameObject.SetActive(true);
          // tìm order unittype
         joyStickCanvaTransform = BattleCanvas.transform.Find("OrderCanva");
         joyStickCanvaTransform = joyStickCanvaTransform.Find("PanelOrder_UnitType");
         joyStickCanvaTransform.gameObject.SetActive(true);
         joyStickCanvaTransform = BattleCanvas.transform.Find("FunctionCanva");
          joyStickCanvaTransform.gameObject.SetActive(true);
         GameObject atkbtn= joyStickCanvaTransform.Find("Atck_Btn").gameObject;
          atkbtn.GetComponent<ButtonHandler>().setAttacks_Var (this.gameObject);//truyền attack vào ở đây
        // joyStickCanvaTransform.gameObject.GetComponent<ButtonHandler>().setAttacks_Var();
         //gameObject.transform.Find("ChosenArea").gameObject.SetActive(false);
        gameObject.transform.Find("AttackArea").gameObject.SetActive(false);  
        playerHasBeenChosen=this.gameObject;
           //joyStickCanvaTransform = joyStickCanvaTransform.Find("PanelOrder_UnitType");
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
            // Tìm đối tượng con
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
else
{
    Debug.LogWarning("Không tìm thấy đối tượng con có tên 'CurrentBar'");
}
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
else
{
    Debug.LogWarning("Không tìm thấy đối tượng con có tên 'CurrentBar'");
}
         }
    }
   
    public void AttackByButton(){
        // hàm này tìm kiếm mục tiêu
        isAtkBtn=true;
     // target
    }
    public GameObject GetChosenPlayer(){
        Debug.Log("Game object này là"+ this.gameObject);
       // playerHasBeenChosen=this.gameObject;
        return playerHasBeenChosen;
    }
    public void SetID(string id){
        this.id=id;
    }
    public void SetBehavius(bool atk, bool def ,bool retreat ){
        this.isAtk_Order=atk;
        this.isDef_Order=def;
        this.isFallBack_Order=retreat;
    }
}