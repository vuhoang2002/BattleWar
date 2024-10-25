using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyManager eb;
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Va chạm với " + other.gameObject);
    }

}
