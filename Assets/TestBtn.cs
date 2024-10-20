using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyManager eb;
    void Start()
    {

    }

    // Update is called once per frame
    public void OnMouseDown()
    {
        eb.SpawnEnemy("E_Knight", 3);
        eb.SpawnEnemy("E_Medusa", 3);
        eb.SpawnEnemy("E_Archer_Skl", 5);
    }
}
