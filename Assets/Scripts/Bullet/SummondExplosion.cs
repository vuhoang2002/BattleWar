using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummondExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefabExpllosion;
    void Start()
    {

    }

    // Update is called once per frame
    public void Creat_Explosion(Vector3 position)
    {
        GameObject explosionIns = Instantiate(prefabExpllosion, position, Quaternion.identity);
    }
    public void DestroyExplolsion()
    {
        Destroy(this.gameObject);
    }
}
