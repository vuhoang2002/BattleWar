using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeginCanva : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnDestroyBeginCanva()
    {
        GameObject btCanva = GameObject.Find("BattleCanva");
        GameObject beginCanva = btCanva.transform.Find("BeginCanva(Clone)").gameObject;
        Destroy(beginCanva);
        Time.timeScale = 1f;

    }
    public void SetTitleMissiton(string title)
    {
        TextMeshProUGUI misionTitle = transform.Find("MissionText").GetComponent<TextMeshProUGUI>();
        misionTitle.text = title;
    }

}
