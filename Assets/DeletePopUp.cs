using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePopUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void DestroyPopup()
{
    Destroy(gameObject); // Xóa đối tượng này
}
}
