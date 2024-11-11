using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupremeMagician_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> summondObject;
    public int summondCount = 0;
    private int maxSummondCount = 2;
    private Attacks attacks;
    private Animator animator;
    public GameObject explosionWhenSummondToken;

    void Start()
    {
        attacks = GetComponent<Attacks>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }
    public void SummondedSkull()
    {
        if (summondCount < maxSummondCount)
        {
            int randomIndex = Random.Range(0, summondObject.Count);
            bool direction = GetComponent<PlayerController>().isRightWay;
            int xAxisSummond = direction ? 1 : -1;
            Vector3 summondPosition = transform.position + new Vector3(xAxisSummond, 0, 0);
            Creat_Gas_WhenSummondTokenSkull(summondPosition);
            GameObject summondSkull_Ins = Instantiate(summondObject[randomIndex], summondPosition, Quaternion.identity);

            summondCount++;
            if (summondCount == 1)
            {
                //skullOne = summondSkull_Ins;
            }
            else if (summondCount == 2)
            {
                /// skullTwo = summondSkull_Ins;
            }
            summondSkull_Ins.SetActive(true);
            string parentName = gameObject.CompareTag("Player") ? "PlayerList(Clone)" : "EnemyList(Clone)";
            // Đặt parent cho summondSkull_Ins
            Transform parent = GameObject.Find(parentName).transform;
            summondSkull_Ins.transform.SetParent(parent);
        }
    }
    public void Creat_Gas_WhenSummondTokenSkull(Vector3 position)
    {
        GameObject gas_Ins = Instantiate(explosionWhenSummondToken, position, Quaternion.identity);
        gas_Ins.SetActive(true);
    }
    public IEnumerator Abl_NuclerExplosion()
    {
        yield return new WaitForSeconds(1f);
    }
}
