using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AboutUs : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI textAboutUs;
    public GameObject aboutUsCanva;
    void Start()
    {
        textAboutUs.text = "Vũ Văn Hoàng -CT050221 \n Nguyên Văn Chúc - CT050206 \n Nguyên Trung Đạt -CT050207 \n Lý Xuân Hòa -CT050220\n Đinh Quang Huy -CT050225 ";
    }

    // Update is called once per frame
    public void CloseAboutUs()
    {
        aboutUsCanva.SetActive(false);
    }
    public void OpenAboutUs()
    {
        aboutUsCanva.SetActive(true);
    }
}
