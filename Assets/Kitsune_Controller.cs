using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitsune_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> foxInferno;
    public List<Transform> foxInfernoPosition;
    PlayerController pl;
    Attacks atks;
    public float timer = 0.3f;
    public int dmg;
    public int extraDmg;
    public WeightUnit weightUnitEnemyExtra;

    void Start()
    {
        pl = GetComponent<PlayerController>();
        atks = GetComponent<Attacks>();
        dmg = atks.abl1_Atk;
        extraDmg = atks.extraDmg;
        weightUnitEnemyExtra = GetComponent<UnitClass>().extraDMGWeight;

    }

    // Update is called once per frame

    public IEnumerator FireSevenFoxInferno()
    {
        int i = 0;
        foreach (Transform position in foxInfernoPosition)
        {
            GameObject instance = Instantiate(foxInferno[i], position.position, Quaternion.identity);
            instance.GetComponent<MeleAttack>().SetUp_MeleAttack(dmg, extraDmg, weightUnitEnemyExtra);
            instance.GetComponent<ArrowAbility>().SetUp(pl.isRightWay);
            instance.SetActive(true);
            i++;
            yield return new WaitForSeconds(timer);
        }
    }
}
