using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconOrder : MonoBehaviour
{
    // Start is called before the first frame update
    private float timeToDestroy=1.5f;
    void Start()
    {

    }

    // Update is called once per frame
    public void SetIconOrder(PlayerController playerController)
    {
        if (playerController == null)
        {
            Debug.LogWarning("PlayerController không hợp lệ.");
            return;
        }
        GameObject playerObject = playerController.gameObject;
        GetComponent<Renderer>().sortingOrder = playerController.GetOrderLayer();
        transform.SetParent(playerObject.transform);
        StartCoroutine(Destroy_After_Time());
    }
    public IEnumerator Destroy_After_Time()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(this.gameObject);

    }

}

