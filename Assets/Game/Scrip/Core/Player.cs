using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Vector3 post;
    public bool wasTouch;


    // Update is called once per frame
    void Update()
    {
        _Rotate();
    }
    public void _Rotate()
    {
         if (Input.GetMouseButton(0))
         {
             post = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if (TestButton.instance.wasTouch == true)
            { 
                transform.DORotate(new Vector3(0, 0, post.x) * 60, 0.01f);
            }
         }
    }


}
