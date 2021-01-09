﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneItem : MonoBehaviour
{
    [SerializeField] private  SpriteRenderer Rsprite;
    [SerializeField] private Rigidbody2D rigidbody2D;
    public int moveX;
    public int moveY;
    public int timeLoopJump;
    public bool itemCanJump;
    public bool wasJump;
    private void Start()
    {
        StartCoroutine(_Jump());
    }
    private IEnumerator _Jump()
    {  
        while (wasJump == true)
        {
            yield return new WaitForSeconds(timeLoopJump);
            int a = Random.Range(-1, 2);
            rigidbody2D.AddForce(new Vector3(moveX * a, moveY, 0));
            Debug.Log("j");       
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.tag == "wall")
        {
            GameContro.instance._StopTime();
            UI.uI.ChangeUI(UI.MenuUI.menu);
            Destroy(gameObject);
        }
    }
    public void _StopMoving()
    {
        this.rigidbody2D.velocity = Vector3.zero;
        this.rigidbody2D.isKinematic = true;
        this.rigidbody2D.gravityScale = 0;
        this.rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        if ( itemCanJump == true)
        {
            StopCoroutine(_Jump());
            wasJump = false;     
        }
    }
    public void _Moving()
    {
        this.rigidbody2D.isKinematic = false;
        this.rigidbody2D.gravityScale = 1;
        this.rigidbody2D.constraints = RigidbodyConstraints2D.None;
    }
}
