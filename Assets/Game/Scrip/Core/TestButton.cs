using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    public static TestButton instance;

    public  bool wasTouch;
    private void Awake()
    {
        instance = this;
    }
    private void OnMouseDown()
    {
        wasTouch = true;
    }
    private void OnMouseUp()
    {
        wasTouch = false;
    }
}
