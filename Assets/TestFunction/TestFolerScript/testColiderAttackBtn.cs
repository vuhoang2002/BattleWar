using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testColiderAttackBtn : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

    }
    private void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
    }
}
