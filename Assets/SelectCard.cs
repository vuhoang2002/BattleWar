using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCard : MonoBehaviour
{
    // Start is called before the first frame update
    public static int MAX_CARD = 6;
    private bool isMaxCard = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool CheckCountOfCard_IsMax()
    {
        Debug.Log("Card is" + transform.childCount);

        // Kiểm tra xem số lượng child có lớn hơn hoặc bằng MAX_CARD không
        if (transform.childCount >= MAX_CARD)
        {
            return true; // Đã đạt tối đa
        }
        return false; // Chưa đạt tối đa
    }
    public void Set_isMaxCard(bool flag)
    {
        isMaxCard = flag;
    }
    public bool Get_isMaxCard()
    {
        return isMaxCard;
    }
}
