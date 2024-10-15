using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bug_amen : MonoBehaviour


{
  private bool isAtk = false;
  // Start is called before the first frame update
  void Start()
  {
    isAtk = false;
    //(isAtk);

  }

  // Update is called once per frame
  void Update()
  {
    //(isAtk);


  }
  private void OnTriggerStay2D(Collider2D other)
  {
    isAtk = true;


  }
  private void OnTriggerExit2D(Collider2D other)
  {
    isAtk = false;


  }
}
