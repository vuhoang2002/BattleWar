using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCanva : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Canva;
    public static bool show = true;
    void Start()
    {

    }

    // Update is called once per frame
    public void OnActiveButton()
    {
        Canva.SetActive(show);
        show = !show;
    }
}
