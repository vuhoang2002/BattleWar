using UnityEngine;
using UnityEngine.EventSystems;

public class bug_amen : MonoBehaviour, IPointerClickHandler
{
  public void OnPointerClick(PointerEventData eventData)
  {
    Debug.Log("GameObject clicked: " + gameObject.name);
  }

  void Update()
  {
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    {
      Vector2 touchPosition = Input.GetTouch(0).position;
      Ray ray = Camera.main.ScreenPointToRay(touchPosition);
      RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

      if (hit.collider != null && hit.collider.gameObject == gameObject)
      {
        Debug.Log("GameObject clicked via touch: " + gameObject.name);
      }
    }
  }
}