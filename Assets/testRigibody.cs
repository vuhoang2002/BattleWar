using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRigibody : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerStay2D(Collider2D other)
{
    Debug.Log("On Collision");
}

void OnTriggerExit2D(Collider2D other)
{
    Debug.Log("Off Collision");
}

}
