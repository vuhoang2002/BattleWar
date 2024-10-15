using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonZoomControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    public Vector3 zoomedScale = new Vector3(1.2f, 1.2f, 1.2f); // Kích thước khi phóng to
    public float zoomSpeed = 0.1f; // Tốc độ zoom

    private bool isHolding = false; // Biến kiểm soát trạng thái ấn giữ

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true; // Đặt trạng thái ấn giữ thành true
        StopAllCoroutines(); // Dừng các coroutine trước đó
        StartCoroutine(ZoomIn()); // Bắt đầu phóng to
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false; // Đặt trạng thái ấn giữ thành false
        StopAllCoroutines(); // Dừng các coroutine trước đó
        StartCoroutine(ZoomOut()); // Bắt đầu thu nhỏ
    }

    private System.Collections.IEnumerator ZoomIn()
    {
        while (rectTransform.localScale.x < zoomedScale.x)
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, zoomedScale, zoomSpeed);
            yield return null; // Đợi frame tiếp theo
        }
        rectTransform.localScale = zoomedScale; // Đảm bảo ở kích thước tối đa
    }

    private System.Collections.IEnumerator ZoomOut()
    {
        while (rectTransform.localScale.x > 1f)
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, Vector3.one, zoomSpeed);
            yield return null; // Đợi frame tiếp theo
        }
        rectTransform.localScale = Vector3.one; // Đảm bảo về kích thước ban đầu
    }

    private void Update()
    {
        // Kiểm tra trạng thái nút và không kích hoạt sự kiện nếu đang ấn giữ
        if (isHolding)
        {
            return; // Không thực hiện hành động gì khi đang ấn giữ
        }

        if (Input.GetButtonDown("Fire1")) // Thay "Fire1" bằng input của bạn
        {

        }
    }
}