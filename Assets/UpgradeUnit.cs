using UnityEngine;

public class UpgradeUnit : MonoBehaviour
{
    // Sự kiện được gọi khi isUpgrade trở thành true
    public delegate void UpgradeEventHandler();
    public event UpgradeEventHandler OnUpgrade;

    public bool _isUpgrade; // Biến riêng tư để lưu trữ trạng thái

    // Thuộc tính để kiểm tra và thiết lập isUpgrade
    public bool isUpgrade
    {
        get { return _isUpgrade; }
        set
        {
            if (value && !_isUpgrade) // Chỉ gọi sự kiện nếu chuyển từ false sang true
            {
                _isUpgrade = true;
                OnUpgrade?.Invoke(); // Gọi sự kiện
            }
            else
            {
                _isUpgrade = value;
            }
        }
    }

    // Ví dụ: phương thức để nâng cấp đơn vị
    public void Upgrade()
    {
        isUpgrade = true; // Đặt isUpgrade thành true
    }
}