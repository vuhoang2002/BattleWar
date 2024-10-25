
using UnityEngine;

public class ScaleMovement : MonoBehaviour
{
    float currentScale;
    public void ScaleMovementActive(float minScale, float maxScale, float highPos, float lowPos, float currentY, bool isRightWay)
    {
        // Tính toán tỉ lệ scale dựa trên tọa độ Y
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.InverseLerp(highPos, lowPos, currentY));
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
    void Update()
    {
        //chỉ dùng cho healthbaR
        float currentScaleX = transform.localScale.x;
        transform.localScale = new Vector3(currentScaleX, 3f, 0);
    }
}
