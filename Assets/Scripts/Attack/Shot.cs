using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
  // Start is called before the first frame update
  public GameObject spawnArrow;
  public Transform spawnPoint;
  public int extraDmg = 0;

  void Start()
  {
    extraDmg = GetComponent<Attacks>().extraDmg;
  }

  // Update is called once per frame

  public void Spawn_Arrow(int basic_Atk, bool arrowDirecction)
  {
    int dealDmg = basic_Atk;
    GameObject arrowInstance = Instantiate(spawnArrow, spawnPoint.position, spawnPoint.rotation);
    arrowInstance.SetActive(true);
    if (GetComponent<Attacks>().Get_IsDealExtraDmg())
    {
      dealDmg += extraDmg;
    }
    arrowInstance.GetComponent<Arrow>().SetArrowDmg_Direction(dealDmg, arrowDirecction);
    // Thêm lực cho mũi tên nếu cần
    //Rigidbody rb = arrowInstance.GetComponent<Rigidbody>();
    // if (rb != null)
    // {
    //  rb.AddForce(spawnPoint.forward * 500f); // Thay đổi giá trị 500f theo ý muốn để điều chỉnh lực
    //}
  }
}
