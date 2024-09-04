using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage=10;
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.GetComponent<Health>()!= null){
            Health health= collider.GetComponent<Health>();
            //health.Damage(damage);
        }
    }
}
