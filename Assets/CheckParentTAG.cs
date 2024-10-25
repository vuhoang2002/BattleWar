using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckParentTAG : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject parentObject;
    public bool isCursor;
    public float currentScaleX;
    public float currentScaleY;

    void Start()
    {

        currentScaleX = transform.localScale.x;
        currentScaleY = transform.localScale.y;
        if (isCursor)
        {
            parentObject = transform.parent.gameObject;
            SetOrderLayerForBullet();
            return;
        }

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
        PlayerController playerController = parentObject.GetComponent<PlayerController>();
        rend.sortingOrder = playerController.GetOrderLayer();
        bool isRightWay = playerController.isRightWay;

        float scale = playerController.scale;
        if (isRightWay)
        {// nếu nhân vật đang quay sang phải
            transform.localScale = new Vector3(currentScaleX * scale, currentScaleY * scale, scale);// nếu nhân vật quay sang trái thì cái này vẫn khiến nhân vật quay sang phải
        }
        else if (!isRightWay)
        {
            transform.localScale = new Vector3(-currentScaleX * scale, currentScaleY * scale, scale);// nếu nhân vật quay sang trái thì cái này vẫn khiến nhân vật quay sang phải
        }
    }
}
