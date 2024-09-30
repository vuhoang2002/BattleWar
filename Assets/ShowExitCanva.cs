using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowExitCanva : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject exitCanva;
    public bool show=true;
    void Start()
    {
        
    }

    // Update is called once per frame
   public void  OnMouseDown(){
   
        exitCanva.SetActive(show);   }
}
