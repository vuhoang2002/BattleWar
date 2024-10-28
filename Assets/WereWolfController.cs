using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolfController : MonoBehaviour
{
    // Start is called before the first frame update
    private float defaultMovespeed = 1f;
    public float runMoveSpeed = 1.7f;
    void Start()
    {
        //defaultMovespeed = GetComponent<PlayerController>().moveSpeed;
        defaultMovespeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void RunAction()
    {
        GetComponent<PlayerController>().moveSpeed = runMoveSpeed;
    }
    public void WalkAction()
    {
        GetComponent<PlayerController>().moveSpeed = defaultMovespeed;
    }
}
