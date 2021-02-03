using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float speedShake;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _Shake();
    }
    private void _Shake()
    {
        var post = Input.acceleration.x * speedShake;
        transform.eulerAngles = new Vector3(0, 0, post);




    }    
}
