using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSamurai_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject arrowAbility;
    public Transform spawnArrow;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Shot_PenetratingShot()
    {
        GameObject ins = Instantiate(arrowAbility, spawnArrow.position, Quaternion.identity);
        ins.GetComponent<ArrowAbility>().SetUp(GetComponent<PlayerController>().isRightWay);
        ins.SetActive(true);

    }
}
