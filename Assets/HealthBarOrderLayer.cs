using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarOrderLayer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject parent;
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        GetComponent<Renderer>().sortingOrder = parent.GetComponent<PlayerController>().GetOrderLayer() + 1;
    }

    // Update is called once per frame

}
