using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skl_Mage_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject summondSkull;
    public int summondCount = 0;
    private int maxSummondCount = 2;
    private Attacks attacks;
    private Animator animator;
    void Start()
    {
        attacks = GetComponent<Attacks>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (summondCount < maxSummondCount)
        {
            float cdTimeAbl1 = attacks.GetAtk1_Cd();
            if (cdTimeAbl1 <= 0)
            {
                animator.SetTrigger("isAbl1");
                attacks.SetAtk1_CdFullTime();
            }
        }
    }
    public void SummondedSkull()
    {
        if (summondCount < maxSummondCount)
        {
            bool direction = GetComponent<PlayerController>().isRightWay;
            int xAxisSummond = direction ? 1 : -1;
            GameObject summondSkull_Ins = Instantiate(summondSkull, transform.position + new Vector3(xAxisSummond, 0, 0), Quaternion.identity);
            summondCount++;

            summondSkull_Ins.SetActive(true);
            string parentName = gameObject.CompareTag("Player") ? "PlayerList(Clone)" : "EnemyList(Clone)";
            // Đặt parent cho summondSkull_Ins

            Transform parent = GameObject.Find(parentName).transform;
            summondSkull_Ins.transform.SetParent(parent);
        }
    }
}
