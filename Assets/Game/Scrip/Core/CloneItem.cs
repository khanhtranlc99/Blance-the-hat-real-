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
    //[SerializeField] private ParticleSystem effect;
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
    //public bool wasTouch;

    private bool isLanded;

    private void OnEnable()
    {
        isLanded = false;
    }

    private void OnDisable()
    {
        isLanded = true;
    }

    private void Start()
    {
        StartCoroutine(_Jump());
        //effect.Stop(effect);
    }
    private IEnumerator _Jump()
    {  
        while (wasJump == true)
        {
            yield return new WaitForSeconds(timeLoopJump);
            itemAnimManager.PlayJumpAnim();
            int a = Random.Range(-1, 2);
            rigidbody2D.AddForce(new Vector3(moveXJump * a, moveYJump , 0));
            Debug.Log("j");
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player" && !isLanded)
        {
            isLanded = true;
            itemAnimManager?.PlayLandAnim();
        }

        if( collision.gameObject.tag == "wall")
        {
            //GameContro.instance._StopTime();
            StopAllCoroutines();
            MakeEnemy.make.wasBool = false;
            if (GameCoreManager.coreManager.wood != null)
            {
                SimplePool.Despawn(GameCoreManager.coreManager.wood.gameObject);
            }

            GameStateManager.WaitGameOver(null);
            UIcontro.uIcontro.uiEndGame._PrinTime();
            Destroy(gameObject);
        }
       

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
}
