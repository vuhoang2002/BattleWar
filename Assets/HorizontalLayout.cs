using UnityEngine;

public class HorizontalLayout : MonoBehaviour
{
    public float spacing = 1.0f; // khoảng cách giữa các đối tượng

    void Start()
    {
        ArrangeChildren();
    }

    void ArrangeChildren()
    {
        int childCount = transform.childCount;
        float totalWidth = 0f;

        // Tính tổng chiều rộng cần thiết
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                totalWidth += renderer.bounds.size.x;
                if (i < childCount - 1)
                {
                    totalWidth += spacing;
                }
            }
        }

        // Bắt đầu sắp xếp các đối tượng
        float currentX = -totalWidth / 2;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                Vector3 size = renderer.bounds.size;
                child.localPosition = new Vector3(currentX + size.x / 2, 0, 0);
                currentX += size.x + spacing;
            }
        }
    }
}