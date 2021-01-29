using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [SerializeField] float speed ;

    public Vector3 post;
    public bool wasDrag;
    public Vector3 postDown;
    float xBegin = 0;
    Vector3 rotBegin = Vector3.zero;
    [SerializeField] private Camera cameGameCore;

    // Update is called once per frame
    void Update()
    {
        _Rotate();
    }
    public void _Rotate()
    {
         if(Input.GetMouseButtonDown(0))
        {       
            postDown = cameGameCore.ScreenToViewportPoint(Input.mousePosition);
            xBegin = postDown.x;
            rotBegin = transform.eulerAngles;
            SoundManager.Play("taptap");
  
        }            

         if (Input.GetMouseButton(0))
         {  
             post = cameGameCore.ScreenToViewportPoint(Input.mousePosition);     
            if (TestButton.instance.wasTouch == true && post != postDown)
            {
                transform.eulerAngles = rotBegin + new Vector3(0, 0, post.x - xBegin) * speed;
            }
        }
       
        //SoundManager.Play("");
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //SimplePool.Despawn(GameCoreManager.coreManager.effect.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.tag == "Item")
        {
            _SpawnSmoke();
            SoundManager.Play("masat3");
        }
     

    } 
    private void _SpawnSmoke()
    {
        Instantiate(GameCoreManager.coreManager.effect.gameObject, new Vector2(GameCoreManager.coreManager.clone.transform.position.x, GameCoreManager.coreManager.clone.transform.position.y - 0.5f), Quaternion.identity);
    }
}
