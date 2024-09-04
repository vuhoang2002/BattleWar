using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight_move : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Animator amt;


    void Start()
    {
        amt = GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            amt.SetBool("isRunning", true);
            transform.Translate(Time.deltaTime *moveSpeed, 0, 0);
            transform.localScale= new Vector2(Mathf.Abs(transform.localScale.x), 1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            amt.SetBool("isRunning", true);
            //move
             transform.Translate(-Time.deltaTime *moveSpeed, 0, 0);
             //flip
              transform.localScale= new Vector2(-Mathf.Abs(transform.localScale.x), 1);
        }
         else if (Input.GetKey(KeyCode.W))
        {
            amt.SetBool("isRunning", true);
            //move
           transform.Translate(0, Time.deltaTime * moveSpeed, 0);
             
             //flip
             // transform.localScale= new Vector2(-Mathf.Abs(transform.localScale.x), 1);
        } else if (Input.GetKey(KeyCode.S))
        {
            amt.SetBool("isRunning", true);
            //move
           transform.Translate(0, -Time.deltaTime * moveSpeed, 0);
             
             //flip
             // transform.localScale= new Vector2(-Mathf.Abs(transform.localScale.x), 1);
        }
        else
        {
            amt.SetBool("isRunning", false);
        }
    }


}