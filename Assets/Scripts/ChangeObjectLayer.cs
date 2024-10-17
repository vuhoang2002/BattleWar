using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectLayer : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeLayer(GameObject gameObject, string newLayerName)
    {
        int newLayer = LayerMask.NameToLayer(newLayerName);
        if (newLayer != -1) // Kiểm tra xem layer có hợp lệ không
        {
            gameObject.layer = newLayer;
        }
    }

}
