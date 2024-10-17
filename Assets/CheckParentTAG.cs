using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckParentTAG : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject parentObject;
    void Start()
    {
        ChangeObjectLayer changeObjectLayer = new ChangeObjectLayer();
        if (parentObject.transform.CompareTag("Enemy"))
        {
            changeObjectLayer.ChangeLayer(this.gameObject, "EnemyBullet");
        }
        else
        {
            changeObjectLayer.ChangeLayer(this.gameObject, "PlayerBullet");
        }
        SetOrderLayerForBullet();
    }

    // Update is called once per frame
    public void SetOrderLayerForBullet()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.sortingOrder = parentObject.GetComponent<PlayerController>().GetOrderLayer();
    }
}
