using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloneItem : MonoBehaviour
{
    [SerializeField] private  SpriteRenderer Rsprite;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private PhysicsMaterial2D physics;
    [SerializeField] private ItemAnimManager itemAnimManager;
   
    public float friction;
    public float bounciness;
    public float mass;
    public bool itemCanJump;
    public bool wasJump;  
    public float moveXJump;
    public float moveYJump;
    public float timeLoopJump;
    public float waterForce;
    public float ballForce;
    public float boomForce;
    public bool ball;

    private bool isLanded;
    public bool inheath;
    private void OnEnable()
    {
        this.RegisterListener((int) EventID.GameLose, OnGameLoseHandler);
        isLanded = false;
    }

    private void OnDisable()
    {
        EventDispatcher.Instance?.RemoveListener((int) EventID.GameLose, OnGameLoseHandler);
        isLanded = true;
    }

    private void Start()
    {
        inheath = false;
        StartCoroutine(_Jump());
        //effect.Stop(effect);
    }
    private IEnumerator _Jump()
    {  
        while (wasJump == true)
        {
            yield return new WaitForSeconds(timeLoopJump);
            itemAnimManager?.PlayJumpAnim();
            int a = Random.Range(-1, 2);
            rigidbody2D.AddForce(new Vector3(moveXJump * a, moveYJump , 0));
            Debug.Log("j");
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      

        if (collision.gameObject.tag.Equals(Constant.PLAYER_TAG) && !isLanded)
        {
            isLanded = true;
    ;
        }
        if (ball == true)
        {

            rigidbody2D.AddForce(new Vector2(0, 150));

        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //inheath = false;
    }
    public void _LoadData()
    {
        physics.bounciness = bounciness;
        physics.friction = friction;
        rigidbody2D.mass = mass;
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
    
    private IEnumerator _StopParticle()
    {
        yield return new WaitForSeconds(2);
        //effect.Stop(effect);
    }
 

    private void OnGameLoseHandler(object param)
    {
        StopAllCoroutines();
        GameCoreManager.coreManager.earthWake = false;
        UiEndGame.ui._Sugget();
        Destroy(gameObject);
    }
    private void _SpawnSmoke()
    {
        Instantiate(GameCoreManager.coreManager.smoke.gameObject, new Vector2(GameCoreManager.coreManager.clone.transform.position.x, GameCoreManager.coreManager.clone.transform.position.y - 0.5f), Quaternion.identity);
    }
}
