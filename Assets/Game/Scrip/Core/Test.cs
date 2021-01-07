using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public Vector3 post;

    // Start is called before the first frame update
    void Start()
    {

    }

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
            transform.DORotate(new Vector3(0, 0, post.x) * 25, 0.01f);


        }

    }
}
