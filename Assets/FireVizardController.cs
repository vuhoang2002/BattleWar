using UnityEngine;

public class FireVizardController : MonoBehaviour
{
    public GameObject fireBallAbl1; // Prefab của FireBall
    public Transform spawnFireBall; // Vị trí để spawn FireBall
    private int fireBallAtk;
    private bool arrowDirection; // Khai báo biến arrowDirection
    private UpgradeUnit upgradeUnit;

    private int extraDmg = 0;

    void Start()
    {
        fireBallAtk = GetComponent<Attacks>().abl1_Atk;
        extraDmg = GetComponent<Attacks>().extraDmg;
        upgradeUnit = GetComponent<UpgradeUnit>();

        if (upgradeUnit != null)
        {
            upgradeUnit.OnUpgrade += HandleUpgrade; // Đăng ký sự kiện
        }

    }

    private void HandleUpgrade()
    {
        // Gọi phương thức OnUpgrade từ UpgradeUnit
        OnUpgrade();
    }

    void OnUpgrade()
    {
        // Thiết lập biến isUpgrade trong UpgradeUnit
        upgradeUnit.isUpgrade = true; // Đảm bảo sử dụng biến từ UpgradeUnit
        Debug.Log("Unit được nâng cấp");
        // Thực hiện các thay đổi khác
        gameObject.GetComponent<Animator>().SetBool("isUpgrade", true);
        fireBallAtk += 5;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Kiểm tra xem collider va chạm có phải là Enemy hay EnemyCastle không
        GetComponent<Attacks>().isAbl1 = Vector3.Distance(other.transform.position, transform.position) >= 1f;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Nếu ra ngoài BoxCollider, đặt lại trạng thái isAbl1
        GetComponent<Attacks>().isAbl1 = false; // Đặt lại trạng thái
    }

    public void Spawn_FireBall()
    {
        // upgradeUnit.Upgrade();
        int deadDmg = fireBallAtk;
        var controller = GetComponent<PlayerController>() ?? (Component)GetComponent<PlayerController>();

        if (controller != null)
        {
            arrowDirection = controller is PlayerController player ? player.isRightWay : ((PlayerController)controller).isRightWay;
        }

        if (fireBallAbl1 != null && spawnFireBall != null) // Kiểm tra prefab và vị trí không null
        {
            // Spawn ra FireBall
            GameObject arrowInstance = Instantiate(fireBallAbl1, spawnFireBall.position, spawnFireBall.rotation);

            // Sử dụng biến isUpgrade từ UpgradeUnit
            if (upgradeUnit.isUpgrade) // Kiểm tra biến từ UpgradeUnit
            {
                Animator amtFireBall = arrowInstance.GetComponent<Animator>();
                amtFireBall.SetBool("isUpgrade", true);
                SpriteRenderer spriteRenderer = arrowInstance.GetComponent<SpriteRenderer>();
                spriteRenderer.color = Color.red;
            }

            arrowInstance.SetActive(true);
            if (GetComponent<Attacks>().Get_IsDealExtraDmg())
            {
                deadDmg += extraDmg;
            }
            arrowInstance.GetComponent<Arrow>().SetArrowDmg_Direction(deadDmg, arrowDirection);
        }
    }
}