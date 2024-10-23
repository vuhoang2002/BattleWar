
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // Thêm dòng này nếu chưa có

public class SpawnPlayer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Sprite unitAvatar; // Hình đại diện cho đơn vị
    public GameObject mapSize; // Kích thước của bản đồ
    public GameObject prefabToSpawn; // Prefab để spawn
    public int priceUnit; // Giá tiền
    //public float cd_Unit;// thời gian hồi
    public Image cooldownImage; // Hình ảnh cho vòng tròn hồi chiêu
    public float cooldownDuration = 5f; // Thời gian hồi chiêu
    public Button priceButton;
    public Button GOLD_Counter;
    private TextMeshProUGUI buttonText;
    public bool isSpawnReady = true;
    private Renderer renderer;
    private PlayerCountDisplay playerCountDisplay; // Tham chiếu đến PlayerCountDisplay
    private bool isMaxPlayer = false;
    private float highest_Y;
    private float lowest_Y;
    private float positionSpawn_X;
    public UnitListManager unitListManager;
    public AttackOrder atk;
    public DefenseOrder def;
    public FallBackOrder fbk;

    private bool isHolding = false; // Biến để kiểm tra trạng thái ấn giữ
    private float holdTime = 0f; // Thời gian ấn giữ
    private const float maxHoldTime = 0.5f; // Thời gian tối đa để zoom
    private RectTransform rectTransform; // RectTransform của nút
    private string creat_ID_For_Unit; // Biến ID cho đơn vị
    private static int nextId = 1; // Biến static để theo dõi ID tiếp theo
    public GameObject playerList;
    public delegate void UnitSpawnedHandler(GameObject newUnit, string unitId);
    public event UnitSpawnedHandler OnUnitSpawned;
    public Level_Controller levelWarMod;
    public bool isWarMod = false;

    private void Start()
    {
        playerCountDisplay = FindObjectOfType<PlayerCountDisplay>(); // Lấy tham chiếu đến PlayerCountDisplay
        isMaxPlayer = playerCountDisplay.get_isMaxPlayer();
        setSpawnLocation(); // Gọi hàm để thiết lập vị trí spawn
        rectTransform = GetComponent<RectTransform>(); // Lấy RectTransform
        GetComponent<Image>().sprite = unitAvatar;
        if (unitListManager == null)
        {
            GameObject ob = GameObject.Find("PUnit_List");
            unitListManager = ob.GetComponent<UnitListManager>();
        }
        playerList = GameObject.Find("PlayerList(Clone)");
        this.OnUnitSpawned += HandleUnitSpawned;
        cooldownImage.fillAmount = 0;
        levelWarMod.OnBattleStart += HandleBattleStart;
        // đăng kí sk war mod

    }

    private void HandleBattleStart()
    {
        //throw new System.NotImplementedException();
        // yield return new WaitForSeconds(levelWarMod.timePlay);
        isWarMod = true;
        cooldownImage.fillAmount = 1f;
        Debug.Log("Chế độ chiến tranh đã được kích hoạt.");

    }

    void Awake()
    {
        GameObject gameManager = GameObject.Find("GAME_MANAGER");
        levelWarMod = gameManager.GetComponentInChildren<Level_Controller>();
        if (levelWarMod.gameMod == GameMod.War)
        {
            levelWarMod.OnGameModeChanged_War += HandleGameModeChanged_War;
            // mod = GameMod.War;
        }
    }


    private void HandleGameModeChanged_War()
    {
        Debug.Log("Chạy war mode timer");
        //StartCoroutine(On_WarModeActive());
    }

    IEnumerator On_WarModeActive()
    {
        yield return new WaitForSeconds(levelWarMod.timer);
        isWarMod = true;
        cooldownImage.fillAmount = 1f;
        Debug.Log("Chế độ chiến tranh đã được kích hoạt.");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true; // Đặt trạng thái ấn giữ thành true
        holdTime = 0f; // Reset thời gian ấn giữ
        StartCoroutine(HandleHold()); // Bắt đầu coroutine xử lý ấn giữ
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false; // Đặt trạng thái ấn giữ thành false

        if (holdTime < maxHoldTime)
        {
            spawnUnit(); // Spawn đơn vị nếu ấn giữ dưới 1.5 giây
            ZoomOut(); // Quay lại kích thước ban đầu
        }
        else
        {
            ZoomOut(); // Quay lại kích thước ban đầu nếu ấn giữ lâu
            // Tính toán vị trí mới của nút chỉ khi đã giữ lâu hơn maxHoldTime
            RepositionButton();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isHolding)
        {
            // Di chuyển nút theo phương X
            Vector2 newPosition = rectTransform.anchoredPosition + new Vector2(eventData.delta.x, 0);
            rectTransform.anchoredPosition = newPosition;
        }
    }

    private IEnumerator HandleHold()
    {
        while (isHolding)
        {
            holdTime += Time.deltaTime; // Tính toán thời gian ấn giữ
            if (holdTime >= maxHoldTime)
            {
                ZoomIn(); // Phóng to nút nếu quá 1.5 giây
                yield break; // Dừng coroutine
            }
            yield return null; // Đợi frame tiếp theo
        }
    }

    private IEnumerator CooldownRoutine()
    {

        isSpawnReady = false;
        float elapsedTime = 0f;

        // Đặt lại giá trị fillAmount
        cooldownImage.fillAmount = 1f;

        while (elapsedTime < cooldownDuration)
        {
            elapsedTime += Time.deltaTime;
            cooldownImage.fillAmount = 1 - (elapsedTime / cooldownDuration); // Cập nhật giá trị fillAmount
            yield return null; // Đợi frame tiếp theo
        }
        //  yield return new WaitForSeconds(cooldownDuration);

        cooldownImage.fillAmount = 0; // Đặt lại khi hoàn thành
        isSpawnReady = true;
    }

    private void ZoomIn()
    {
        rectTransform.localScale = new Vector3(2f, 2f, 2f); // Đặt kích thước phóng to gấp 2 lần
    }

    private void ZoomOut()
    {
        rectTransform.localScale = new Vector3(1f, 1f, 1f); // Quay lại kích thước ban đầu
    }

    private void RepositionButton()
    {
        // Lấy tất cả các nút trong cùng một parent
        SpawnPlayer[] buttons = FindObjectsOfType<SpawnPlayer>();
        Vector2 currentPos = rectTransform.anchoredPosition;
        int newIndex = -1;

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != this) // Tránh so sánh với chính nó
            {
                Vector2 buttonPos = buttons[i].rectTransform.anchoredPosition;

                if (currentPos.x < buttonPos.x)
                {
                    newIndex = i; // Ghi nhận chỉ số nút thay thế
                    break; // Ngừng vòng lặp nếu đã tìm thấy nút có vị trí lớn hơn
                }
            }
        }

        // Đặt vị trí mới cho nút
        if (newIndex == -1)
        {
            // Nếu không tìm thấy nút nào lớn hơn, đặt ở cuối
            rectTransform.SetAsLastSibling();
        }
        else
        {
            // Nếu tìm thấy, đặt vị trí nút hiện tại ra sau nút đang so sánh
            rectTransform.SetAsFirstSibling();
            for (int i = 0; i < newIndex; i++)
            {
                if (buttons[i] != this)
                {
                    buttons[i].rectTransform.SetAsLastSibling(); // Đẩy nút khác về cuối
                }
            }
        }
    }

    public void setUpButton(GameObject unitToSpawn, Sprite unitAvatar, GameObject mapSize, int price, float cd, GameMod mod)
    {
        gameObject.SetActive(true);
        // Thiết lập nút cho đơn vị để spawn
        this.prefabToSpawn = unitToSpawn; // Gán prefab để spawn
        this.unitAvatar = unitAvatar; // Gán hình đại diện
        GetComponent<Image>().sprite = unitAvatar; // Thiết lập hình đại diện cho UI
        this.mapSize = mapSize; // Gán kích thước bản đồ
        this.priceUnit = price; // Gán giá
        if (mod == GameMod.War)
        {
            this.cooldownDuration = cd * (0.1f);// giảm 90% duration
        }
        else
        {
            this.cooldownDuration = cd;
        }
        setSpawnLocation(); // Gọi hàm để thiết lập vị trí spawn

        // thiết lập giá tiền
        buttonText = priceButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = priceUnit.ToString();
        }
        else
        {
        }
    }

    private void spawnUnit()
    {
        // Thiết lập vị trí spawn ngẫu nhiên
        if (!isSpawnReady || GOLD_Counter.GetComponent<GoldCount>().currentGold < priceUnit || isWarMod)
        {
            GetComponent<SoundPlay>().PlayBtnSound(false);
            return;
        }

        float positionSpawn_Y = Random.Range(lowest_Y, highest_Y);
        isMaxPlayer = playerCountDisplay.get_isMaxPlayer(); // Lấy thông tin về số lượng người chơi hiện tại

        if (!isMaxPlayer)
        {
            if (prefabToSpawn != null)
            {

                Vector3 spawnPosition = new Vector3(positionSpawn_X, positionSpawn_Y, 0f);
                GameObject newUnit = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity); // Spawn prefab

                // Gán ID cho đơn vị mới
                creat_ID_For_Unit = "P" + nextId; // Tạo ID với chữ "P" trước
                nextId++; // Tăng ID cho lần tạo tiếp theo

                // Thiết lập ID cho đơn vị
                newUnit.GetComponent<PlayerController>().SetID(creat_ID_For_Unit);
                newUnit.GetComponent<PlayerController>().Set_BehaviusForPrefab(atk.isAtk_Active, def.isDef_Active, fbk.isFallBack_Active); // set bahavius
                newUnit.GetComponent<PlayerController>().Set_Retreat_Position(spawnPosition);
                newUnit.transform.SetParent(playerList.transform);
                unitListManager.AddUnitToTagList(prefabToSpawn.name, newUnit, creat_ID_For_Unit);
                // Sau khi được thêm, ta cần tạo cho nó 1 defposition
                unitListManager.CreatDef(prefabToSpawn.name);
                OnUnitSpawned?.Invoke(newUnit, creat_ID_For_Unit);
                GOLD_Counter.GetComponent<GoldCount>().currentGold -= priceUnit;
                GetComponent<SoundPlay>().PlayBtnSound(true);
                StartCoroutine(CooldownRoutine());

            }
        }
        else
        {
            GetComponent<SoundPlay>().PlayBtnSound(false);
        }
    }

    private void setSpawnLocation()
    {
        renderer = mapSize.GetComponent<Renderer>();

        Vector3 min = renderer.bounds.min; // Tọa độ thấp nhất
        Vector3 max = renderer.bounds.max; // Tọa độ cao nhất
        highest_Y = max.y; // Lấy tọa độ Y cao nhất
        lowest_Y = min.y; // Lấy tọa độ Y thấp nhất
        positionSpawn_X = min.x; // Lấy tọa độ X thấp nhất
    }

    private void HandleUnitSpawned(GameObject newUnit, string unitId)
    {
    }
}