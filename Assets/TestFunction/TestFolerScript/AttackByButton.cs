using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByButton : MonoBehaviour

{

  public GameObject hitBox;

  // Start is called before the first frame update
  void Start()
  {

  }

  public void OnMouseDown()
  {
    //hitBox.GetComponent<BoxCollider2D>().enabled = true;
    StartCoroutine(ActivateColliderTemporarily(1f));
  }
  private IEnumerator ActivateColliderTemporarily(float duration)
  {
    hitBox.GetComponent<BoxCollider2D>().enabled = true; // Bật collider

    yield return new WaitForSeconds(duration); // Chờ trong khoảng thời gian đã chỉ định

    hitBox.GetComponent<BoxCollider2D>().enabled = false; // Tắt collider
  }
  private void OnTriggerEnter2D(Collider2D other)
  {
  }
}
