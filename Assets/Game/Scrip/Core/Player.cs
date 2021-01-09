using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 30;

    public Vector3 post;
    public bool wasDrag;
    public Vector3 postDown;
    float xBegin = 0;
    Vector3 rotBegin = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        _Rotate();
    }
    public void _Rotate()
    {
         if(Input.GetMouseButtonDown(0))
        {
            postDown = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            xBegin = postDown.x;
            rotBegin = transform.eulerAngles;
        }            

         if (Input.GetMouseButton(0))
         {  
             post = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        
            if (TestButton.instance.wasTouch == true && post != postDown)
            {
                transform.eulerAngles = rotBegin + new Vector3(0, 0, post.x - xBegin) * speed;
            }
        }
    }
  
}
