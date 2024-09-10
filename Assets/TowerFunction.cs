using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFunction : MonoBehaviour
{
    // Start is called before the first frame update
   
    private int healingValue; // khả năng hổi máu mỗi giây cho lính đồng minh trong castle
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void  OnTriggerStay2D(Collider2D other){
            if(other.gameObject.CompareTag("Player")){
                Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());// vô hiệu hóa
            }
    }
}
