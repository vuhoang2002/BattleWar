using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthGolem_Controller : MonoBehaviour

{
    // Start is called before the first frame update
    public GameObject explosionEarth;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AttackAndCreateExplosionEarth()
    {
        bool isRightWay = GetComponent<PlayerController>().isRightWay;
        int direction = isRightWay ? 1 : -1;
        GameObject explosion_Ins = Instantiate(explosionEarth, transform.position + new Vector3(1f, 0.2f, 0), Quaternion.identity);
        explosion_Ins.SetActive(true);
    }
}
