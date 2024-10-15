using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 0.1f; // Tốc độ di chuyển camera
    public float smoothSpeed = 0.05f; // Tốc độ mượt mà khi di chuyển camera
    private Vector3 lastTouchPosition;
    public GameObject chosenPlayer; // Đối tượng người chơi

    // Các GameObject đại diện cho vị trí bé nhất và lớn nhất
    public GameObject minPosition;
    public GameObject maxPosition;
    private Vector3 worldMinPosition;
    private Vector3 worldMaxPosition;
    //public bool isChosen = false;
    public float cameraOffsetX = 1.0f;

    void Start()
    {
        // Lấy tọa độ thế giới của minPosition và maxPosition
        worldMinPosition = minPosition.transform.position;
        worldMaxPosition = maxPosition.transform.position;
    }

    void Update()
    {
        if (chosenPlayer == null)
        {
            FreeCamera();
        }
        else
        {
            LockCamera();
        }
    }

    private void LockCamera()
    {
        if (chosenPlayer != null) // Kiểm tra xem chosenPlayer có null không
        {
            float desiredPositionX = chosenPlayer.transform.position.x + cameraOffsetX; // Thêm offset

            // Kiểm tra giới hạn
            desiredPositionX = CheckBounds(desiredPositionX, worldMinPosition.x, worldMaxPosition.x);

            Vector3 smoothedPosition = new Vector3(desiredPositionX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, smoothedPosition, smoothSpeed);
        }
        else
        {
            Debug.LogWarning("chosenPlayer is not assigned!");
        }
    }

    // Hàm kiểm tra và giới hạn vị trí camera
    private float CheckBounds(float newPositionX, float minX, float maxX)
    {
        if (newPositionX >= maxX)
        {
            return maxX; // Dừng ở max
        }
        else if (newPositionX <= minX)
        {
            return minX; // Dừng ở min
        }
        return newPositionX; // Không thay đổi
    }

    private void FreeCamera()
    {
        // Kiểm tra touch hoặc chuột
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    Vector3 deltaPosition = new Vector3(touch.position.x, 0, 0) - new Vector3(lastTouchPosition.x, 0, 0);
                    lastTouchPosition = touch.position;

                    // Di chuyển camera theo trục X (đảo ngược hướng)
                    Vector3 move = new Vector3(-deltaPosition.x, 0, 0) * moveSpeed; // Đảo ngược hướng
                    Vector3 newPosition = transform.position + move;

                    // Kiểm tra và dừng nếu đạt giới hạn
                    newPosition.x = CheckBounds(newPosition.x, worldMinPosition.x, worldMaxPosition.x);

                    transform.position = newPosition; // Cập nhật vị trí camera

                    // Debug thông tin
                    break;
            }
        }
        else if (Input.GetMouseButton(0)) // Kiểm tra chuột
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastTouchPosition = Input.mousePosition; // Ghi lại vị trí chuột ban đầu
            }

            Vector3 deltaPosition = new Vector3(Input.mousePosition.x, 0, 0) - new Vector3(lastTouchPosition.x, 0, 0);
            lastTouchPosition = Input.mousePosition;

            // Di chuyển camera theo trục X (đảo ngược hướng)
            Vector3 move = new Vector3(-deltaPosition.x, 0, 0) * moveSpeed; // Đảo ngược hướng
            Vector3 newPosition = transform.position + move;

            // Kiểm tra và dừng nếu đạt giới hạn
            newPosition.x = CheckBounds(newPosition.x, worldMinPosition.x, worldMaxPosition.x);

            transform.position = newPosition; // Cập nhật vị trí camera

            // Debug thông tin
        }
    }
    public void setChosenPlayer(GameObject chosen, bool flag)
    {
        if (flag = true)
        {//chosen
            chosenPlayer = chosen;
        }
        else
        {
            chosen = null;// canel chosen
        }
    }

}