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
    public GameObject nuclearExplosion;
    public int nuclear_explosionCount;
    public float spaceBtwNuclerExplosion;


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
    public IEnumerator SummondedSupremeObject()
    {

        if (summondCount < maxSummondCount)
        {
            int randomIndex = Random.Range(0, summondObject.Count);
            Debug.Log("Random index là " + randomIndex);
            bool direction = GetComponent<PlayerController>().isRightWay;
            int xAxisSummond = direction ? 1 : -1;
            Vector3 summondPosition = transform.position + new Vector3(xAxisSummond, 0, 0);
            Creat_Gas_WhenSummondTokenSkull(summondPosition);
            GameObject summondObject_Ins = Instantiate(summondObject[randomIndex], summondPosition, Quaternion.identity);
            SummondToken summondToken = summondObject_Ins.AddComponent<SummondToken>();
            summondToken.masterOfToken = this.gameObject;
            summondCount++;
            if (summondCount == 1)
            {
                //skullOne = summondSkull_Ins;
            }
            else if (summondCount == 2)
            {
                /// skullTwo = summondSkull_Ins;
            }
            summondObject_Ins.SetActive(true);
            string parentName = gameObject.CompareTag("Player") ? "PlayerList(Clone)" : "EnemyList(Clone)";
            // Đặt parent cho summondSkull_Ins
            Transform parent = GameObject.Find(parentName).transform;
            summondObject_Ins.transform.SetParent(parent);
            yield return new WaitForEndOfFrame();
        }
    }
    public void Creat_Gas_WhenSummondTokenSkull(Vector3 position)
    {
        GameObject gas_Ins = Instantiate(explosionWhenSummondToken, position, Quaternion.identity);
        gas_Ins.SetActive(true);
    }
    public IEnumerator Abl_NuclerExplosion()
    {
        Debug.Log("eXPLOSIO");
        float space = GetComponent<PlayerController>().isRightWay ? spaceBtwNuclerExplosion : -spaceBtwNuclerExplosion;

        for (int i = 0; i < nuclear_explosionCount; i++)
        {
            GameObject ins = Instantiate(nuclearExplosion, transform.position + new Vector3(space * (i + 1), 0f, 0f), Quaternion.identity);
            ins.GetComponent<MeleAttack>().SetUp_MeleAttack(attacks.abl1_Atk, attacks.extraDmg, attacks.extraWeight);
            ins.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
